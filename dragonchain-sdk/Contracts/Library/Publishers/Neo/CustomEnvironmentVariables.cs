using dragonchain_sdk.Contracts.Library.Networks;

namespace dragonchain_sdk.Contracts.Library.Publishers.Neo
{
    public class CustomEnvironmentVariables
    {
        public string AuthorizationHeader { get; set; }
        public NeoNetwork Network { get; set; }
    }
}