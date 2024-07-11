using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Endpoints.UniqueItems
{
    internal class UniqueItemDbEndpointGroup(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger) : BaseDbEndpoint(pDb, pCache, pLogger)
    {
        public UniqueItemDbSelectEndpoint Select { get; private set; } = new(pDb, pCache, pLogger);
        public UniqueItemDbUpdateEndpoint Update { get; private set; } = new(pDb, pCache, pLogger);
    }
}
