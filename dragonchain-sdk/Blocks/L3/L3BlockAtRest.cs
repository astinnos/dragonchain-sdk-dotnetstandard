using Newtonsoft.Json;
using dragonchain_sdk.Blocks.Common;

namespace dragonchain_sdk.Blocks.L3
{
    public class L3BlockAtRest : IBlockAtRest
    {
        public string Dcrn { get { return "Block::L3::AtRest"; } }
        public IHeader Header { get; set; }
        [JsonProperty(PropertyName = "l2-Validations")]
        public Validations L2Validations { get; set; }
        public Proof Proof { get; set; }
        public string Validation { get; set; }
        public string Version { get { return "2"; } }       
    }
}