using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiApi.Models;
using PoeWikiData.Endpoints;
using PoeWikiData.Mappers.SQLiteMappers;
using PoeWikiData.Models;
using PoeWikiData.Models.Enums;
using PoeWikiData.Utils;

namespace PoeWikiData
{
    public sealed class PoeDbManager(string pDbFilePath, bool pIsNewDb, ConsoleLogger pLogger)
    {
        private readonly SQLiteConnection _sqlite = new($"Data Source={pDbFilePath};New={pIsNewDb};");
        private readonly PoeDbCache _cache = new();
        private readonly ConsoleLogger _log = pLogger;
        private readonly PoeWikiManager _api = new(pLogger);

        // Base Generics
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

        private List<T> ExecuteQuery<T>(string pQuery, Func<SQLiteDataReader, List<T>> pCreateModel)
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
            SQLiteDataReader reader = command.ExecuteReader();
            List<T> modelList = pCreateModel(reader);
            _sqlite.Close();

            _log.TimeStopLogAndAppend(timer, $"END: {pQuery}");

            return modelList;
        }

        // Generic Queries
        private void Create(string pTableName, List<string> pColumns)
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

        private void DropIfAble(string pTableName)
        {
            try
            {
                Drop(pTableName);
            }
            catch (SQLiteException ex)
            {
                _log.Log($"Could not drop table {pTableName}: {ex.Message}");
                _sqlite.Close();
            }
        }

        private void InsertInto(string pTableName, List<string>? pSpecifiedColumns, List<string> pValues)
        {
            if (pValues == null || pValues.Count == 0)
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

        private void InsertAll<T>(string pTableName, List<string>? pSpecifiedColumns, Func<PoeWikiManager, List<T>> pFromApi, Func<T, List<string>> pToStrings, Action<T>? pPostProcess) where T: BaseWikiModel
        {
            List<T> fromApi = pFromApi(_api);
            foreach (T wikiModel in fromApi)
            {
                List<string> dataStrings = pToStrings(wikiModel);
                InsertInto(pTableName, pSpecifiedColumns, dataStrings);
                pPostProcess?.Invoke(wikiModel);
            }
        }

        private Dictionary<string, string> SelectStaticData(string pTableName, bool pIsReversed)
        {
            Func<SQLiteDataReader, List<KeyValuePair<string, string>>> createModel = (SQLiteDataReader pReader) =>
            {
                List<KeyValuePair<string, string>> result = [];
                while (pReader.Read())
                {
                    string itemClassId = pReader.GetInt16(0).ToString();
                    string name = pReader.GetString(1);

                    KeyValuePair<string, string> data = new(itemClassId, name);
                    result.Add(data);
                }

                return result;
            };

            List<KeyValuePair<string, string>> result = Select(pTableName, null, null, null, null, null, false, createModel);
            Func<KeyValuePair<string, string>, string> ToDictKey = (KeyValuePair<string, string> kv) =>
            {
                return pIsReversed ? kv.Value : kv.Key;
            };
            Func<KeyValuePair<string, string>, string> ToDictVal = (KeyValuePair<string, string> kv) =>
            {
                return pIsReversed ? kv.Key : kv.Value;
            };
            return result.ToDictionary(ToDictKey, ToDictVal);
        }

        private List<T> Select<T>(string pTableName, string? pFields, string? pWhere, string? pGroupBy, string? pOrderBy, ushort? pTop, bool pIsDistinct, Func<SQLiteDataReader, List<T>> pCreateModel)
        {
            if (string.IsNullOrWhiteSpace(pTableName))
            {
                throw new ArgumentNullException(nameof(pTableName));
            }

            string fields = string.IsNullOrWhiteSpace(pFields) ? "*" : pFields;
            if (pTop != null && pTop > 0)
            {
                fields = $"TOP {pTop} {fields}";
            }
            if (pIsDistinct)
            {
                fields = $"DISTINCT {fields}";
            }

            string conditions = "";
            if (!string.IsNullOrWhiteSpace(pWhere))
            {
                conditions += $" WHERE {pWhere}";
            }
            if (!string.IsNullOrWhiteSpace(pGroupBy))
            {
                conditions += $" GROUP BY {pGroupBy}";
            }
            if (!string.IsNullOrWhiteSpace(pOrderBy))
            {
                conditions += $" ORDER BY {pOrderBy}";
            }

            string query = $"SELECT {fields} FROM {pTableName}{conditions}";
            return ExecuteQuery(query, pCreateModel);
        }

        // Segments for Update/Select
        private void UpdateLeagues()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, "BEGIN: UPDATE LEAGUES");

