using System.Diagnostics;
using NUnit.Framework;
using RestSharp;
using System.Threading.Tasks;

namespace APITests
{
    [TestFixture]
    public class PerformanceTests
    {
        private RestClient _client;

        [SetUp]
        public void Setup() => _client = new RestClient("https://localhost:5001/api");

        [Test]
        public async Task GetAssignments_ResponseTimeUnder200ms()
        {
            var request = new RestRequest("assignments", Method.Get);
            var sw = Stopwatch.StartNew();
            var response = await _client.ExecuteAsync(request);
            sw.Stop();
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 200);
        }

        [Test]
        public async Task PostAssignment_ResponseTimeUnder300ms()
        {
            var request = new RestRequest("assignments", Method.Post);
            request.AddJsonBody(new { title = "Test", description = "Speed test" });
            var sw = Stopwatch.StartNew();
            var response = await _client.ExecuteAsync(request);
            sw.Stop();
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 300);
        }

        [Test]
        public async Task PutAssignment_ResponseTimeUnder300ms()
        {
            var request = new RestRequest("assignments/1", Method.Put);
            request.AddJsonBody(new { title = "Updated" });
            var sw = Stopwatch.StartNew();
            var response = await _client.ExecuteAsync(request);
            sw.Stop();
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 300);
        }

        [Test]
        public async Task DeleteAssignment_ResponseTimeUnder250ms()
        {
            var request = new RestRequest("assignments/1", Method.Delete);
            var sw = Stopwatch.StartNew();
            var response = await _client.ExecuteAsync(request);
            sw.Stop();
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 250);
        }

        [Test]
        public async Task GetAssignmentById_ResponseTimeUnder200ms()
        {
            var request = new RestRequest("assignments/1", Method.Get);
            var sw = Stopwatch.StartNew();
            var response = await _client.ExecuteAsync(request);
            sw.Stop();
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 200);
        }

        [Test]
        public async Task GetAssignments_AfterCreationIsFast()
        {
            var create = new RestRequest("assignments", Method.Post);
            create.AddJsonBody(new { title = "Load", description = "Performance" });
            await _client.ExecuteAsync(create);

            var get = new RestRequest("assignments", Method.Get);
            var sw = Stopwatch.StartNew();
            var response = await _client.ExecuteAsync(get);
            sw.Stop();
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 250);
        }

        [Test]
        public async Task MultipleSequentialGets_Under500msTotal()
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 5; i++)
            {
                var request = new RestRequest("assignments", Method.Get);
                await _client.ExecuteAsync(request);
            }
            sw.Stop();
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 500);
        }
    }
