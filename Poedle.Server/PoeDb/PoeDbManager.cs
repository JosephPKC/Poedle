using LiteDB;
using Poedle.Enums;
using Poedle.PoeDb.DbControllers;
using Poedle.PoeDb.Models;
using Poedle.PoeWiki;
using Poedle.Utils.Logger;

namespace Poedle.PoeDb
{
    public class PoeDbManager
    {
        private readonly LiteDatabase _db;
        private readonly PoeWikiApi _api;
        private readonly DebugLogger _log;

        #region "Controllers"
        public DbScoreController Score { get; private set; }
        public DbLeagueController League { get; private set; } 
        public DbUniqueController Unique { get; private set; }
        #endregion

        private readonly Dictionary<string, List<LeaguesEnum.Leagues>> _versionLeagueCache = [];

        public PoeDbManager(string pDbPath, DebugLogger pLogger)
        {
            _db = new LiteDatabase(pDbPath);
            _api = new PoeWikiApi(pLogger);
            _log = pLogger;

            Score = new(_db, _log);
            League = new(_db, _log);
            Unique = new(_db, _log);
        }

        #region "Reset"
        public void ResetAll()
        {
            // Flush the api cache to clean out old info.
            _api.FlushCache();
            // Collections that do not require other collections go first.
            League.ResetAll(_api);
            CacheLeagues();
            // Collections that require other collections go last.
            Unique.ResetAll(_api, _versionLeagueCache);
        }

        /// <summary>
        /// Reset meta data, that is meant to be persistant.
        /// Includes scores, guesses, and other data.
        /// </summary>
        public void ResetMetaData()
        {
            Score.ResetAll();
        }

        private void CacheLeagues()
        {
            List<DbLeague> allLeagues = League.GetAll();
            foreach (DbLeague league in allLeagues)
            {
                string majorMinor = $"{league.ReleaseVersionMajor}.{league.ReleaseVersionMinor}";

                if (!_versionLeagueCache.TryGetValue(majorMinor, out List<LeaguesEnum.Leagues>? value))
                {
                    value = ([]);
                    _versionLeagueCache.Add(majorMinor, value);
                }

                value.Add(EnumUtil.GetEnumByName<LeaguesEnum.Leagues>(league.Name));
            }
        }
        #endregion

        public void InternalMarkForReview<T>(string pColName, Func<ILiteCollection<T>, List<T>> pLogic) where T : BaseDbModel
        {
            ILiteCollection<T> col = _db.GetCollection<T>(pColName);

            // Set the logic for which ones to review
            List<T> toReview = pLogic(col);

            foreach (var x in toReview)
            {
                x._MarkForReview = true;
                col.Update(x);
            }
        }
    }
}
