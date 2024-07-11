using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiApi.Models;
using PoeWikiData.Mappers.Leagues;
using PoeWikiData.Models;
using PoeWikiData.Models.Leagues;
using PoeWikiData.Utils;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Endpoints.Leagues
{
    internal class LeagueDbUpdateEndpoint(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger) : BaseDbUpdateEndpoint(pDb, pCache, pLogger)
    {
        public void Update(PoeWikiManager pApi)
        {
            static List<LeagueWikiModel> getWikiDataFromApi(PoeWikiManager pApi) => pApi.Leagues.GetAll();
            LeagueDbModel? getDbModelFromWikiData(LeagueWikiModel pModel) => LeagueDbMapper.Map(pModel);
            SQLiteValues? getSQLValuesFromDbModel(LeagueDbModel pModel) => LeagueSQLiteMapper.Map(pModel);

            PoeDbSchema schema = PoeDbSchemaList.SchemaList[PoeDbSchemaList.PoeDbTypes.Leagues];
            FullyUpdateTable("LEAGUES", schema, null, pApi, getWikiDataFromApi, getDbModelFromWikiData, getSQLValuesFromDbModel, null);
        }
    }
}
