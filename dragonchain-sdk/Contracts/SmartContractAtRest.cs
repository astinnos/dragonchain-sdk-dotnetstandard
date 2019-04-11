using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Contracts
{
    public class SmartContractAtRest
    {               
        public string Dcrn { get; set; }
        public string Version { get; set; }

        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }

        public string Id { get; set; }
        public ContractStatus Status { get; set; }
        public string Image  { get; set; }

        [JsonProperty(PropertyName = "auth_key_id")]
        public string  AuthKeyId { get; set; }

        [JsonProperty(PropertyName = "image_digest")]
        public string  ImageDigest { get; set; }

        public string  Cmd { get; set; }

        [JsonProperty(PropertyName = "args")]
        public IEnumerable<string> Arguments { get; set; }
        
        [JsonProperty(PropertyName = "env")]
        public object EnvironmentVariables { get; set; }

        [JsonProperty(PropertyName = "existing_secrets")]
        public IEnumerable<string> ExistingSecrets { get; set; }

        public string Cron { get; set; }
        public string Seconds { get; set; }

        [JsonProperty(PropertyName = "execution_order")]
        public string ExecutionOrder { get; set; }
    }     
}