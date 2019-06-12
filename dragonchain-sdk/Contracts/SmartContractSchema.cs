using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Contracts
{
    public class SmartContractSchema
    {
        public string Version { get; set; }
        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }
        public string Image { get; set; }
        [JsonProperty(PropertyName = "execution_order")]
        public SmartContractExecutionOrder? ExecutionOrder { get; set; }
        public string Cmd { get; set; }
        [JsonProperty(PropertyName = "args")]
        public IEnumerable<string> Arguments { get; set; }
        [JsonProperty(PropertyName = "env")]
        public object EnvironmentVariables { get; set; }
        public object Secrets { get; set; }
        public int? Seconds { get; set; }
        public string Cron { get; set; }
        public string Auth { get; set; }        
        [JsonProperty(PropertyName = "desired_state")]
        public SmartContractDesiredState? DesiredState { get; set; }
    }
}