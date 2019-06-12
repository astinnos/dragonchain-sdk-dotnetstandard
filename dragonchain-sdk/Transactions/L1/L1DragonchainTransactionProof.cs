namespace dragonchain_sdk.Transactions.L1
{
    public class L1DragonchainTransactionProof
    {
        /// <summary>
        /// Hash of the full transaction
        /// </summary>
        public string Full { get; set; }
        /// <summary>
        /// Signature of the stripped transaction
        /// </summary>
        public string Stripped { get; set; }
    }
}