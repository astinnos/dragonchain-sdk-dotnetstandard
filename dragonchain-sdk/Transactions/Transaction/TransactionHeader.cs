namespace dragonchain_sdk.Transactions.Transaction
{
    public class TransactionHeader
    {        
        public string BlockId { get; set; }        
        public string DcId { get; set; }
        public string Invoker { get; set; }
        public string Tag { get; set; }
        public string Timestamp { get; set; }        
        public string TxnId { get; set; }        
        public string TxnType { get; set; }
    }
}