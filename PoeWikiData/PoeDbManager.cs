using System.Data.SQLite;
using System.Security.Cryptography;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiData.Endpoints.Leagues;
using PoeWikiData.Endpoints.UniqueItems;
using PoeWikiData.Models;
using PoeWikiData.Models.Leagues;
using PoeWikiData.Models.UniqueItems;

namespace PoeWikiData
{
    public class PoeDbManager
    {
        private static readonly long _cacheSizeLimit = 4096;

        private readonly SQLiteConnection _sqlite;
        private readonly CacheHandler<string, IEnumerable<BaseDbModel>> _cache;
        private readonly ConsoleLogger _log;
        private readonly PoeWikiManager _api ;

        private readonly LeagueDbEndpoint _league;
        private readonly UniqueItemDbEndpoint _uniqueItem;

        public PoeDbManager(string pDbFilePath, bool pIsNewDb, ConsoleLogger pLog)
        {
            _sqlite = new($"Data Source={pDbFilePath};New={pIsNewDb};");
            _log = pLog;
            _cache = new(_cacheSizeLimit);
            _api = new(pLog);
            

            _league = new(_sqlite, _cache, pLog);
            _uniqueItem = new(_sqlite, _cache, pLog);
        }

        public void ResetData()
        {
            _league.Update(_api);
            _uniqueItem.Update(_api, _league.SelectAll());
        }

        public void ClearMetaData()
        {

        }

        public TModel? GetById<TModel>(uint pId) where TModel : BaseDbModel
        {
            return typeof(TModel) switch
            {
                Type model when model == typeof(LeagueDbModel) => _league.Select(pId) as TModel,
                Type model when model == typeof(UniqueItemDbModel) => _uniqueItem.Select(pId) as TModel,
                _ => null
            };
        }

        public IEnumerable<TModel> GetAll<TModel>() where TModel : BaseDbModel
        {
            return typeof(TModel) switch
            {
                Type model when model == typeof(LeagueDbModel) => _league.SelectAll().GetAll() as IEnumerable<TModel> ?? [],
                Type model when model == typeof(UniqueItemDbModel) => _uniqueItem.SelectAll().GetAll() as IEnumerable<TModel> ?? [],
                _ => []
            };
        }
    }
}
