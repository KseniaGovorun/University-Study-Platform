using System.Diagnostics;
using NUnit.Framework;
using RestSharp;
using System.Threading.Tasks;

namespace APITests
{
    [TestFixture]
    public class LoadTests
    {
        private RestClient _client;

        [SetUp]
        public void Setup() => _client = new RestClient("https://localhost:5001/api");

        [Test]
        public async Task LoadTest_100ParallelGets()
        {
            var tasks = new Task<RestResponse>[100];
            for (int i = 0; i < 100; i++)
                tasks[i] = _client.ExecuteAsync(new RestRequest("assignments", Method.Get));

            var responses = await Task.WhenAll(tasks);
            foreach (var res in responses) Assert.IsTrue(res.IsSuccessful);
        }

        [Test]
        public async Task LoadTest_50ParallelPosts()
        {
            var tasks = new Task<RestResponse>[50];
            for (int i = 0; i < 50; i++)
            {
                var req = new RestRequest("assignments", Method.Post);
                req.AddJsonBody(new { title = $"Test {i}", description = "Load" });
                tasks[i] = _client.ExecuteAsync(req);
            }
            var responses = await Task.WhenAll(tasks);
            foreach (var res in responses) Assert.IsTrue(res.IsSuccessful);
        }

        [Test]
        public async Task LoadTest_500GetsInLoop()
        {
            for (int i = 0; i < 500; i++)
            {
                var response = await _client.ExecuteAsync(new RestRequest("assignments", Method.Get));
                Assert.IsTrue(response.IsSuccessful);
            }
        }

        [Test]
        public async Task LoadTest_BulkCreateThenGet()
        {
            for (int i = 0; i < 20; i++)
            {
                var req = new RestRequest("assignments", Method.Post);
                req.AddJsonBody(new { title = $"Bulk {i}", description = "Bulk" });
                await _client.ExecuteAsync(req);
            }
            var response = await _client.ExecuteAsync(new RestRequest("assignments", Method.Get));
            Assert.IsTrue(response.IsSuccessful);
        }

        [Test]
        public async Task LoadTest_ParallelPutRequests()
        {
            var tasks = new Task<RestResponse>[20];
            for (int i = 0; i < 20; i++)
            {
                var req = new RestRequest($"assignments/{i + 1}", Method.Put);
                req.AddJsonBody(new { title = "Updated Load" });
                tasks[i] = _client.ExecuteAsync(req);
            }
            var results = await Task.WhenAll(tasks);
            foreach (var res in results) Assert.IsTrue(res.IsSuccessful);
        }

        [Test]
        public async Task LoadTest_DeleteMultiple()
        {
            for (int i = 1; i <= 10; i++)
            {
                var req = new RestRequest($"assignments/{i}", Method.Delete);
                await _client.ExecuteAsync(req);
            }
            Assert.Pass();
        }

        [Test]
        public async Task LoadTest_CreateReadDeleteCycle()
        {
            var post = new RestRequest("assignments", Method.Post);
            post.AddJsonBody(new { title = "Cycle", description = "Test" });
            var createResp = await _client.ExecuteAsync(post);
            Assert.IsTrue(createResp.IsSuccessful);

            var get = new RestRequest("assignments", Method.Get);
            var getResp = await _client.ExecuteAsync(get);
            Assert.IsTrue(getResp.IsSuccessful);
        }

    }
}