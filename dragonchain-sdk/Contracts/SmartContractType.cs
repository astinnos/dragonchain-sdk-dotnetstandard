using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts
{
    public enum SmartContractType
    {
        [JsonProperty("cron")]
        Cron,
        [JsonProperty("transaction")]
        Transaction        
    }
}