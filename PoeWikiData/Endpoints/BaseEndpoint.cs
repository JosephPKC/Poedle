using System.Data.SQLite;
using System.Diagnostics;
using BaseToolsUtils.Logging;

namespace PoeWikiData.Endpoints
{
    public abstract class BaseEndpoint(SQLiteConnection pSqlite, ConsoleLogger pLogger)
    {
        protected readonly SQLiteConnection _sqlite = pSqlite;
        protected readonly ConsoleLogger _log = pLogger;

        #region "SELECT"

        #endregion

        #region "INSERT"

        #endregion

        #region "UPDATE"

        #endregion

        #region "DELETE"

        #endregion

        #region "CREATE"
        protected void CreateTable(string pTableName, List<string> pColumns)
        {
            string query = $"CREATE TABLE {pTableName}({string.Join(",", pColumns)});";

            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: {query}");

            SQLiteCommand command = _sqlite.CreateCommand();
            command.CommandText = query;

            _sqlite.Open();
            command.ExecuteNonQuery();
            _sqlite.Close();

            _log.TimeStopLogAndAppend(timer, $"END: {query}");
        }
        #endregion

        #region "DROP"

        #endregion
    }
}
