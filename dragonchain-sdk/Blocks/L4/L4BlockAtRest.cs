using System.Collections.Generic;
using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L4
{
    public class L4BlockAtRest
    {
        public string Dcrn { get; set; }
        public Common.Proof Proof { get; set; }
        public string Version { get; set; }
        public Header Header { get; set; }        
        [JsonProperty(PropertyName = "l3-Validations")]
        public IEnumerable<Validations> L3Validations { get; set; }                   
    }
}