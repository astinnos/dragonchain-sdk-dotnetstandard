using Newtonsoft.Json;

namespace dragonchain_sdk.Interchain.Networks
{
    public enum BitcoinNetwork
    {
        [JsonProperty(PropertyName = "BTC_MAINNET")]
        BTC_MAINNET,
        [JsonProperty(PropertyName = "BTC_TESTNET3")]
        BTC_TESTNET3
    }
}