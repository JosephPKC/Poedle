using System.Diagnostics;

using LiteDB;

using Poedle.PoeDb.Mappers;
using Poedle.PoeDb.Models;
using Poedle.PoeWiki;
using Poedle.PoeWiki.Models;
using Poedle.Utils.Logger;

using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.PoeDb.DbControllers
{
    public class DbResetter(LiteDatabase pDb, PoeWikiApi pApi, DebugLogger pLogger)
    {
        private readonly LiteDatabase _db = pDb;
        private readonly PoeWikiApi _api = pApi;
        private readonly DebugLogger _log = pLogger;

        #region "Reset"
        protected void ResetAll<D, W>(DbColTypes pColType, Func<List<W>> pGetAllFromApi, Func<W, D> pGetDbModel, Action<D, W>? pPostProcess = null, Action<ILiteCollection<D>>? pPostProcessAll = null) where D : BaseDbPoeModel where W : BasePoeWikiModel
        {
            Stopwatch timer = new();
            string colName = DbParamsMap[pColType].ColName;
            _log.TimeStartLog(timer, $"BEGIN: RESET {colName.ToUpper()}.");

            ILiteCollection<D> col = _db.GetCollection<D>(colName);
            int docsDeleted = col.DeleteAll();
            _log.Log($"DELETED {docsDeleted} docs.");

            List<W> allFromApi = pGetAllFromApi();
            foreach (W apiData in allFromApi)
            {
                D dbData = pGetDbModel(apiData);
                if (dbData == null)
                {
                    _log.Log($"DB: DATA IS NULL: {apiData.Name}.");
                    continue;
                }

                pPostProcess?.Invoke(dbData, apiData);

                _log.Log($"DB INSERT: {dbData.Name} / {dbData.PageName}.");
                col.Insert(dbData);
            }
            pPostProcessAll?.Invoke(col);

            _log.TimeStopLogAndAppend(timer, $"END: RESET {colName.ToUpper()}.");
        }

        #region "Leagues"
        public void ResetAllLeagues()
        {
            List<PoeWikiLeague> GetAllFromApi() => _api.League.GetAll();
            DbLeague GetDbModel(PoeWikiLeague x) => LeagueMapper.MapLeague(x);
            void PostProcessAll(ILiteCollection<DbLeague> x) => x.EnsureIndex("ReleaseVersion");

            ResetAll(DbColTypes.LEAGUES, GetAllFromApi, GetDbModel, null, PostProcessAll);
        }

        #endregion

        #region "Uniques"
        public void ResetAllUniques(DbLeagueGetter pLeagueGetter)
        {
            List<PoeWikiUnique> GetAllFromApi() => _api.Unique.GetAll();
            DbUnique GetDbModel(PoeWikiUnique x)
            {
                List<DbLeague> leagues = pLeagueGetter.GetByMajorMinorVersion(x.ReleaseVersion);
                return UniqueMapper.MapUnique(x, leagues);
            };
            void PostProcessAll(ILiteCollection<DbUnique> x) => x.EnsureIndex("PageName");

            ResetAll(DbColTypes.UNIQUES, GetAllFromApi, GetDbModel, null, PostProcessAll);
        }
        #endregion

        #region "Meta"
        public void ResetAllUniqueAnswers(DbLeagueGetter pLeagueGetter)
        {
            List<PoeWikiUnique> GetAllFromApi() => _api.Unique.GetAll();
            DbUnique GetDbModel(PoeWikiUnique x)
            {
                List<DbLeague> leagues = pLeagueGetter.GetByMajorMinorVersion(x.ReleaseVersion);
                return UniqueMapper.MapUnique(x, leagues);
            };
            void PostProcessAll(ILiteCollection<DbUnique> x) => x.EnsureIndex("PageName");

            ResetAll(DbColTypes.UNIQUES, GetAllFromApi, GetDbModel, null, PostProcessAll);
        }
        #endregion
        #endregion
    }
}
