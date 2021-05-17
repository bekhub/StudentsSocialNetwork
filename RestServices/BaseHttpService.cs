using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestServices
{
    public class BaseHttpService
    {
        protected readonly HttpClient HttpClient;

        public BaseHttpService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected async Task<T> GetAsync<T>(string uri)
            where T : class
        {
            var result = await HttpClient.GetAsync(uri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        protected async Task<T> DeleteAsync<T>(string uri, int id)
            where T : class
        {
            var result = await HttpClient.DeleteAsync($"{uri}/{id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        protected async Task<T> PostAsync<T>(string uri, object dataToSend)
            where T : class
        {
            var content = ToJson(dataToSend);

            var result = await HttpClient.PostAsync(uri, content);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        protected async Task<T> PutAsync<T>(string uri, object dataToSend)
            where T : class
        {
            var content = ToJson(dataToSend);

            var result = await HttpClient.PutAsync(uri, content);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        private static StringContent ToJson(object obj)
        {
            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static async Task<T> FromHttpResponseMessage<T>(HttpResponseMessage result)
        {
            return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
    }
}
