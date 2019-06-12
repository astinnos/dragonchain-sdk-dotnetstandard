using System.Collections.Generic;

namespace dragonchain_sdk.Framework.Web
{
    /// <summary>
    /// Data returned from a query against a chain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResult<T>
    {
        /// <summary>
        /// IEnumerable of results
        /// </summary>
        public IEnumerable<T> Results { get; set; }

        /// <summary>
        /// Total count of results that match the query
        /// Note this number can be higher than the length of the `results` array,
        /// indicating that pagination was used to shorten the `results` returned
        /// </summary>
        public ulong Total { get; set; }
    }
}