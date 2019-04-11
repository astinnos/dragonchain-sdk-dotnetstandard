using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts.Library.Currency
{
    public enum Governance
    {
        [JsonProperty("ethereum")]
        Ethereum,
        [JsonProperty("bitcoin")]
        Bitcoin,
        [JsonProperty("dragon")]
        Dragon,
        [JsonProperty("custom")]
        Custom
    }
}