using System.Data.SQLite;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Endpoints
{
    internal class BaseDbSelectEndpoint(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger) : BaseDbEndpoint(pDb, pCache, pLogger)
    {
        protected L SelectAll<L>(string pTableName, Func<SQLiteDataReader, L> pModelCreator) where L : BaseDbModelList
        {
            return _db.SelectMany(pTableName, null, null, null, null, null, false, pModelCreator);
        }

        protected L SelectAll<L>(PoeDbSchema pSchema, Func<SQLiteDataReader, L> pModelCreator) where L : BaseDbModelList
        {
            return SelectAll(pSchema.TableName, pModelCreator);
        }
    }
}
