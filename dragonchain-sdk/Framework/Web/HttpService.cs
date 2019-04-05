using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using dragonchain_sdk.Credentials;

namespace dragonchain_sdk.Framework.Web
{
    internal class HttpService : IHttpService
    {
        private ICredentialService _credentialService;
        private HttpClient _httpClient;        
        const string DefaultContentType = "application/json";

        public HttpService(ICredentialService credentialService, string endpoint)
        {
            _credentialService = credentialService;            
            _httpClient = CreateHttpClient(endpoint);
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string path)
        {
            var request = CreateRequest(HttpMethod.Get, path);
            return await HandleResponseAsync<T>(await _httpClient.SendAsync(request));            
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string path, object body)
        {
            var jsonBody = JsonConvert.SerializeObject(body, CreateJsonSerializerSettings());
            var request = CreateRequest(HttpMethod.Post, path, jsonBody);                           
            return await HandleResponseAsync<T>(await _httpClient.SendAsync(request));                                        
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string path, object body)
        {
            var jsonBody = JsonConvert.SerializeObject(body, CreateJsonSerializerSettings());
            var request = CreateRequest(HttpMethod.Put, path, jsonBody);
            return await HandleResponseAsync<T>(await _httpClient.SendAsync(request));            
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string path)
        {            
            var request = CreateRequest(HttpMethod.Delete, path);
            return await HandleResponseAsync<T>(await _httpClient.SendAsync(request));            
        }

        public void SetEndpoint(string endpoint)
        {            
            _httpClient.BaseAddress = new Uri(endpoint);
        }               

        private HttpClient CreateHttpClient(string endpoint)
        {            
            return new HttpClient
            {
                BaseAddress = new Uri(endpoint)
            };                        
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string path, string body = "", string contentType = DefaultContentType, string callbackURL = "")
        {
            var timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var request = new HttpRequestMessage(method, path)
            {
                Content = new StringContent(body)
            };            
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(DefaultContentType);            
            request.Headers.TryAddWithoutValidation("dragonchain", _credentialService.DragonchainId);
            request.Headers.TryAddWithoutValidation("X-Callback-URL", callbackURL);
            request.Headers.TryAddWithoutValidation("Authorization", _credentialService.GetAuthorizationHeader(method.ToString(),
                path,
                timeStamp,
                contentType,
                body
                ));
            request.Headers.TryAddWithoutValidation("timestamp", timeStamp);
            return request;
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