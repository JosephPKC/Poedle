namespace PoeWikiData.Utils
{
    internal static class SQLiteHelper
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
