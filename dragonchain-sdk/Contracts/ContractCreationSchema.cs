using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Contracts
{
    public class ContractCreationSchema
    {
        [JsonProperty(PropertyName = "args")]
        public IEnumerable<string> Arguments { get; set; }               
        public string Auth { get; set; }
        public string Cmd { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "sc_type")]
        public SmartContractType? SmartContractType { get; set; }
        public string Cron { get; set; }
        public string Dcrn { get { return "SmartContract::L1::Create"; } }     
        [JsonProperty(PropertyName = "desired_state")]
        public SmartContractDesiredState? DesiredState { get; set; }
        [JsonProperty(PropertyName = "env")]
        public object EnvironmentVariables { get; set; }
        [JsonProperty(PropertyName = "execution_order")]
        public SmartContractExecutionOrder? ExecutionOrder { get; set; }
        public string Image { get; set; }
        public int? Seconds { get; set; }        
        public object Secrets { get; set; }
        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }        
        public string Version { get; set; }        
    }
}