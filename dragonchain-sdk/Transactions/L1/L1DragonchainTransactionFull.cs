namespace dragonchain_sdk.Transactions.L1
{
    public class L1DragonchainTransactionFull
    {
        /// <summary>
        /// String representing this Dragonchain Resource Name
        /// </summary>
        public string Dcrn { get; set; }
        /// <summary>
        /// String representing the version of this DataTransferObject
        /// </summary>
        public string Version { get; set; }
        public L1DragonchainTransactionHeader Header { get; set; }
        /// <summary>
        /// String of payload data for this transaction
        /// </summary>
        public object Payload { get; set; }
        public L1DragonchainTransactionProof Proof { get; set; }
        
    }
}