﻿using Newtonsoft.Json;

namespace dragonchain_sdk.Blocks.Common
{
    public class Proof
    {
        public decimal? Nonce { get; set; }
        [JsonProperty(PropertyName = "proof")]
        public string Value { get; set; }
        public string Scheme { get; set; }
    }
}