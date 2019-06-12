using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L1
{
    public class Header
    {
        [JsonProperty(PropertyName = "dc_id")]
        public string DcId { get; set; }
        [JsonProperty(PropertyName = "block_id")]
        public string BlockId { get; set; }
        public string Level { get; set; }
        public string Timestamp { get; set; }
        [JsonProperty(PropertyName = "prev_id")]
        public string PreviousId { get; set; }
        [JsonProperty(PropertyName = "prev_proof")]
        public string PreviousProof { get; set; }
    }
}