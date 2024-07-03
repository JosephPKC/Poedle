namespace PoeWikiData.Endpoints.DbQueryParams
{
    internal static class DataQueryParams
    {
        public enum DbTypes
        {
            NONE,
            LEAGUES
        }

        public class DbParams
        {
            public string TableName { get; set; } = "";
            public List<string> TableColumns { get; set; } = [];
        }

        public class DbColumnParams
        {
            public string Name { get; set; } = "";

        }

        public static readonly Dictionary<DbTypes, DbParams> DbParamsMap = new()
        {
            { DbTypes.LEAGUES, new DbParams() {
                TableName = "Leagues"
            }}
        };

        public static string GetTableName(DbTypes pType)
        {
            return DbParamsMap[pType].TableName;
        }
    }
}
