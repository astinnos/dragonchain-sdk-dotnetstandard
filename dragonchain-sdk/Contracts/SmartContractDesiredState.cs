using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts
{
    public enum SmartContractDesiredState
    {
        [JsonProperty("active")]
        Active,
        [JsonProperty("inactive")]
        Inactive
    }
}