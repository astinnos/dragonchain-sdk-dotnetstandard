using Newtonsoft.Json;

namespace dragonchain_sdk.Transactions
{
    public class DragonchainTransactionCreateResponse
    {
        [JsonProperty(PropertyName = "transaction_id")]        
        public string TransactionId { get; set; }
    }
}