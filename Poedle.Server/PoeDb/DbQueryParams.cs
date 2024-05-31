namespace Poedle.PoeDb
{
    public static class DbQueryParams
    {
        public enum DbColTypes
        {
            NONE,
            SCORES,
            LEAGUES,
            UNIQUES,
            SKILLGEMS,
            PASSIVES
        }

        public struct DbParams
        {
            public string ColName { get; set; }
        }

        public static readonly Dictionary<DbColTypes, DbParams> DbParamsMap = new()
        {
            { DbColTypes.SCORES, new DbParams() {
                ColName = "scores"
            }},
            { DbColTypes.LEAGUES, new DbParams() {
                ColName = "leagues"
            }},
            { DbColTypes.UNIQUES, new DbParams() {
                ColName = "uniques"
            }},
            { DbColTypes.SKILLGEMS, new DbParams() {
                ColName = "skillgems"
            }},
            { DbColTypes.PASSIVES, new DbParams(){
                ColName = "passives"
            }}
        };

        public static string GetColName(DbColTypes pType)
        {
            return DbParamsMap[pType].ColName;
        }
    }
}
