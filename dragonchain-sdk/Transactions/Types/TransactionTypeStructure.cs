using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Transactions.Types
{
    public class TransactionTypeStructure
    {
        [JsonProperty(PropertyName = "custom_indexes")]
        public IEnumerable<CustomIndexStructure> CustomIndexes { get; set; }

        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }

        public string Version { get; set; }
    }
}