using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts
{
    public class ContractStatus
    {
        public Status State { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }

        public string Timestamp { get; set; }
    }

    public enum Status
    {
        [JsonProperty(PropertyName = "active")]
        Active,
        [JsonProperty(PropertyName = "inactive")]
        Inactive        
    }    
}