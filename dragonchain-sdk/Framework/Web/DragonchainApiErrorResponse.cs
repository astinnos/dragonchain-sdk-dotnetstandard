namespace dragonchain_sdk.Framework.Web
{
    internal class DragonchainApiErrorResponse
    {
        public DragonchainApiError Error { get; set; }
    }

    public class DragonchainApiError
    {
        public string Type { get; set; }
        public string Details { get; set; }
    }
}