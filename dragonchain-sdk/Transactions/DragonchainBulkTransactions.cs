using System.Collections.Generic;

namespace dragonchain_sdk.Transactions
{
    public class DragonchainBulkTransactions
    {
        public IEnumerable<DragonchainTransactionCreatePayload> Payload { get; set; }
    }
}