using LiteDB;

using Poedle.PoeDb.Mappers;
using Poedle.PoeDb.Models;
using Poedle.PoeWiki;
using Poedle.PoeWiki.Models;
using Poedle.Utils.Logger;

using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.PoeDb.DbControllers
{
    public class DbLeagueController(LiteDatabase pDb, DebugLogger pLogger) : BaseDbController(pDb, pLogger)
    {
        #region "GET"
        public List<DbLeague> GetAll(ushort? pLimit = null)
        {
            return GetAllOrdered<DbLeague>(DbColTypes.LEAGUES, pLimit);
        }

        public DbLeague? GetById(int pId)
        {
            return GetById<DbLeague>(DbColTypes.LEAGUES, pId);
        }

        public List<DbLeague> GetByReleaseVersion(string pReleaseVersion)
        {
            string exp = $"UPPER($.ReleaseVersion)=UPPER(\"{pReleaseVersion}\")";
            return GetAllByExpGeneric<DbLeague>(DbColTypes.LEAGUES, exp);
        }

        public List<DbLeague> GetByMajorMinorVersion(string pVersion)
        {
            DbVersionUtil version = new(pVersion);
            string exp = $"UPPER($.ReleaseVersionMajor)=UPPER(\"{version.Major}\") AND UPPER($.ReleaseVersionMinor)=UPPER(\"{version.Minor}\")";
            return GetAllByExpGeneric<DbLeague>(DbColTypes.LEAGUES, exp);
        }
        #endregion

        #region "RESET"
        public void ResetAll(PoeWikiApi pApi)
        {
            List<PoeWikiLeague> GetAllFromApi() => pApi.League.GetAll();
            DbLeague GetDbModel(PoeWikiLeague x) => LeagueMapper.MapLeague(x);
            void PostProcessAll(ILiteCollection<DbLeague> x) => x.EnsureIndex("ReleaseVersion");

            ResetAll(DbColTypes.LEAGUES, GetAllFromApi, GetDbModel, null, PostProcessAll);
        }
        #endregion
    }
}
