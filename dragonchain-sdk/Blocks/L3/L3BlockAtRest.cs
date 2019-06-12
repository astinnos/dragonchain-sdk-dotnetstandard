using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L3
{    
    public class L3BlockAtRest
    {
        public string Version { get; set; }
        public string Dcrn { get; set; }
        public Common.Header Header { get; set; }
        [JsonProperty(PropertyName = "l2-Validations")]
        public Validations L2Validations { get; set; }
        public Common.Proof Proof { get; set; }
    }
}