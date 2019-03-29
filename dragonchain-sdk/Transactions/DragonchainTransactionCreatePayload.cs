namespace dragonchain_sdk.Transactions
{
    public class DragonchainTransactionCreatePayload
    {
        public object Payload { get; set; }
        public string Tag { get; set; }
        public string TxnType { get; set; }
        public string Version { get; set; }
    }
}