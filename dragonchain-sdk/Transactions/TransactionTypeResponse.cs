using System.Collections.Generic;

namespace dragonchain_sdk.Transactions
{
    public class TransactionTypeResponse
    {        
        public IEnumerable<CustomIndexStructure> CustomIndexes { get; set; }        
        public bool IsContract { get; set; }       
        public string TxnType { get; set; }        
        public string Version { get; set; }
    }
}