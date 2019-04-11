using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Transactions.Types
{
    public class DragonchainTransactionTypeQueryResult
    {
        [JsonProperty(PropertyName = "transaction_types")]
        public IEnumerable<TransactionTypeResponse> TransactionTypes { get; set; }
    }
}