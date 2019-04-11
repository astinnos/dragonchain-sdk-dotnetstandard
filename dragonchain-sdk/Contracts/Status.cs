using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts
{
    public class ContractStatus
    {
        public string State { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }

        public string Timestamp { get; set; }
    }

    public enum Status
    {
        Approved,
        Rejected,
        Pending
    }

    
}
