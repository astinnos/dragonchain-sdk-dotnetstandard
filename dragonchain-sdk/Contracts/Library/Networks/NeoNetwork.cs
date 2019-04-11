using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts.Library.Networks
{
    public enum NeoNetwork
    {
        [JsonProperty("neo")]
        Neo,
        [JsonProperty("testnet")]
        Testnet,
        [JsonProperty("NEOtestnet")]
        NeoTestnet
    }
}
