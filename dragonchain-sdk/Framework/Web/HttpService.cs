using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using dragonchain_sdk.Credentials;
using System.Net.Http.Headers;

namespace dragonchain_sdk.Framework.Web
{
    internal class HttpService : IHttpService
    {
        private ICredentialService _credentialService;
        private string _endpoint;
        const string DefaultContentType = "application/json";

        public HttpService(ICredentialService credentialService, string endpoint)
        {
            _credentialService = credentialService;
            _endpoint = endpoint;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string path)
        {
            using (var httpClient = CreateHttpClient("GET", path))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, path)
                {
                    Content = new StringContent("")
                };
                request.Content.Headers.ContentType = new MediaTypeHeaderValue(DefaultContentType);                                
                return await HandleResponseAsync<T>(await httpClient.SendAsync(request));
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

        public void SetEndpoint(string endPoint)
        {
            _endpoint = endPoint;
        }

        private HttpClient CreateHttpClient(string method, string path, string body = "", string contentType = DefaultContentType, string callbackURL = "")
        {
            var timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var client = new HttpClient
            {
                BaseAddress = new Uri(_endpoint)
            };
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", contentType);
            client.DefaultRequestHeaders.TryAddWithoutValidation("dragonchain", _credentialService.DragonchainId);
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Callback-URL", callbackURL);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _credentialService.GetAuthorizationHeader(method, 
                path,
                timeStamp, 
                contentType, 
                body
                ));
            client.DefaultRequestHeaders.TryAddWithoutValidation("timestamp", timeStamp);            
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
            var errorJson = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<DragonchainApiErrorResponse>(errorJson, CreateJsonSerializerSettings());
            throw new DragonchainApiException(errorResponse.Error);             
        }

        private JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()                
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