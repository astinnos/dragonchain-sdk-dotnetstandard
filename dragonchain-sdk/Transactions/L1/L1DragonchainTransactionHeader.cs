using Newtonsoft.Json;

namespace dragonchain_sdk.Transactions.L1
{
    public class L1DragonchainTransactionHeader
    {
        /// <summary>
        /// Name of a smart contract, or 'transaction'
        /// </summary>
        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }
        /// <summary>
        /// The dragonchainId which originally received this transaction
        /// </summary>
        [JsonProperty(PropertyName = "dc_id")]
        public string DcId { get; set; }
        /// <summary>
        /// The GUID of this transaction
        /// </summary>
        [JsonProperty(PropertyName = "txn_id")]
        public string TransactionId { get; set; }
        /// <summary>
        /// Free-form string of search searchable data submitted by the transaction author
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Unix timestamp of when this transaction was first processed
        /// </summary>
        public string Timestamp { get; set; }
        /// <summary>
        /// The block id to which this transaction was fixated
        /// </summary>
        [JsonProperty(PropertyName = "block_id")]
        public string BlockId { get; set; }
        /// <summary>
        /// The optional GUID of a smart-contract transaction which triggered this record.
        /// SC invocation requests are null here, their output will contain the transaction ID of their invokation request transaction
        /// </summary>
        public string Invoker { get; set; }
    }
}