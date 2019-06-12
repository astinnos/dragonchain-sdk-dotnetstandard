using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Contracts
{
    public class SmartContractAtRest
    {
        /// <summary>
        /// String representing this Dragonchain Resource Name
        /// </summary>
        public string Dcrn { get; set; }
        /// <summary>
        /// String representing the version of this DataTransferObject
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// The name (and also transaction type to invoke) this smart contract
        /// </summary>
        [JsonProperty(PropertyName = "txn_type")]
        public string TransactionType { get; set; }
        /// <summary>
        /// The unique guid identifier for this contract
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Data about the current status of the smart contract
        /// </summary>
        public ContractStatus Status { get; set; }
        /// <summary>
        /// Docker image of the smart contract
        /// </summary>
        public string Image  { get; set; }
        /// <summary>
        /// Id of the auth key that is used by the smart contract for communication back with the chain
        /// </summary>
        [JsonProperty(PropertyName = "auth_key_id")]
        public string  AuthKeyId { get; set; }
        /// <summary>
        /// Docker image pull digest of the deployed smart contract
        /// </summary>
        [JsonProperty(PropertyName = "image_digest")]
        public string  ImageDigest { get; set; }
        /// <summary>
        /// Command that is run on execution of the smart contract
        /// </summary>
        public string  Cmd { get; set; }
        /// <summary>
        /// Args passed into the command on execution of the smart contract
        /// </summary>
        [JsonProperty(PropertyName = "args")]
        public IEnumerable<string> Arguments { get; set; }
        /// <summary>
        /// Environment variables given to the smart contract
        /// </summary>
        [JsonProperty(PropertyName = "env")]
        public object EnvironmentVariables { get; set; }
        /// <summary>
        /// Array of secret names for this smart contract
        /// </summary>
        [JsonProperty(PropertyName = "existing_secrets")]
        public IEnumerable<string> ExistingSecrets { get; set; }
        /// <summary>
        /// Cron expression for scheduling automatic execution of the smart contract
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// Number of seconds between automatic executions of the smart contract
        /// </summary>
        public int Seconds { get; set; }
        /// <summary>
        /// Execution order of the contract, whether it gets invoked asap (parallel), or in a single queue (serial)
        /// </summary>
        [JsonProperty(PropertyName = "execution_order")]
        public SmartContractExecutionOrder ExecutionOrder { get; set; }
    }     
}