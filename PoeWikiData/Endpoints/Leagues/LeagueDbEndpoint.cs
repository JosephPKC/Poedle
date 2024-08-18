using System.Data.SQLite;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiData.Mappers.Leagues;
using PoeWikiData.Models;
using PoeWikiData.Models.Leagues;
using PoeWikiData.Schema;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Endpoints.Leagues
{
    internal class LeagueDbEndpoint(SQLiteConnection pSQLite, CacheHandler<string, IEnumerable<BaseDbModel>> pCache, ConsoleLogger pLog) : BaseDbEndpoint(pSQLite, pCache, pLog)
    {
        public void Update(PoeWikiManager pApi)
        {
            FullUpdate("LEAGUES", PoeDbSchemaTypes.Leagues, null, null, pApi.Leagues.GetAll(), LeagueDbMapper.Map, LeagueSQLiteMapper.Map, null);
        }

        public LeagueDbModel? Select(uint pId)
        {
            string where = $"LeagueId={SQLiteUtils.SQLiteString(pId.ToString())}";
            return SelectOne(PoeDbSchemaTypes.Leagues, LeagueSQLiteMapper.Read, where);
        }

        public LeagueDbLookUp SelectAll()
        {
            return new(SelectAll(PoeDbSchemaTypes.Leagues, LeagueSQLiteMapper.Read));
        }
    }
}
