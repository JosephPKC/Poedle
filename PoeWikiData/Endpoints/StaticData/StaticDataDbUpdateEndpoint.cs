using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Endpoints.StaticData
{
    internal class StaticDataDbUpdateEndpoint(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger) : BaseDbUpdateEndpoint(pDb, pCache, pLogger)
    {

    }
}
