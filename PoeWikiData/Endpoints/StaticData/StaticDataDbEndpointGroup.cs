using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Endpoints.Leagues;
using PoeWikiData.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Endpoints.StaticData
{
    internal class StaticDataDbEndpointGroup(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger)
    {
        public StaticDataDbSelectEndpoint Select { get; private set; } = new(pDb, pCache, pLogger);
        public StaticDataDbUpdateEndpoint Update { get; private set; } = new(pDb, pCache, pLogger);
    }
}
