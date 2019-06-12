using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Transactions.Types
{
    public class TransactionTypeResponse
    {
        public string Version { get; set; }
        [JsonProperty(PropertyName = "txn_type")]
        public string TxnType { get; set; }
        [JsonProperty(PropertyName = "custom_indexes")]
        public IEnumerable<TransactionTypeCustomIndex> CustomIndexes { get; set; }
        /// <summary>
        /// If this is a ledger contract type, (not assigned to a contract), then this field will simply be the boolean false,
        /// otherwise this will be the string of the associated contract id
        /// </summary>
        [JsonProperty(PropertyName = "contract_id")]
        public object ContractData { get; set; }
    }
}