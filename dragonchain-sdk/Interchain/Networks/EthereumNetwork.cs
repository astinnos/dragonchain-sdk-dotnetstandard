using Newtonsoft.Json;

namespace dragonchain_sdk.Interchain.Networks
{
    public enum EthereumNetwork
    {
        [JsonProperty(PropertyName = "ETH_MAINNET")]
        ETH_MAINNET,
        [JsonProperty(PropertyName = "ETH_ROPSTEN")]
        ETH_ROPSTEN,
        [JsonProperty(PropertyName = "ETC_MAINNET")]
        ETC_MAINNET,
        [JsonProperty(PropertyName = "ETC_MORDEN")]
        ETC_MORDEN
    }
}