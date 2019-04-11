using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts.Library.Networks
{
    public enum BitcoinNetwork
    {
        [JsonProperty("BTC")]
        BTC,
        [JsonProperty("testnet3")]
        Testnet3        
    }
}