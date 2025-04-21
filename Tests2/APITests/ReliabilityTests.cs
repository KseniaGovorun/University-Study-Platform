using System.Diagnostics;
using NUnit.Framework;
using RestSharp;
using System.Threading.Tasks;

namespace APITests
{
    [TestFixture]
    public class ReliabilityTests
    {
        private RestClient _client;

        [SetUp]
        public void Setup() => _client = new RestClient("https://localhost:5001/api");

        [Test]
        public async Task RepeatGet100Times_ShouldSucceed()
        {
            for (int i = 0; i < 100; i++)
            {
                var request = new RestRequest("assignments", Method.Get);
                var response = await _client.ExecuteAsync(request);
                Assert.IsTrue(response.IsSuccessful);
            }
        }

        [Test]
        public async Task CreateThenGet_DeleteThenGet()
        {
            var post = new RestRequest("assignments", Method.Post);
            post.AddJsonBody(new { title = "Test", description = "Temp" });
            var postResp = await _client.ExecuteAsync(post);

            var get = new RestRequest("assignments", Method.Get);
            var getResp = await _client.ExecuteAsync(get);
            Assert.IsTrue(getResp.IsSuccessful);

            var delete = new RestRequest("assignments/1", Method.Delete);
            await _client.ExecuteAsync(delete);

            var getAfterDelete = await _client.ExecuteAsync(get);
            Assert.IsTrue(getAfterDelete.IsSuccessful);
        }

        [Test]
        public async Task LongRunningGetRequests()
        {
            for (int i = 0; i < 1000; i++)
            {
                var req = new RestRequest("assignments", Method.Get);
                var res = await _client.ExecuteAsync(req);
                Assert.IsTrue(res.IsSuccessful);
            }
        }

        [Test]
        public async Task BulkCreateAssignments()
        {
            for (int i = 0; i < 50; i++)
            {
                var req = new RestRequest("assignments", Method.Post);
                req.AddJsonBody(new { title = $"Item {i}", description = "Reliable" });
                var res = await _client.ExecuteAsync(req);
                Assert.IsTrue(res.IsSuccessful);
            }
        }

        [Test]
        public async Task ConsistentStatusCodesInLoop()
        {
            for (int i = 0; i < 50; i++)
            {
                var response = await _client.ExecuteAsync(new RestRequest("assignments", Method.Get));
                Assert.AreEqual(200, (int)response.StatusCode);
            }
        }

        [Test]
        public async Task ParallelCreateAndDelete()
        {
            var create = new RestRequest("assignments", Method.Post);
            create.AddJsonBody(new { title = "Quick", description = "Delete" });
            await _client.ExecuteAsync(create);

            var delete = new RestRequest("assignments/1", Method.Delete);
            var deleteResp = await _client.ExecuteAsync(delete);
            Assert.IsTrue(deleteResp.IsSuccessful);
        }

        [Test]
        public async Task RestartGetAfterFailure()
        {
            var response = await _client.ExecuteAsync(new RestRequest("assignments", Method.Get));
            if (!response.IsSuccessful)
            {
                await Task.Delay(1000);
                response = await _client.ExecuteAsync(new RestRequest("assignments", Method.Get));
            }
            Assert.IsTrue(response.IsSuccessful);
        }
    }
}