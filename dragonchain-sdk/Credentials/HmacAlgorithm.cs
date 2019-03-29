using Newtonsoft.Json;

namespace dragonchain_sdk.Credentials
{
    public enum HmacAlgorithm
    {        
        SHA256,
        [JsonProperty(PropertyName = "SHA3-256")]
        SHA3_256,
        BLAKE2b512
    }

    public static class HmacAlgorithmExtensions
    {
        public static string ToValue(this HmacAlgorithm hmacAlgo)
        {
            switch (hmacAlgo)
            {                
                case HmacAlgorithm.SHA3_256:
                    return "SHA3-256";                
                default:
                    return hmacAlgo.ToString();
            }
        }
    }
}