using System;

namespace dragonchain_sdk.Framework.Web
{
    public class DragonchainApiException : Exception
    {
        public int Status { get; set; }

        public DragonchainApiException(DragonchainApiError error, int status)
            : base($"{error.Type}: {error.Details}") { }

        public DragonchainApiException(string message):base(message) { }
    }    
}