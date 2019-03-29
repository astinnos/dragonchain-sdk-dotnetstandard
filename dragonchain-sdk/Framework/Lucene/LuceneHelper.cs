using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dragonchain_sdk.Framework.Lucene
{
    public static class LuceneHelper
    {
        public static string GetLuceneParams(string query, string sort, int offset = 0, int limit = 10)
        {
            var @params = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(query)) { @params.Add("q", query); }
            if (!string.IsNullOrWhiteSpace(sort)) { @params.Add("sort", sort); }
            @params.Add("offset", offset.ToString());
            @params.Add("limit", limit.ToString());
            return GenerateQueryString(@params);
        }        

        private static string GenerateQueryString(Dictionary<string, string> queryObject)
        {
            const string query = "?";
            var array = queryObject.Select(i => $"{HttpUtility.UrlEncode(i.Key)}={HttpUtility.UrlEncode(i.Value)}").ToArray();            
            return $"{query}{string.Join("&", array)}";
        }
    }
}