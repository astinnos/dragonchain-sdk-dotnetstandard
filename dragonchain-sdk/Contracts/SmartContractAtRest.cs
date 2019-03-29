namespace dragonchain_sdk.Contracts.SmartContractAtRest
{
    public class SmartContractAtRest
    {
        public string Code { get; set; }        
        public object CustomEnvironmentVariables { get; set; }
        public string Dcrn { get { return "SmartContract::L1::AtRest"; } }
        public string Id { get; set; }        
        public string IsSerial { get; set; }
        public string Name { get; set; }
        public Origin Origin { get; set; }
        public ContractRuntime Runtime { get; set; }
        public string S3_bucket { get; set; }
        public string S3_path { get; set; }
        public string Sc_type { get; set; }
        public Status Status { get; set; }
        public string Version { get; set; }
    }    
}