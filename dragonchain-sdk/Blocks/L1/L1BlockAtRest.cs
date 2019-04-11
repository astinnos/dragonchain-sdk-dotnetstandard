using System.Collections.Generic;

namespace dragonchain_sdk.Blocks.L1
{
    public class L1BlockAtRest
    {
        public string Dcrn { get; set; }        
        public Common.Proof Proof { get; set; }
        public string Version { get; set; }
        public Header Header { get; set; }
        public IEnumerable<string> Transactions { get; set; }        
    }
}