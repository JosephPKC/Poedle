using Poedle.PoeWiki.Controllers;
using Poedle.Utils.Cache;
using Poedle.Utils.Http;
using Poedle.Utils.Logger;

namespace Poedle.PoeWiki
{
    public class PoeWikiApi
    {
        private static readonly long _cacheSizeLimit = 1024;

        private readonly HttpRetriever _http;
        private readonly CacheHandler<string, string> _cache;
        private readonly DebugLogger _log;

        #region "Controllers"
        public WikiLeagueGetter League { get; private set; }
        public WikiUniqueGetter Unique { get; private set; }
        public WikiSkillGemGetter SkillGem { get; private set; }
        public WikiPassiveGetter Passive { get; private set; }
        #endregion

        public PoeWikiApi(DebugLogger pLogger)
        {
            _http = new();
            _cache = new(_cacheSizeLimit);
            _log = pLogger;

            League = new(_http, _cache, _log);
            Unique = new(_http, _cache, _log);
            SkillGem = new(_http, _cache, _log);
            Passive = new(_http, _cache, _log);
        }

        public void FlushCache()
        {
            _cache.Flush(_cacheSizeLimit);
        }
    }
}
 