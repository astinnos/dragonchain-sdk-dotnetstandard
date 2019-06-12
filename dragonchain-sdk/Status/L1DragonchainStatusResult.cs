namespace dragonchain_sdk.Status
{
    public class L1DragonchainStatusResult
    {
        /// <summary>
        /// Level of this dragonchain (as an integer)
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// Cloud that this chain is running in
        /// </summary>
        public string Cloud { get; set; }
        /// <summary>
        /// URL of the chain
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Region that this chain is operating in
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// Proof scheme that this chain uses
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// Ethereum wallet assigned to this chain
        /// </summary>
        public string Wallet { get; set; }
        /// <summary>
        /// Hashing algorithm used for blocks on this chain
        /// </summary>
        public string HashAlgo { get; set; }
        /// <summary>
        /// Dragonchain version of this chain
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Encryption algorithm used for blocks on this chain
        /// </summary>
        public string EncryptionAlgo { get; set; }
    }
}