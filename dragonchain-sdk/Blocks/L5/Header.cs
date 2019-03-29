using dragonchain_sdk.Blocks.Common;

namespace dragonchain_sdk.Blocks.L5
{
    public class Header : IHeader
    {        
        public string BlockId { get; set; }        
        public string DcId { get; set; }        
        public string Level { get { return "5"; } }        
        public string PrevProof { get; set; }
        public string Timestamp { get; set; }
    }
}