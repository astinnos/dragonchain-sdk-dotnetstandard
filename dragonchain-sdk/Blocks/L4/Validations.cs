using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L4
{
    public class Validations
    {
        [JsonProperty(PropertyName = "l3_block_id")]
        public string L3BlockId { get; set; }

        [JsonProperty(PropertyName = "l3_dc_id")]
        public string L3DcId { get; set; }

        [JsonProperty(PropertyName = "l3_proof")]
        public string L3Proof { get; set; }              

        public bool Valid { get; set; }        
    }    
}