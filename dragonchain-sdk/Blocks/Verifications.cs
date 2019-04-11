using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Blocks
{
    public interface IVerifications { }

    public class Verifications : IVerifications
    {
        [JsonProperty(PropertyName = "2")]
        public IEnumerable<L2.L2BlockAtRest> L2 { get; set; }
        [JsonProperty(PropertyName = "3")]
        public IEnumerable<L3.L3BlockAtRest> L3 { get; set; }
        [JsonProperty(PropertyName = "4")]
        public IEnumerable<L4.L4BlockAtRest> L4 { get; set; }
        [JsonProperty(PropertyName = "5")]
        public IEnumerable<L5.L5BlockAtRest> L5 { get; set; }
    }

    public class LevelVerifications : IVerifications
    {
        public IEnumerable<BlockSchemaType> Verifications { get; set; }
    }
}
