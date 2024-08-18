using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi.Models;
using PoeWikiData.Models;
using PoeWikiData.Schema;
using PoeWikiData.Utils.SQL;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Endpoints
{
    internal abstract class BaseDbEndpoint(SQLiteConnection pSQLite, CacheHandler<string, IEnumerable<BaseDbModel>> pCache, ConsoleLogger pLog)
    {
        private readonly SQLiteConnection _sqlite = pSQLite;
        private readonly CacheHandler<string, IEnumerable<BaseDbModel>> _cache = pCache;
        protected readonly ConsoleLogger _log = pLog;

        #region "Update"
        protected void FullUpdate<TDbModel, TWikiModel>(string pOperationName, PoeDbSchemaTypes pSchemaType, IEnumerable<PoeDbSchemaTypes>? pLinkSchemaTypes, IEnumerable<string>? pSpecifiedColumns, IEnumerable<TWikiModel> pAllApiData,
            Func<TWikiModel, TDbModel?> pGetDbModel, Func<TDbModel, SQLiteValues?> pGeSQLValues, Action<TDbModel>? pUpdateLinks)
            where TDbModel : BaseDbModel where TWikiModel : BaseWikiModel
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: UPDATE {pOperationName}");

            Reset(pSchemaType);
            if (pLinkSchemaTypes != null)
            {
                foreach (PoeDbSchemaTypes pLinkSchemaType in pLinkSchemaTypes)
                {
                    Reset(pLinkSchemaType);
                }
            }

            InsertDataFromApi(pSchemaType, pSpecifiedColumns, pAllApiData, pGetDbModel, pGeSQLValues, pUpdateLinks);

            _log.TimeStopLogAndAppend(timer, $"END: UPDATE {pOperationName}");
        }

        protected void UpdateTableLinks<TDbModel, TDbLink>(string pOperationName, PoeDbSchemaTypes pSchemaType, TDbModel pModel, IEnumerable<TDbLink> pLinks, Func<TDbModel, TDbLink, SQLiteValues> pGetSQLValues) where TDbModel : BaseDbModel
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: UPDATE LINKS {pOperationName}");

            if (!pLinks.Any())
            {
                _log.Log("No linking data found.");
                _log.TimeStopLogAndAppend(timer, $"END: UPDATE LINKS {pOperationName}");
                return;
            }

            PoeDbSchema schema = PoeDbSchemaManager.GetSchema(pSchemaType);
            foreach (TDbLink link in pLinks)
            {
                SQLiteValues sqlData = pGetSQLValues(pModel, link);
                Insert(schema.Table, null, sqlData);
            }

            _log.TimeStopLogAndAppend(timer, $"END: UPDATE LINKS {pOperationName}");
        }

        protected void UpdateTableLinksWithOrder<TDbModel, TDbLink>(string pOperationName, PoeDbSchemaTypes pSchemaType, TDbModel pModel, IEnumerable<TDbLink> pDataLinks, Func<TDbModel, TDbLink, uint, SQLiteValues> pDbToSQLConverter) where TDbModel : BaseDbModel
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: UPDATE LINKS {pOperationName}");

            if (!pDataLinks.Any())
            {
                _log.Log("No linking data found.");
                _log.TimeStopLogAndAppend(timer, $"END: UPDATE LINKS {pOperationName}");
                return;
            }

            PoeDbSchema schema = PoeDbSchemaManager.GetSchema(pSchemaType);
            uint order = 0;
            foreach (TDbLink link in pDataLinks)
            {
                SQLiteValues sqlData = pDbToSQLConverter(pModel, link, order++);
                Insert(schema.Table, null, sqlData);
            }

            _log.TimeStopLogAndAppend(timer, $"END: UPDATE LINKS {pOperationName}");
        }
        #endregion

        #region "Reset"
        protected void Reset(PoeDbSchemaTypes pSchemaType)
        {
            PoeDbSchema schema = PoeDbSchemaManager.GetSchema(pSchemaType);
            try
            {
                Drop(schema.Table);
            }
            catch (Exception)
            {
                Console.WriteLine($"Table {schema.Table} not found.");
            }
            Create(schema.Table, schema.Columns);
        }
        #endregion

        #region "Insert"
        protected void InsertDataFromApi<TDbModel, TWikiModel>(PoeDbSchemaTypes pSchemaType, IEnumerable<string>? pSpecifiedColumns, IEnumerable<TWikiModel> pAllApiData, 
            Func<TWikiModel, TDbModel?> pGetDbModel, Func<TDbModel, SQLiteValues?> pGetSQLValues, Action<TDbModel>? pUpdateLinks) 
            where TDbModel : BaseDbModel where TWikiModel : BaseWikiModel
        {
            PoeDbSchema schema = PoeDbSchemaManager.GetSchema(pSchemaType);
            foreach (TWikiModel apiData in pAllApiData)
            {
                Stopwatch timer = new();
                _log.TimeStartLog(timer, $"BEGIN: UPDATE DATA {apiData.Id}: {apiData.Name}");

                TDbModel? dbDataModel = pGetDbModel(apiData);
                if (dbDataModel == null)
                {
                    _log.Log($"There was an issue getting the db model version for model {apiData.Name}.", LogLevel.ERROR);
                    continue;
                }

                Models.SQLiteValues? sqlData = pGetSQLValues(dbDataModel);
                if (sqlData == null)
                {
                    _log.Log($"There was an issue getting the SQL values for model {apiData.Name}.", LogLevel.ERROR);
                    continue;
                }
                Insert(schema.Table, pSpecifiedColumns, sqlData);
                pUpdateLinks?.Invoke(dbDataModel);

                _log.TimeStartLog(timer, $"END: UPDATE DATA {apiData.Id}: {apiData.Name}");
            }
        }
        #endregion

        #region "Select"
        protected TDbModel? SelectOne<TDbModel>(PoeDbSchemaTypes pSchemaType, Func<IDataReader, IEnumerable<TDbModel>> pCreateModel, string? pWhere = null) where TDbModel : BaseDbModel
        {
            PoeDbSchema schema = PoeDbSchemaManager.GetSchema(pSchemaType);
            return Select(schema.Table, null, pWhere, null, null, null, null, false, pCreateModel).FirstOrDefault();
        }

        protected IEnumerable<TDbModel> SelectAll<TDbModel>(PoeDbSchemaTypes pSchemaType, Func<IDataReader, IEnumerable<TDbModel>> pCreateModel, string? pWhere = null, string? pOrderBy = null, bool? pIsAsc = null) where TDbModel : BaseDbModel
        {
            PoeDbSchema schema = PoeDbSchemaManager.GetSchema(pSchemaType);
            return Select(schema.Table, null, pWhere, null, pOrderBy, pIsAsc, null, false, pCreateModel);
        }

        protected IEnumerable<TData> SelectLinks<TLinkModel, TData>(PoeDbSchemaTypes pSchemaType, Func<IDataReader, IEnumerable<TLinkModel>> pCreateModel, Func<IEnumerable<TLinkModel>, IEnumerable<TData>> pCreateModelFromLinks, string pIdCol, uint pId, string? pOrderCol = null, bool? pIsAsc = null) where TLinkModel : BaseDbModel
        {
            string where = $"{pIdCol}={SQLiteUtils.SQLiteString(pId.ToString())}";
            IEnumerable<TLinkModel> links = SelectAll(pSchemaType, pCreateModel, where, pOrderCol, pIsAsc);
            return pCreateModelFromLinks(links);
        }

        #endregion

        #region "Basic Generic Queries"
        private void Create(string pTableName, IEnumerable<string> pColumns)
        {
            if (string.IsNullOrWhiteSpace(pTableName))
            {
                throw new ArgumentNullException(nameof(pColumns));
            }

            if (string.IsNullOrWhiteSpace(pTableName))
            {
                throw new ArgumentNullException(nameof(pColumns));
            }

            string query = $"CREATE TABLE {pTableName}({string.Join(",", pColumns)});";
            ExecuteNonQuery(query);
        }

        private void Drop(string pTableName)
        {
            if (string.IsNullOrWhiteSpace(pTableName))
            {
                throw new ArgumentNullException(nameof(pTableName));
            }

            string query = $"DROP TABLE {pTableName};";
            ExecuteNonQuery(query);
        }

        protected void Insert(string pTableName, IEnumerable<string>? pSpecifiedColumns, SQLiteValues pValues)
        {
            ArgumentNullException.ThrowIfNull(pValues);

            string columns = string.Empty;
            if (pSpecifiedColumns != null && pSpecifiedColumns.Any())
            {
                columns = $" ({string.Join(",", pSpecifiedColumns)})";
            }

            string query = $"INSERT INTO {pTableName}{columns} VALUES ({pValues});";
            ExecuteNonQuery(query);
        }

        private void InsertAll(string pTableName, IEnumerable<string>? pSpecifiedColumns, IEnumerable<SQLiteValues> pAllValues)
        {
            if (pAllValues == null || !pAllValues.Any())
            {
                throw new ArgumentNullException(nameof(pAllValues));
            }

            string columns = string.Empty;
            if (pSpecifiedColumns != null && pSpecifiedColumns.Any())
            {
                columns = $" ({string.Join(",", pSpecifiedColumns)})";
            }

            ICollection<string> valueRows = [];
            foreach (SQLiteValues valueRow in pAllValues)
            {
                valueRows.Add($"({valueRow})");
            }

            string query = $"INSERT INTO {pTableName}{columns} VALUES {string.Join(", ", valueRows)};";
            ExecuteNonQuery(query);
        }

        private IEnumerable<TDbModel> Select<TDbModel>(string pTableName, string? pFields, string? pWhere, string? pGroupBy, string? pOrderBy, bool? pIsAsc, uint? pTop, bool? pIsDistinct, Func<IDataReader, IEnumerable<TDbModel>> pCreateModels) where TDbModel : BaseDbModel
        {
            if (string.IsNullOrWhiteSpace(pTableName))
            {
                throw new ArgumentNullException(nameof(pTableName));
            }

            SQLiteQueryBuilder queryBuilder = new();
            // SELECT FIELDS FROM TABLE
            queryBuilder.Select(pFields).Distinct(pIsDistinct).Top(pTop).From(pTableName);

            // Conditions: WHERE X GROUP BY Y ORDER BY Z
            queryBuilder.Where(pWhere).GroupBy(pGroupBy).OrderBy(pOrderBy).Asc(pIsAsc);

            string query = queryBuilder.ToString();
            return ExecuteQuery(query, pCreateModels);
        }
        #endregion

        #region "Low-Level Helpers"
        private void ExecuteNonQuery(string pQuery)
        {
            if (string.IsNullOrWhiteSpace(pQuery))
            {
                throw new ArgumentNullException(nameof(pQuery));
            }

            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: {pQuery}");

            using (SQLiteCommand command = new(pQuery, _sqlite))
            {
                try
                {
                    _sqlite.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    _sqlite.Close();
                }
            }

            _log.TimeStopLogAndAppend(timer, $"END: {pQuery}");
        }

        private IEnumerable<TDbModel> ExecuteQuery<TDbModel>(string pQuery, Func<IDataReader, IEnumerable<TDbModel>> pCreateModels) where TDbModel : BaseDbModel
        {
            if (string.IsNullOrWhiteSpace(pQuery))
            {
                throw new ArgumentNullException(nameof(pQuery));
            }

            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: {pQuery}", LogLevel.VERBOSE);

            if (_cache.Get(pQuery) is IEnumerable<TDbModel> modelFromCache)
            {
                _log.Log("Found model from cache.");
                _log.TimeStopLogAndAppend(timer, $"END: {pQuery}");
                return modelFromCache;
            }

            IEnumerable<TDbModel> model;
            using (SQLiteCommand command = new(pQuery, _sqlite))
            {
                _sqlite.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    model = pCreateModels(reader);
                }
                _sqlite.Close();
            }

            _cache.Set(pQuery, model as IEnumerable<BaseDbModel> ?? []);
            _log.TimeStopLogAndAppend(timer, $"END: {pQuery}", LogLevel.VERBOSE);

            return model;
        }
        #endregion
    }
}
