using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.Common
{
    public class Header
    {
        [JsonProperty(PropertyName = "dc_id")]
        public string DcId { get; set; }
        [JsonProperty(PropertyName = "current_ddss")]
        public string CurrentDDSS { get; set; }
        public string Level { get; set; }
        [JsonProperty(PropertyName = "block_id")]
        public string BlockId { get; set; }
        public string Timestamp { get; set; }
        [JsonProperty(PropertyName = "prev_proof")]
        public string PreviousProof { get; set; }        
    }
}