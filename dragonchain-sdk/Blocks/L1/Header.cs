using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.L1
{
    public class Header : Common.Header
    {
        [JsonProperty(PropertyName = "prev_id")]
        public string PreviousId { get; set; }
    }
}