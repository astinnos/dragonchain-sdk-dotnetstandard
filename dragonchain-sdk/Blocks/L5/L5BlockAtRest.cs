using System.Collections.Generic;
using Newtonsoft.Json;
using dragonchain_sdk.Blocks.Common;

namespace dragonchain_sdk.Blocks.L5
{
    public class L5BlockAtRest : IBlockAtRest
    {
        public string Dcrn { get { return "Block::L5::AtRest"; } }
        public IHeader Header { get; set; }
        [JsonProperty(PropertyName = "l4-blocks")]
        public IEnumerable<string> L4Blocks { get; set; }
        public Proof Proof { get; set; }        
        public string Version { get { return "2"; } }
    }
}
