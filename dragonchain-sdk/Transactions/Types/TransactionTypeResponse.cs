using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Transactions.Types
{
    public class TransactionTypeResponse
    {
        [JsonProperty(PropertyName = "custom_indexes")]
        public IEnumerable<CustomIndexStructure> CustomIndexes { get; set; }

        [JsonProperty(PropertyName = "contract_data")]
        public object ContractData { get; set; }

        [JsonProperty(PropertyName = "is_contract")]
        public bool IsContract { get; set; }

        [JsonProperty(PropertyName = "txn_type")]
        public string TxnType { get; set; }      
        
        public string Version { get; set; }
    }
}