            // Schema values
            string tableName = "Leagues";
            List<string> columns =
            [
                "LeagueId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                "Name TEXT COLLATE NOCASE NOT NULL",
                "ReleaseVersionMajor INTEGER NOT NULL",
                "ReleaseVersionMinor INTEGER NOT NULL",
                "ReleaseVersionPatch INTEGER NOT NULL",
            ];

            DropIfAble(tableName);
            Create(tableName, columns);

            static List<LeagueWikiModel> fromApi(PoeWikiManager api) => api.Leagues.GetAll();
            static List<string> toStrings(LeagueWikiModel model) => LeagueSQLiteMapper.Map(model);
            InsertAll(tableName, null, fromApi, toStrings, null);

            _log.TimeStopLogAndAppend(timer, "END: UPDATE LEAGUES");
        }

        private void UpdateUniqueItems()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, "BEGIN: UPDATE UNIQUE ITEMS");

            // Schema values
            string tableName = "UniqueItems";
            List<string> columnms =
            [
                "UniqueItemId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                "Name TEXT COLLATE NOCASE NOT NULL",
                "ItemClassId INTEGER REFERENCES ItemClasses (ItemClassId) ON DELETE CASCADE ON UPDATE CASCADE",
                "BaseItem TEXT COLLATE NOCASE NOT NULL",
                "ReqLvl INTEGER NOT NULL",
                "ReqDex INTEGER NOT NULL",
                "ReqInt INTEGER NOT NULL",
                "ReqStr INTEGER NOT NULL",
            ];

            DropIfAble(tableName);
            Create(tableName, columnms);

            static List<UniqueItemWikiModel> fromApi(PoeWikiManager api) => api.UniqueItems.GetAll();
            List<string> toStrings(UniqueItemWikiModel model) => UniqueItemsSQLiteMapper.Map(model, GetUniqueItemItemClasses());
            InsertAll(tableName, null, fromApi, toStrings, UpdateUniqueItemLinks);

            _log.TimeStopLogAndAppend(timer, "END: UPDATE UNIQUE ITEMS");
        }

        private void UpdateUniqueItemLinks(UniqueItemWikiModel pModel)
        {
            List<string> DropSources = []; // We need the mapper to get this
            UpdateTableLinks(pModel.Id.ToString(), DropSources, "UniqueItems_DropSources", GetUniqueItemDropSourcesR());
        }

        private void UpdateTableLinks(string pModelId, List<string> pLinkTexts, string pLinkTableName, Dictionary<string, string> pLinkIds)
        {
            foreach (string link in pLinkTexts)
            {
                UpdateTableLink(pModelId, link, pLinkTableName, pLinkIds);
            }
        }

        private void UpdateTableLink(string pModelId, string pLinkText, string pLinkTableName, Dictionary<string, string> pLinkIds)
        {
            if (!pLinkIds.TryGetValue(pLinkText, out string? value))
            {
                return;
            }

            List<string> values =
            [
                pModelId,
                value
            ];

            InsertInto(pLinkTableName, null, values);
        }

        private Dictionary<string, string> GetUniqueItemItemClasses()
        {
            if (_cache.UniqueItemItemClassesR == null)
            {
                _cache.UniqueItemItemClassesR = SelectStaticData("ItemClasses", true);
            }
            return _cache.UniqueItemItemClassesR;
        }

        private Dictionary<string, string> GetUniqueItemDropSources()
        {
            return _cache.UniqueItemDropSources;
        }

        private Dictionary<string, string> GetUniqueItemDropSourcesR()
        {
            return _cache.UniqueItemDropSourcesR;
        }

        // Public API
        public void UpdateData()
        {
            UpdateLeagues();
            UpdateUniqueItems();
        }

        public void ResetMetaData()
        {
            // Drop then Create
        }
    }
}
