using System.Text.RegularExpressions;

namespace BaseToolsUtils.Utils
{
    public static partial class GeneralUtils
    {
        [GeneratedRegex("([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))")]
        private static partial Regex Word();

        public static string DisplayText(string pStr)
        {
            return Word().Replace(pStr, "$1 ");
        }
    }
}
