using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts.Library.Currency
{
    public class CreateCurrencyContract
    {
        [JsonProperty(PropertyName = "custom_environment_variables")]
        public CustomEnvironmentVariables CustomEnvironmentVariables { get; set; }
        public string Dcrn => "SmartContract::L1::Create"; 
        [JsonProperty(PropertyName = "is_serial")]
        public bool IsSerial => true;
        public string LibraryContractName => "currency";
        public string Name { get; set; }
        public string Origin => "library";
        public ContractRuntime Runtime => ContractRuntime.Nodejs8_10;
        [JsonProperty(PropertyName = "sc_type")]
        public SmartContractType SmartContractType { get { return SmartContractType.Transaction; } }
        public int Version => 2;
    }
}