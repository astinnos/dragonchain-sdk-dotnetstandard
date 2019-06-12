using System.Collections.Generic;
using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L5
{    
    public class L5BlockAtRest
    {
        public string Version { get; set; }
        public string Dcrn { get; set; }
        public Common.Header Header { get; set; }
        [JsonProperty(PropertyName = "l4-blocks")]
        public IEnumerable<string> L4Blocks { get; set; }
        public Proof Proof { get; set; }
    }
}