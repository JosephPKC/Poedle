﻿using LiteDB;
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
        private readonly DbResetter _dbReset;
        public DbLeagueGetter League { get; private set; } 
        public DbUniqueGetter Unique { get; private set; }
        #endregion

        public PoeDbManager(string pDbPath, DebugLogger pLogger)
        {
            _db = new LiteDatabase(pDbPath);
            _api = new PoeWikiApi(pLogger);
            _log = pLogger;
            _dbReset = new DbResetter(_db, _api, _log);

            League = new(_db, _log);
            Unique = new(_db, _log);
        }

        #region "Reset"
        public void ResetAll()
        {
            // Flush the api cache to clean out old info.
            _api.FlushCache();
            // Collections that do not require other collections go first.
            _dbReset.ResetAllLeagues();
            // Collections that require other collections go last.
            _dbReset.ResetAllUniques(League);
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
