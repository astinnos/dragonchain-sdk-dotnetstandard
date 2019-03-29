using dragonchain_sdk.Transactions.Transaction;

namespace dragonchain_sdk.Transactions.L1
{
    public class L1DragonchainTransactionFull
    {
        public string Dcrn { get { return "Transaction::L1::FullTransaction";  } }
        public TransactionHeader Header { get; set; }
        public string Payload { get; set; }
        public TransactionProof Proof { get; set; }
        public string Version { get; set; }
    }
}