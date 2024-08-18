using System.Data.SQLite;
using System.Diagnostics;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiData.Mappers.StaticData;
using PoeWikiData.Models;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Schema;

namespace PoeWikiData.Endpoints.StaticData
{
    internal class StaticDataDbEndpoint(SQLiteConnection pSQLite, CacheHandler<string, IEnumerable<BaseDbModel>> pCache, ConsoleLogger pLog) : BaseDbEndpoint(pSQLite, pCache, pLog)
    {
        public void Update(PoeWikiManager pApi)
        {
            FullUpdateStaticData("Drop Sources", PoeDbSchemaTypes.DropSources, null, StaticDataMasterRef.DropSources, StaticDataSQLiteMapper.Map);
            FullUpdateStaticData("Drop Types", PoeDbSchemaTypes.DropTypes, null, StaticDataMasterRef.DropTypes, StaticDataSQLiteMapper.Map);
            FullUpdateStaticData("Gem Tags", PoeDbSchemaTypes.GemTags, null, StaticDataMasterRef.GemTags, StaticDataSQLiteMapper.Map);
            FullUpdateStaticData("Item Aspects", PoeDbSchemaTypes.ItemAspects, null, StaticDataMasterRef.ItemAspects, StaticDataSQLiteMapper.Map);
            FullUpdateStaticData("Item Classes", PoeDbSchemaTypes.ItemClasses, null, StaticDataMasterRef.ItemClasses, StaticDataSQLiteMapper.Map);
        }

        protected void FullUpdateStaticData(string pOperationName, PoeDbSchemaTypes pSchemaType, IEnumerable<string>? pSpecifiedColumns, StaticDataDbLookUp pAllStaticData, Func<StaticDataDbModel, SQLiteValues?> pGeSQLValues)
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: UPDATE {pOperationName}");

            Reset(pSchemaType);

            InsertDataFromApiStaticData(pSchemaType, pSpecifiedColumns, pAllStaticData, pGeSQLValues);

            _log.TimeStopLogAndAppend(timer, $"END: UPDATE {pOperationName}");
        }

        protected void InsertDataFromApiStaticData(PoeDbSchemaTypes pSchemaType, IEnumerable<string>? pSpecifiedColumns, StaticDataDbLookUp pAllStaticData, Func<StaticDataDbModel, SQLiteValues?> pGetSQLValues)
        {
            PoeDbSchema schema = PoeDbSchemaManager.GetSchema(pSchemaType);
            foreach (StaticDataDbModel staticData in pAllStaticData.GetAll(false))
            {
                Stopwatch timer = new();
                _log.TimeStartLog(timer, $"BEGIN: UPDATE DATA {staticData.Id}: {staticData.Name}");

                SQLiteValues? sqlData = pGetSQLValues(staticData);
                if (sqlData == null)
                {
                    _log.Log($"There was an issue getting the SQL values for model {staticData}.", LogLevel.ERROR);
                    continue;
                }
                Insert(schema.Table, pSpecifiedColumns, sqlData);

                _log.TimeStartLog(timer, $"END: UPDATE DATA {staticData.Id}: {staticData.Name}");
            }
        }
    }
}
