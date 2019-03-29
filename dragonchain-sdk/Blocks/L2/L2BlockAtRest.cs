using dragonchain_sdk.Blocks.Common;

namespace dragonchain_sdk.Blocks.L2
{
    public class L2BlockAtRest : IBlockAtRest
    {
        public string Dcrn { get { return "Block::L2::AtRest"; } }
        public IHeader Header { get; set; }
        public Proof Proof { get; set; }
        public string Validation { get; set; }
        public string Version { get { return "2"; } }
    }
}