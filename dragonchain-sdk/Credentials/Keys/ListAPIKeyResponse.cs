using System.Collections.Generic;

namespace dragonchain_sdk.Credentials.Keys
{
    public class ListAPIKeyResponse
    {
        public IEnumerable<GetAPIKeyResponse> Keys { get; set; }
    }
}