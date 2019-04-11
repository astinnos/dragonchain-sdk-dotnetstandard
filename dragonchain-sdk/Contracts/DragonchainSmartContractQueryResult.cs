using System.Collections.Generic;

namespace dragonchain_sdk.Contracts
{
    public class DragonchainSmartContractQueryResult
    {
        public IEnumerable<SmartContractAtRest> Results { get; set; }
        public ulong Total { get; set; }
    }
}
