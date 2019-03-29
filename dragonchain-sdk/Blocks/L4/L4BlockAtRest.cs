using System.Collections.Generic;
using Newtonsoft.Json;
using dragonchain_sdk.Blocks.Common;

namespace dragonchain_sdk.Blocks.L4
{
    public class L4BlockAtRest : IBlockAtRest
    {
        public string Dcrn { get { return "Block::L4::AtRest"; } }
        public IHeader Header { get; set; }
        [JsonProperty(PropertyName = "l3-Validations")]
        public IEnumerable<Validations> L3Validations { get; set; }
        public Proof Proof { get; set; }
        public string Validation { get; set; }
        public string Version { get { return "2"; } }
    }
}