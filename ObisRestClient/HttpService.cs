using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ObisRestClient
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient, IOptions<ObisApiSettings> apiSettingsOptions)
        {
            _httpClient = httpClient;
            var apiSettings = apiSettingsOptions.Value;
            _httpClient.BaseAddress = new Uri(apiSettings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("appKey", apiSettings.AppKey);
        }

        public async Task<T> GetAsync<T>(string uri)
            where T : class
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                
            }
            var result = await _httpClient.GetAsync(uri);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> DeleteAsync<T>(string uri, int id)
            where T : class
        {
            var result = await _httpClient.DeleteAsync($"{uri}/{id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> PostAsync<T>(string uri, object dataToSend)
            where T : class
        {
            var content = ToJson(dataToSend);

            var result = await _httpClient.PostAsync(uri, content);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> PutAsync<T>(string uri, object dataToSend)
            where T : class
        {
            var content = ToJson(dataToSend);

            var result = await _httpClient.PutAsync(uri, content);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public void SetAuthKey(string authKey)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", authKey);
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
