﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace dragonchain_sdk.Transactions.Types
{    
    public class TransactionTypeListResponse
    {
        [JsonProperty(PropertyName = "transaction_types")]
        public IEnumerable<TransactionTypeResponse> TransactionTypes { get; set; }
    }
}