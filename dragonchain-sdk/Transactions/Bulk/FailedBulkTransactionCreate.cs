using Newtonsoft.Json;

namespace dragonchain_sdk.Transactions.Bulk
{
    public class FailedBulkTransactionCreate
    {
        public string Version { get; set; }
        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }
        public object Payload { get; set; }
        public string Tag { get; set; }
    }
}
