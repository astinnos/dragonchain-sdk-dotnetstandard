﻿using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts.Library.InterchainWatchers.Ethereum
{
    public class CreateEthereumInterchainWatcherContract
    {
        [JsonProperty(PropertyName = "custom_environment_variables")]
        public CustomEnvironmentVariables CustomEnvironmentVariables { get; set; }
        public string Dcrn => "SmartContract::L1::Create";        
        public string LibraryContractName => "interchainWatcher";
        public string Name { get; set; }
        public string Origin => "library";
        public ContractRuntime Runtime => ContractRuntime.Nodejs8_10;
        [JsonProperty(PropertyName = "sc_type")]
        public SmartContractType SmartContractType { get { return SmartContractType.Cron; } }
        public int Version => 2;
    }
}