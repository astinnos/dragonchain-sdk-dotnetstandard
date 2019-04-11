using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts.Library.Networks
{
    public enum EthereumNetwork
    {
        [JsonProperty("classic")]
        Classic,
        [JsonProperty("ropsten")]
        Ropsten,
        [JsonProperty("mainnet")]
        Mainnet,
        [JsonProperty("ropsten-infura")]
        RopstenInfura
    }
}
