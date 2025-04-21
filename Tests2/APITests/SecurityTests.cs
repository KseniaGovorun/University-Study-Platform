using System.Diagnostics;
using NUnit.Framework;
using RestSharp;
using System.Threading.Tasks;

namespace APITests
{
    [TestFixture]
    public class SecurityTests
    {
        private RestClient _client;

        [SetUp]
        public void Setup() => _client = new RestClient("https://localhost:5001/api");

        [Test]
        public async Task GetWithoutToken_ShouldFail()
        {
            var request = new RestRequest("assignments", Method.Get);
            var response = await _client.ExecuteAsync(request);
            Assert.AreEqual(401, (int)response.StatusCode);
        }

        [Test]
        public async Task PostWithInvalidToken_ShouldFail()
        {
            var request = new RestRequest("assignments", Method.Post);
            request.AddHeader("Authorization", "Bearer FAKE_TOKEN");
            request.AddJsonBody(new { title = "Hack" });
            var response = await _client.ExecuteAsync(request);
            Assert.AreEqual(401, (int)response.StatusCode);
        }

        [Test]
        public async Task SqlInjection_ShouldBeBlocked()
        {
            var request = new RestRequest("assignments", Method.Post);
            request.AddHeader("Authorization", "Bearer VALID_TOKEN");
            request.AddJsonBody(new { title = "'; DROP TABLE Users; --" });
            var response = await _client.ExecuteAsync(request);
            Assert.AreNotEqual(500, (int)response.StatusCode);
        }

        [Test]
        public async Task XssInjection_ShouldNotBeRendered()
        {
            var request = new RestRequest("assignments", Method.Post);
            request.AddHeader("Authorization", "Bearer VALID_TOKEN");
            request.AddJsonBody(new { title = "<script>alert('xss')</script>" });
            var response = await _client.ExecuteAsync(request);
            Assert.AreNotEqual(500, (int)response.StatusCode);
        }

        [Test]
        public async Task AccessWithStudentRole_ToDelete_ShouldBeForbidden()
        {
            var request = new RestRequest("assignments/1", Method.Delete);
            request.AddHeader("Authorization", "Bearer STUDENT_TOKEN");
            var response = await _client.ExecuteAsync(request);
            Assert.AreEqual(403, (int)response.StatusCode);
        }

        [Test]
        public async Task AccessToOtherUserData_ShouldBeRestricted()
        {
            var request = new RestRequest("assignments/999", Method.Get);
            request.AddHeader("Authorization", "Bearer OTHER_USER_TOKEN");
            var response = await _client.ExecuteAsync(request);
            Assert.AreEqual(403, (int)response.StatusCode);
        }

        [Test]
        public async Task TamperedToken_ShouldBeRejected()
        {
            var request = new RestRequest("assignments", Method.Get);
            request.AddHeader("Authorization", "Bearer TAMPERED_TOKEN");
            var response = await _client.ExecuteAsync(request);
            Assert.AreEqual(401, (int)response.StatusCode);
        }
    }
}