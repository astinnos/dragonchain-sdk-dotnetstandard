using System.Collections.Generic;

namespace dragonchain_sdk.Blocks
{
    public class DragonchainBlockQueryResult
    {
        public IEnumerable<BlockSchemaType> Results { get; set; }
        public ulong Total { get; set; }
    }    
}