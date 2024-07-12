using System.Globalization;
using System.Text.RegularExpressions;

namespace PoeWikiData.Utils
{
    internal static class StringUtils
    {
        public static string SetOrAppend(string pString, string? pNewString, bool pIsAppend)
        {
            if (string.IsNullOrWhiteSpace(pNewString))
            {
                return pIsAppend ? pString : "";
            }
            return pIsAppend ? pString + pNewString : pNewString;
        }

        public static string NoSpaceDash(string pStr)
        {
            return pStr.Replace(" ", "").Replace("-", "").Replace("_", "");
        }

        public static string TitleCase(string pStr)
        {
            Console.WriteLine($"{pStr}");
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(NoSpaceDash(pStr.ToLower()));
        }

        public static string DisplayText(string pStr)
        {
            return Regex.Replace(pStr, "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1 ");
        }

        public static string CleanUpEndlines(string pStr)
        {
            return pStr.Trim().Replace("\n\n", "\n");
        }

        public static IEnumerable<string> SeparateStringLines(string pStr, bool pbCleanNull = true, bool pbCleanEndlines = true)
        {
            string str = pStr;
            if (pbCleanEndlines)
            {
                str = CleanUpEndlines(str);
            }

            List<string> cleanedTexts = [.. str.Split("\n")];

            if (pbCleanNull)
            {
                cleanedTexts.ForEach(x => x = x.Trim());
            }

            return cleanedTexts;
        }

        public static IEnumerable<string> RemoveBlankLines(IEnumerable<string> pStrs)
        {
            ICollection<string> cleanedStrs = [];
            foreach (string str in pStrs)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    cleanedStrs.Add(str);
                }
            }
            return cleanedStrs;
        }

        public static bool ContainsIgnoreCase(string pStr, string pSubStr)
        {
            return pStr.Contains(pSubStr, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool ContainsAnyIgnoreCase(string pStr, IEnumerable<string> pSubStrs)
        {
            foreach (string pSubStr in pSubStrs)
            {
                if (ContainsIgnoreCase(pStr, pSubStr)) return true;
            }

            return false;
        }

        public static bool ContainsIgnoreCase(IEnumerable<string> pStrs, string pStr)
        {
            return pStrs.Any(x => x.Equals(pStr, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool ContainsSubIgnoreCase(IEnumerable<string> pStrs, string pSubStr)
        {
            return pStrs.Any(x => x.Contains(pSubStr, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool EqualsIgnoreCase(string pStr1, string pStr2)
        {
            return pStr1.Equals(pStr2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
