using System.Data.SQLite;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Mappers.Leagues;
using PoeWikiData.Models;
using PoeWikiData.Models.Leagues;
using PoeWikiData.Utils;

namespace PoeWikiData.Endpoints.Leagues
{
    internal class LeagueDbSelectEndpoint(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger) : BaseDbSelectEndpoint(pDb, pCache, pLogger)
    {
        public LeagueDbModelList SelectAll()
        {
            static LeagueDbModelList getAllModelsFromDb(SQLiteDataReader pReader) => LeagueDbMapper.Read(pReader);

            PoeDbSchema schema = PoeDbSchemaList.SchemaList[PoeDbSchemaList.PoeDbTypes.Leagues];
            return SelectAll(schema, getAllModelsFromDb);
        }
    }
}
