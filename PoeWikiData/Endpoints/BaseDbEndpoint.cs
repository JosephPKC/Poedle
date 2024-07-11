using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Endpoints
{
    internal abstract class BaseDbEndpoint(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger)
    {
        protected readonly PoeDbHandler _db = pDb;
        protected readonly CacheHandler<string, BaseModel> _cache = pCache;
        protected readonly ConsoleLogger _log = pLogger;
    }
}
