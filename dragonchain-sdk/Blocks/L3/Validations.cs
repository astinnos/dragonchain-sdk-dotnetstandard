using System.Collections.Generic;
using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L3
{
    public class Validations
    {
        public IEnumerable<string> Clouds { get; set; }        
        public string Count { get; set; }
        public string Ddss { get; set; }                
        public string L1BlockId { get; set; }        
        public string L1DcId { get; set; }        
        public string L1Proof { get; set; }
        public IEnumerable<string> Regions { get; set; }
    }
}