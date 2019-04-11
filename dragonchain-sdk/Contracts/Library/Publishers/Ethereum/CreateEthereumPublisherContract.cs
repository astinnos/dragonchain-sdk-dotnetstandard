using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace dragonchain_sdk.Contracts.Library.Publishers.Ethereum
{
    public class CreateEthereumPublisherContract
    {
        [JsonProperty(PropertyName = "custom_environment_variables")]
        public CustomEnvironmentVariables CustomEnvironmentVariables { get; set; }
        public string Dcrn => "SmartContract::L1::Create";        
        public string LibraryContractName => "ethereumPublisher";
        public string Name { get; set; }
        public string Origin => "library";
        public ContractRuntime Runtime => ContractRuntime.Nodejs8_10;
        [JsonProperty(PropertyName = "sc_type")]
        public SmartContractType SmartContractType { get { return SmartContractType.Transaction; } }
        public int Version => 2;
    }
}
