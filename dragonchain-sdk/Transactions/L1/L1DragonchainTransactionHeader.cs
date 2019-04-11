using Newtonsoft.Json;

namespace dragonchain_sdk.Transactions.L1
{
    public class L1DragonchainTransactionHeader
    {
        [JsonProperty(PropertyName = "block_id")]
        public string BlockId { get; set; }

        [JsonProperty(PropertyName = "dc_id")]
        public string DcId { get; set; }

        public string Invoker { get; set; }
        public string Tag { get; set; }
        public string Timestamp { get; set; }

        [JsonProperty(PropertyName = "txn_id")]
        public string TransactionId { get; set; }

        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }
    }
}