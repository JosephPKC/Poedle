namespace PoeWikiData.Utils.SQLite
{
    internal static class SQLiteUtils
    {
        public static string SQLiteString(string? pString)
        {
            if (string.IsNullOrWhiteSpace(pString))
            {
                return "NULL";
            }

            pString = pString.Replace("'", "''");

            return $"'{pString}'";
        }
    }
}
