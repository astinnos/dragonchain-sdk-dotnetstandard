using Newtonsoft.Json;

namespace dragonchain_sdk.Transactions
{
    public class DragonchainTransactionCreatePayload
    {
        public object Payload { get; set; }
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }
        public string Version { get; set; }
    }
}