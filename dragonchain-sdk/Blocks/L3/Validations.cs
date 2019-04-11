using System.Collections.Generic;
using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L3
{
    public class Validations
    {
        public IEnumerable<string> Clouds { get; set; }        
        public string Count { get; set; }
        public string Ddss { get; set; }

        [JsonProperty(PropertyName = "l1_block_id")]
        public string L1BlockId { get; set; }

        [JsonProperty(PropertyName = "l1_dc_id")]
        public string L1DcId { get; set; }

        [JsonProperty(PropertyName = "l1_proof")]
        public string L1Proof { get; set; }

        public IEnumerable<string> Regions { get; set; }
    }
}