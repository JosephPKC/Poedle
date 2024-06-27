using System.Data.SQLite;
using BaseToolsUtils.Logging;
using static PoeWikiData.Endpoints.DbQueryParams.DbQueryParams;

namespace PoeWikiData.Endpoints
{
    public class LeagueEndpoint : BaseEndpoint
    {
        public LeagueEndpoint(SQLiteConnection pSqlite, ConsoleLogger pLogger) : base(pSqlite, pLogger)
        {

        }

        public void CreateTables()
        {
            List<string> columns = new()
            {
                "TestId INTEGER PRIMARY KEY UNIQUE NOT NULL",
                "Name TEXT NOT NULL COLLATE NOCASE"
            };
            CreateTable(GetTableName(DbTypes.LEAGUES), columns);
        }
    }
}
