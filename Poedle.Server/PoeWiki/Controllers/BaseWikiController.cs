using System.Diagnostics;

using Flurl;

using Poedle.Utils.Cache;
using Poedle.Utils.Http;
using Poedle.Utils.Logger;
using Poedle.Utils.Parser;


namespace Poedle.PoeWiki.Controllers
{
    public abstract class BaseWikiController(HttpRetriever pHttp, CacheHandler<string, string> pCache, DebugLogger pLogger)
    {
        private static readonly string _uriBase = @"https://www.poewiki.net/w/index.php";
        protected static readonly string _cargoTitle = "Special:CargoExport";

        private readonly HttpRetriever _http = pHttp;
        private readonly CacheHandler<string, string> _cache = pCache;
        private readonly DebugLogger _log = pLogger;

        protected T? GetFirstDataModel<T>(string pTitle, string pTables, string pFields, string pWhere)
        {
            return JsonParser.ParseJsonList<T>(GetFromCacheOrApi(BuildQueryString(pTitle, pTables, pFields, pWhere, 1, 0))).FirstOrDefault();
        }

        protected List<T> GetListWithBatching<T>(string pTitle, string pTables, string pFields, string pWhere, int pLimit, int pOffsetStart)
        {
            List<T> returnList = [];
            for (int offset = pOffsetStart, count = pLimit; count >= pLimit; offset += pLimit)
            {
                List<T> batchList = JsonParser.ParseJsonList<T>(GetFromCacheOrApi(BuildQueryString(pTitle, pTables, pFields, pWhere, pLimit, offset)));
                count = batchList.Count;
                returnList.AddRange(batchList);
            }

            return returnList;
        }

        private string GetFromCacheOrApi(string pUri)
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: GET FROM {pUri}.");

            string? result = _cache.Get(pUri);
            if (result == null)
            {
                result = _http.Get(pUri);
                _log.Log("FOUND: api.");
                _cache.Set(pUri, result);
            }
            else
            {
                _log.Log("FOUND: cache.");
            }

            _log.TimeStopLogAndAppend(timer, $"END: GET FROM {pUri}.");
            return result;
        }

        private static string BuildQueryString(string pTitle, string pTables, string pFields, string pWhere, int pLimit, int pOffset)
        {
            return _uriBase.AppendQueryParam("title", pTitle).AppendQueryParam("tables", pTables).AppendQueryParam("fields", pFields).AppendQueryParam("where", pWhere).AppendQueryParam("limit", pLimit).AppendQueryParam("offset", pOffset).AppendQueryParam("format", "json");
        }
    }
}
