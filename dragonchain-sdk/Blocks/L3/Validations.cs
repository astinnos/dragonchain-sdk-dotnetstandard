using System.Collections.Generic;
using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L3
{
    public class Validations
    {
        [JsonProperty(PropertyName = "l1_dc_id")]
        public string L1DcId { get; set; }
        [JsonProperty(PropertyName = "l1_block_id")]
        public string L1BlockId { get; set; }
        [JsonProperty(PropertyName = "l1_proof")]
        public string L1Proof { get; set; }
        [JsonProperty(PropertyName = "l2_proofs")]
        public L2Proofs L2Proofs { get; set; }
        public string Ddss { get; set; }
        public string Count { get; set; }
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<string> Clouds { get; set; }
    }

    public class L2Proofs
    {
        [JsonProperty(PropertyName = "dc_id")]
        public string DcId { get; set; }
        [JsonProperty(PropertyName = "block_id")]
        public string BlockId { get; set; }
        public string Proof { get; set; }
    }
}