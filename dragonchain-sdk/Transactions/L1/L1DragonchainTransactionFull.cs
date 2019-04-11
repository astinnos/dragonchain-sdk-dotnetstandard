namespace dragonchain_sdk.Transactions.L1
{
    public class L1DragonchainTransactionFull
    {
        public string Dcrn { get; set; }
        public L1DragonchainTransactionHeader Header { get; set; }
        public dynamic Payload { get; set; }
        public L1DragonchainTransactionProof Proof { get; set; }
        public string Version { get; set; }
    }
}