using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L2
{
    public class Validation
    {
        [JsonProperty(PropertyName = "dc_id")]
        public string DcId { get; set; }
        [JsonProperty(PropertyName = "block_id")]
        public string BlockId { get; set; }        
        [JsonProperty(PropertyName = "stripped_proof")]
        public string StrippedProof { get; set; }
        public string Transactions { get; set; }
    }
}