using dragonchain_sdk.Contracts.Library.Networks;

namespace dragonchain_sdk.Contracts.Library.Publishers.Ethereum
{
    public class CustomEnvironmentVariables
    {
        public string AuthorizationHeader { get; set; }
        public EthereumNetwork Network { get; set; }
    }
}
