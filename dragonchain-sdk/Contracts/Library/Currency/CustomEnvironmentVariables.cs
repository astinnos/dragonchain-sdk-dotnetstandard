using System;
using System.Collections.Generic;
using System.Text;

namespace dragonchain_sdk.Contracts.Library.Currency
{
    public class CustomEnvironmentVariables
    {
        public AddressScheme AddressScheme { get; set; }
        public Governance Governance { get; set; }
        public string OriginWalletAddress { get; set; }
        public int Precision { get; set; }
        public ulong TotalAmount { get; set; }        
    }
}
