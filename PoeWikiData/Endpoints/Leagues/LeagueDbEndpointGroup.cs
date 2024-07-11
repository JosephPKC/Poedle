using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Endpoints.Leagues
{
    internal class LeagueDbEndpointGroup(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger)
    {
        public LeagueDbSelectEndpoint Select { get; private set; } = new(pDb, pCache, pLogger);
        public LeagueDbUpdateEndpoint Update { get; private set; } = new(pDb, pCache, pLogger);

    }
}
