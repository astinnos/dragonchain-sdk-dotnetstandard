using Newtonsoft.Json;

namespace dragonchain_sdk.Credentials.Keys
{
    public class GetAPIKeyResponse
    {
        public string Id { get; set; }
        [JsonProperty("registration_time")]
        public int RegistrationTime { get; set; }
    }
}