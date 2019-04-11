using dragonchain_sdk.Contracts.Library.Networks;

namespace dragonchain_sdk.Contracts.Library.InterchainWatchers.Bitcoin
{
    public class CustomEnvironmentVariables
    {
        public string Address { get; set; }
        public string ApiKey { get; set; }
        public BitcoinNetwork Network { get; set; }
        public string Url { get; set; }
    }
}