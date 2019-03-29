using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using dragonchain_sdk.Credentials;

namespace dragonchain_sdk.Framework.Web
{
    internal class HttpService : IHttpService
    {
        private ICredentialService _credentialService;
        const string DefaultContentType = "application/json";

        public HttpService(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string path)
        {
            using (var httpClient = CreateHttpClient("GET", path))
            {                
                return await HandleResponseAsync<T>(await httpClient.GetAsync(path));                
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string path, object body)
        {
            var jsonBody = JsonConvert.SerializeObject(body, CreateJsonSerializerSettings());
            using (var httpClient = CreateHttpClient("POST", path, jsonBody))
            {                
                var content = new StringContent(jsonBody, Encoding.UTF8, DefaultContentType);                
                return await HandleResponseAsync<T>(await httpClient.PostAsync(path, content));                
            }            
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string path, object body)
        {
            var jsonBody = JsonConvert.SerializeObject(body, CreateJsonSerializerSettings());
            using (var httpClient = CreateHttpClient("PUT", path, jsonBody))            {
                
                var content = new StringContent(jsonBody, Encoding.UTF8, DefaultContentType);
                return await HandleResponseAsync<T>(await httpClient.PutAsync(path, content));
            }            
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string path)
        {
            using (var httpClient = CreateHttpClient("DELETE", path))
            {                
                return await HandleResponseAsync<T>(await httpClient.DeleteAsync(path));
            }            
        }

        private HttpClient CreateHttpClient(string method, string path, string body = "", string contentType = DefaultContentType)
        {
            var client = new HttpClient();            
            client.DefaultRequestHeaders.TryAddWithoutValidation("dragonchain", _credentialService.DragonchainId);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _credentialService.GetAuthorizationHeader(method, 
                path, 
                DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), 
                contentType, 
                body
                ));
            return client;
        }               

        private async Task<ApiResponse<T>> HandleResponseAsync<T>(HttpResponseMessage response)
        {            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();                
                var jsonBody = JsonConvert.DeserializeObject<T>(json, CreateJsonSerializerSettings());
                return new ApiResponse<T> { Ok = true, Status = (int)response.StatusCode, Response = jsonBody };
            }
            return new ApiResponse<T> { Ok = false, Status = (int)response.StatusCode, Response = default(T) };
        }

        private JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()                
            };
            var settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver                
            };
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return settings;
        }
    }
}