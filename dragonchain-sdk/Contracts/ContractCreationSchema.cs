using System.Collections.Generic;

namespace dragonchain_sdk.Contracts
{
    public class ContractCreationSchema
    {
        public string Version { get { return "3"; } }
        public string Dcrn { get { return "SmartContract::L1::Create"; } }
        public string TxnType { get; set; }
        public string Image { get; set; }
        public string Cmd { get; set; }
        public SmartContractExecutionOrder ExecutionOrder { get; set; }
        public SmartContractDesiredState DesiredState { get; set; }
        public IEnumerable<string> Args { get; set; }
        public object Env { get; set; }
        public object Secrets { get; set; }
        public int Seconds { get; set; }
        public string Cron { get; set; }
        public string Auth { get; set; }
    }
}