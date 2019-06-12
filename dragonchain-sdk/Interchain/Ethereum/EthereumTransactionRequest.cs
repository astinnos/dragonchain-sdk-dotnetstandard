using dragonchain_sdk.Interchain.Networks;

namespace dragonchain_sdk.Interchain.Ethereum
{
    public class EthereumTransactionRequest
    {
        public EthereumNetwork Network { get; set; }
        public EthereumTransaction Transaction { get; set; }
    }

    public class EthereumTransaction
    {
        public string To { get; set; }
        public string Value { get; set; }
        public string Data { get; set; }
        public string GasPrice { get; set; }
        public string Gas { get; set; }
    }    
}