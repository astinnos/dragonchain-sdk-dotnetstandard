namespace dragonchain_sdk.Transactions.Bulk
{    
    public class BulkTransactionPayload
    {
        /// <summary>
        /// The transaction type to use for this new transaction. This transaction type must already exist on the chain (via `createTransactionType`)
        /// </summary>
        public string TransactionType { get; set; }
        /// <summary>
        /// Payload of the transaction. Must be a utf-8 encodable string, or any json object
        /// </summary>
        public object Payload { get; set; }
        /// <summary>
        /// Tag of the transaction which gets indexed and can be searched on for queries
        /// </summary>
        public string Tag { get; set; }        
    }
}