using System.Collections.Generic;
using Newtonsoft.Json;

namespace dragonchain_sdk.Transactions.Bulk
{
    public class DragonchainBulkTransactionCreateResponse
    {
        /// <summary>
        /// Successfully posted transactions
        /// </summary>
        [JsonProperty(PropertyName = "201")]
        public IEnumerable<DragonchainTransactionCreateResponse> Created { get; set; }
        /// <summary>
        /// Transactions that failed to post
        /// </summary>
        [JsonProperty(PropertyName = "400")]
        public IEnumerable<FailedBulkTransactionCreate> Failed { get; set; }
    }
}
