using System;

namespace dragonchain_sdk.Framework.Web
{
    public class DragonchainApiException : Exception
    {
        public DragonchainApiException(DragonchainApiError error)
            : base($"{error.Type}: {error.Details}") { }
    }    
}