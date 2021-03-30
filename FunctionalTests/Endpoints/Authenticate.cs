using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Endpoints.Auth;
using Core.Extensions;
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
        [InlineData("demouser@microsoft.com", AuthorizationConstants.DEFAULT_PASSWORD, "some-token")]
        [InlineData("demouser@microsoft.com", "badpassword", null)]
        [InlineData("baduser@microsoft.com", "badpassword", null)]
        public async Task ReturnsExpectedResultGivenCredentials(string testUsername, string testPassword, object token)
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

            Assert.Equal(model.Token.GetType(), token.GetType());
        }
    }
}
