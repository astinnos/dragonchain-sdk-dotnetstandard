using dragonchain_sdk.Contracts.Library.Networks;

namespace dragonchain_sdk.Contracts.Library.InterchainWatchers.Ethereum
{
    public class CustomEnvironmentVariables
    {
        public string Address { get; set; }
        public string Url { get; set; }
        public string Contract { get; set; }
        public string TokenContractAddress { get; set; }
        public string ApiKey { get; set; }
        public EthereumNetwork EthereumNetwork { get; set; }
    }
}