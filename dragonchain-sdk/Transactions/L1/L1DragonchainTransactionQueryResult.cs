using System.Collections.Generic;

namespace dragonchain_sdk.Transactions.L1
{
    public class L1DragonchainTransactionQueryResult
    {
        public IEnumerable<L1DragonchainTransactionFull> Results { get; set; }
        public ulong Total { get; set; }
    }
}