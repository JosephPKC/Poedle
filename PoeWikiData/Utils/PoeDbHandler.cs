using System.Data.SQLite;
using System.Diagnostics;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Models;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Utils
{
    internal class PoeDbHandler(string pDbFilePath, bool pIsNewDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger)
    {
        private readonly SQLiteConnection _sqlite = new($"Data Source={pDbFilePath};New={pIsNewDb};");
        protected readonly CacheHandler<string, BaseModel> _cache = pCache;
        private readonly ConsoleLogger _log = pLogger;

        #region "CREATE TABLE"
        public void CreateTable(string pTableName, List<string> pColumns)
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
        #endregion

        #region "DROP TABLE"
        public void DropTable(string pTableName)
        {
            if (string.IsNullOrWhiteSpace(pTableName))
            {
                throw new ArgumentNullException(nameof(pTableName));
            }

            string query = $"DROP TABLE {pTableName};";
            ExecuteNonQuery(query);
        }

        public void DropTableIfAble(string pTableName)
        {
            try
            {
                DropTable(pTableName);
            }
            catch (SQLiteException ex)
            {
                _log.Log($"Could not drop table {pTableName}: {ex.Message}");
                _sqlite.Close();
            }
        }
        #endregion

        #region "INSERT INTO TABLE"
        public void InsertIntoTable(string pTableName, List<string>? pSpecifiedColumns, SQLiteValues pValues)
        {
            if (pValues == null || pValues.IsEmpty())
            {
                throw new ArgumentNullException(nameof(pValues));
            }

            string table = $"{pTableName}";
            if (pSpecifiedColumns != null && pSpecifiedColumns.Count > 0)
            {
                table += $" ({string.Join(",", pSpecifiedColumns)})";
            }

            string query = $"INSERT INTO {table} VALUES ({string.Join(",", pValues)});";
            ExecuteNonQuery(query);
        }

        public void InsertIntoTable(string pTableName, List<string>? pSpecifiedColumns, List<string> pValues)
        {
            InsertIntoTable(pTableName, pSpecifiedColumns, new SQLiteValues(pValues));
        }

        public void InsertAllIntoTable(string pTableName, List<string>? pSpecifiedColumns, List<SQLiteValues> pAllValues)
        {
            foreach (SQLiteValues values in pAllValues)
            {
                InsertIntoTable(pTableName, pSpecifiedColumns, values);
            }
        }

        public void InsertAllIntoTable(string pTableName, List<string>? pSpecifiedColumns, List<List<string>> pAllValues)
        {
            List<SQLiteValues> sqliteValues = [];
            foreach (List<string> values in pAllValues)
            {
                sqliteValues.Add(new(values));
            }

            InsertAllIntoTable(pTableName, pSpecifiedColumns, sqliteValues);
        }
        #endregion

        #region "SELECT"
        public T SelectMany<T>(string pTableName, string? pFields, string? pWhere, string? pGroupBy, string? pOrderBy, uint? pTop, bool pIsDistinct, Func<SQLiteDataReader, T> pCreateModel) where T : BaseDbModelList
        {
            if (string.IsNullOrWhiteSpace(pTableName))
            {
                throw new ArgumentNullException(nameof(pTableName));
            }

            SQLiteQueryBuilder queryBuilder = new();
            // SELECT FIELDS FROM TABLE
            queryBuilder.Select(pFields).Distinct(pIsDistinct).Top(pTop).From(pTableName);

            // Conditions: WHERE X GROUP BY Y ORDER BY Z
            queryBuilder.Where(pWhere).GroupBy(pGroupBy).OrderBy(pOrderBy).Asc(true);
            
            string query = queryBuilder.ToString();
            return ExecuteQueryList(query, pCreateModel);
        }

        public T SelectFirst<T>(string pTableName, string? pFields, string? pWhere, string? pGroupBy, string? pOrderBy, bool pIsDistinct, Func<SQLiteDataReader, T> pCreateModel) where T : BaseDbModel
        {
            if (string.IsNullOrWhiteSpace(pTableName))
            {
                throw new ArgumentNullException(nameof(pTableName));
            }

            SQLiteQueryBuilder queryBuilder = new();
            // SELECT FIELDS FROM TABLE
            queryBuilder.Select(pFields).Distinct(pIsDistinct).Top(1).From(pTableName);

            // Conditions: WHERE X GROUP BY Y ORDER BY Z
            queryBuilder.Where(pWhere).GroupBy(pGroupBy).OrderBy(pOrderBy).Asc(true);

            string query = queryBuilder.ToString();
            return ExecuteQuery(query, pCreateModel);
        }

        public StaticDataDbModelList SelectStaticData(string pTableName)
        {
            static StaticDataDbModelList createModel(SQLiteDataReader pReader)
            {
                
                List<StaticDataDbModel> result = [];
                while (pReader.Read())
                {
                    StaticDataDbModel data = new()
                    {
                        Id = (uint)pReader.GetInt16(0),
                        Name = pReader.GetString(1)
                    };
                    result.Add(data);
                }
                return new(result);
            }

            return SelectMany(pTableName, null, null, null, null, null, false, createModel);
        }
        #endregion

        #region "Helpers"
        private void ExecuteNonQuery(string pQuery)
        {
            if (string.IsNullOrWhiteSpace(pQuery))
            {
                throw new ArgumentNullException(nameof(pQuery));
            }

            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: {pQuery}");

            SQLiteCommand command = _sqlite.CreateCommand();
            command.CommandText = pQuery;

            _sqlite.Open();
            command.ExecuteNonQuery();
            _sqlite.Close();

            _log.TimeStopLogAndAppend(timer, $"END: {pQuery}");
        }

        private T ExecuteQueryList<T>(string pQuery, Func<SQLiteDataReader, T> pCreateModel) where T : BaseDbModelList
        {
            if (string.IsNullOrWhiteSpace(pQuery))
            {
                throw new ArgumentNullException(nameof(pQuery));
            }

            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: {pQuery}");

            BaseModel? modelFromCache = _cache.Get(pQuery);
            if (modelFromCache != null)
            {
                _log.TimeStopLogAndAppend(timer, $"END: Found model from cache.");
                return (T)modelFromCache;
            }

            SQLiteCommand command = _sqlite.CreateCommand();
            command.CommandText = pQuery;

            _sqlite.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            T model = pCreateModel(reader);
            reader.Close();
            _sqlite.Close();

            _cache.Set(pQuery, model);
            _log.TimeStopLogAndAppend(timer, $"END: {pQuery}");

            return model;
        }

        private T ExecuteQuery<T>(string pQuery, Func<SQLiteDataReader, T> pCreateModel) where T : BaseDbModel
        {
            if (string.IsNullOrWhiteSpace(pQuery))
            {
                throw new ArgumentNullException(nameof(pQuery));
            }

            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: {pQuery}");

            BaseModel? modelFromCache = _cache.Get(pQuery);
            if (modelFromCache != null)
            {
                _log.TimeStopLogAndAppend(timer, $"END: Found model from cache.");
                return (T)modelFromCache;
            }

            SQLiteCommand command = _sqlite.CreateCommand();
            command.CommandText = pQuery;

            _sqlite.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            T model = pCreateModel(reader);
            reader.Close();
            _sqlite.Close();

            _cache.Set(pQuery, model);
            _log.TimeStopLogAndAppend(timer, $"END: {pQuery}");

            return model;
        }
        #endregion
    }
}
