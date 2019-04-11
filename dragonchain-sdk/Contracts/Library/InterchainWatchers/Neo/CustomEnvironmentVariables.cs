using dragonchain_sdk.Contracts.Library.Networks;

namespace dragonchain_sdk.Contracts.Library.InterchainWatchers.Neo
{
    public class CustomEnvironmentVariables
    {
        public string Address { get; set; }
        public string ApiKey { get; set; }
        public NeoNetwork Network { get; set; }
    }
}