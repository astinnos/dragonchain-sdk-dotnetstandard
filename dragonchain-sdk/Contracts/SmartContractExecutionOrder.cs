using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts
{
    public enum SmartContractExecutionOrder
    {
        [JsonProperty("parallel")]
        Parallel,
        [JsonProperty("serial")]
        Serial
    }
}