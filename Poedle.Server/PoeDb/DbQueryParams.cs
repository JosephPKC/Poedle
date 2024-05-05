namespace Poedle.PoeDb
{
    public static class DbQueryParams
    {
        public enum DbColTypes
        {
            NONE,
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
    }
}
