using Flurl;

namespace PoeWikiApi.Utils
{
    internal static class UrlBuilder
    {
        public static string BuildUrl(string pBase, Dictionary<string, string> pQueryParams)
        {
            string url = pBase;
            foreach (KeyValuePair<string, string> query in pQueryParams)
            {
                url = url.AppendQueryParam(query.Key, query.Value);
            }

            return url;
        }
    }
}
