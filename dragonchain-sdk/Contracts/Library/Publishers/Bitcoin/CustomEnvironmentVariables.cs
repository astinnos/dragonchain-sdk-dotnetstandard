using dragonchain_sdk.Contracts.Library.Networks;

namespace dragonchain_sdk.Contracts.Library.Publishers.Bitcoin
{
    public class CustomEnvironmentVariables
    {
        public string AuthorizationHeader { get; set; }
        public BitcoinNetwork Network { get; set; }
    }
}