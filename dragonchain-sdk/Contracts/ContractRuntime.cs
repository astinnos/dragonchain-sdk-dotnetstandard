using Newtonsoft.Json;

namespace dragonchain_sdk.Contracts
{
    public enum ContractRuntime
    {
        [JsonProperty("nodejs6.10")]
        Nodejs6_10,
        [JsonProperty("nodejs8.10")]
        Nodejs8_10,
        [JsonProperty("java8")]
        Java8,
        [JsonProperty("python2.7")]
        Python2_7,
        [JsonProperty("python3.6")]
        Python3_6,
        [JsonProperty("dotnetcore1.0")]
        Dotnetcore1_0,
        [JsonProperty("dotnetcore2.0")]
        Dotnetcore2_0,
        [JsonProperty("dotnetcore2.1")]
        Dotnetcore2_1,
        [JsonProperty("go1.x")]
        Go1_x,
    }    
}