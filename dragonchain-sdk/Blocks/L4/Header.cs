using dragonchain_sdk.Blocks.Common;

namespace dragonchain_sdk.Blocks.L4
{
    public class Header : IHeader
    {        
        public string BlockId { get; set; }        
        public string DcId { get; set; }        
        public int L1BlockId { get; set; }        
        public int L1DcId { get; set; }        
        public int L1Proof { get; set; }
        public string Level { get { return "4"; } }        
        public string PrevProof { get; set; }
        public string Timestamp { get; set; }
    }
}