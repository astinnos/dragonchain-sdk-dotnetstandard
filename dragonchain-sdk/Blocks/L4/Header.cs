using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L4
{
    public class Header : Common.Header
    {
        [JsonProperty(PropertyName = "l1_block_id")]
        public string L1BlockId { get; set; }
        [JsonProperty(PropertyName = "l1_dc_id")]
        public string L1DcId { get; set; }
        [JsonProperty(PropertyName = "l1_proof")]
        public string L1Proof { get; set; }        
    }
}