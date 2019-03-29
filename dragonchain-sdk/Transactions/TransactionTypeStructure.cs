using System.Collections.Generic;

namespace dragonchain_sdk.Transactions
{
    public class TransactionTypeStructure
    {        
        public IEnumerable<CustomIndexStructure> CustomIndexes { get; set; }        
        public string TxnType { get; set; }                
        public string Version { get; set; }
    }
}