using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Blocks.L5
{
    public class Proof : Common.Proof
    {
        [JsonProperty(PropertyName = "block_last_sent_at")]
        public string BlockLastSentAt { get; set; }
        [JsonProperty(PropertyName = "transaction_hash")]
        public IEnumerable<string> TransactionHash { get; set; }        
        public string Network { get; set; }
    }
}
