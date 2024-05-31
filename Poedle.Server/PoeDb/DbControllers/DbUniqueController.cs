using LiteDB;
using Poedle.Enums;
using Poedle.PoeDb.Mappers;
using Poedle.PoeDb.Models;
using Poedle.PoeWiki;
using Poedle.PoeWiki.Models;
using Poedle.Utils.Logger;

using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.PoeDb.DbControllers
{
    public class DbUniqueController(LiteDatabase pDb, DebugLogger pLogger) : BaseDbController(pDb, pLogger)
    {
        #region "GET"
        public List<DbUnique> GetAll(ushort? pLimit = null)
        {
            return GetAllOrdered<DbUnique>(DbColTypes.UNIQUES, pLimit);
        }

        public DbUnique? GetById(int pId)
        {
            return GetById<DbUnique>(DbColTypes.UNIQUES, pId);
        }

        public DbUnique? GetByPageName(string pPageName)
        {
            return GetByPageName<DbUnique>(DbColTypes.UNIQUES, pPageName);
        }

        public DbUnique? GetRandom()
        {
            return GetRandom<DbUnique>(DbColTypes.UNIQUES);
        }
        #endregion

        #region "RESET"
        public void ResetAll(PoeWikiApi pApi, Dictionary<string, List<LeaguesEnum.Leagues>> pLeagueMapCache)
        {
            List<PoeWikiUnique> GetAllFromApi() => pApi.Unique.GetAll();
            DbUnique GetDbModel(PoeWikiUnique x)
            {
                DbVersionUtil version = new(x.ReleaseVersion);
                List<LeaguesEnum.Leagues> leagues = [];
                if (pLeagueMapCache.TryGetValue(version.MajorMinorText, out List<LeaguesEnum.Leagues>? value))
                {
                    leagues = value;
                }
                return UniqueMapper.MapUnique(x, leagues);
            };
            void PostProcessAll(ILiteCollection<DbUnique> x) => x.EnsureIndex("PageName");

            ResetAll(DbColTypes.UNIQUES, GetAllFromApi, GetDbModel, null, PostProcessAll);
        }
        #endregion
    }
}
