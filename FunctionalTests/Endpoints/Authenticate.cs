using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Endpoints.Auth;
using Common.Extensions;
using Infrastructure.Identity;
using Xunit;

namespace FunctionalTests.Endpoints
{
    [Collection("Sequential")]
    public class Authenticate :  IClassFixture<ApiTestFixture>
    {
        private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        public HttpClient Client { get; }

        public Authenticate(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }
        
        [Theory]
        [InlineData("demouser@microsoft.com", AuthorizationConstants.DEFAULT_PASSWORD, true)]
        [InlineData("demouser@microsoft.com", "badpassword", false)]
        [InlineData("baduser@microsoft.com", "badpassword", false)]
        public async Task ReturnsExpectedResultGivenCredentials(string testUsername, string testPassword, bool isTokenExist)
        {
            var request = new Request.Authenticate 
            { 
                Username = testUsername,
                Password = testPassword,
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/authenticate", jsonContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<Response.Authenticate>();

            Assert.Equal(model.JwtToken != null, isTokenExist);
        }
    }
}
