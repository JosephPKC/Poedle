using System.Diagnostics;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiApi.Models;
using PoeWikiData.Models;
using PoeWikiData.Utils;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Endpoints
{
    internal class BaseDbUpdateEndpoint(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger) : BaseDbEndpoint(pDb, pCache, pLogger)
    {
        protected void FullyUpdateTable<D, W>(string pOperationName, string pTableName, List<string> pColumns, List<string>? pSpecifiedColumns, PoeWikiManager pApi, Func<PoeWikiManager, List<W>> pApiDataRetriever, Func<W, D?> pWikiToDbConverter, Func<D, SQLiteValues?> pDbToSQLConverter, Action<D>? pPostProcesser) where D : BaseDbModel where W : BaseWikiModel
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: UPDATE {pOperationName}");

            ResetTable(pTableName, pColumns);
            AddApiDataToTable(pTableName, pSpecifiedColumns, pApi, pApiDataRetriever, pWikiToDbConverter, pDbToSQLConverter, pPostProcesser);

            _log.TimeStopLogAndAppend(timer, $"END: UPDATE {pOperationName}");
        }

        protected void FullyUpdateTable<D, W>(string pOperationName, PoeDbSchema pSchema, List<string>? pSpecifiedColumns, PoeWikiManager pApi, Func<PoeWikiManager, List<W>> pApiDataRetriever, Func<W, D?> pWikiToDbConverter, Func<D, SQLiteValues?> pDbToSQLConverter, Action<D>? pPostProcesser) where D : BaseDbModel where W : BaseWikiModel
        {
            FullyUpdateTable(pOperationName, pSchema.TableName, pSchema.Columns, pSpecifiedColumns, pApi, pApiDataRetriever, pWikiToDbConverter, pDbToSQLConverter, pPostProcesser);
        }

        protected void ResetTable(string pTableName, List<string> pColumns)
        {
            _db.DropTableIfAble(pTableName);
            _db.CreateTable(pTableName, pColumns);
        }

        protected void ResetTable(PoeDbSchema pSchema)
        {
            ResetTable(pSchema.TableName, pSchema.Columns);
        }

        protected void AddApiDataToTable<D, W>(string pTableName, List<string>? pSpecifiedColumns, PoeWikiManager pApi, Func<PoeWikiManager, List<W>> pApiDataRetriever, Func<W, D?> pWikiToDbConverter, Func<D, SQLiteValues?> pDbToSQLConverter, Action<D>? pPostProcesser) where D : BaseDbModel where W : BaseWikiModel
        {
            List<W> apiDataList = pApiDataRetriever(pApi);
            List<D> dbDataList = [];
            List<SQLiteValues> allSQLValues = [];

            foreach (W apiData in apiDataList)
            {
                D? dbDataModel = pWikiToDbConverter(apiData);
                if (dbDataModel == null)
                {
                    _log.Log($"There was an issue getting the db model version for model {apiData.Name}.", LogLevel.ERROR);
                    continue;
                }
                dbDataList.Add(dbDataModel);

                SQLiteValues? sqlData = pDbToSQLConverter(dbDataModel);
                if (sqlData == null)
                {
                    _log.Log($"There was an issue getting the SQL values for model {apiData.Name}.", LogLevel.ERROR);
                    continue;
                }
                allSQLValues.Add(sqlData);
            }
            _db.InsertAllIntoTable(pTableName, pSpecifiedColumns, allSQLValues);

            foreach (D dbData in dbDataList)
            {
                pPostProcesser?.Invoke(dbData);
            }
        }

        protected void AddApiDataToTable<D, W>(PoeDbSchema pSchema, List<string>? pSpecifiedColumns, PoeWikiManager pApi, Func<PoeWikiManager, List<W>> pApiDataRetriever, Func<W, D?> pWikiToDbConverter, Func<D, SQLiteValues?> pDbToSQLConverter, Action<D>? pPostProcesser) where D : BaseDbModel where W : BaseWikiModel
        {
            AddApiDataToTable(pSchema.TableName, pSpecifiedColumns, pApi, pApiDataRetriever, pWikiToDbConverter, pDbToSQLConverter, pPostProcesser);
        }
    }
}
