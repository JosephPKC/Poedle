namespace PoeWikiData.Utils
{
    internal static class BaseUtils
    {
        public static string SetOrAppend(string pString, string? pNewString, bool pIsAppend)
        {
            if (string.IsNullOrWhiteSpace(pNewString))
            {
                return pIsAppend ? pString : "";
            }
            return pIsAppend ? pString + pNewString : pNewString;
        }

        public static T GetEnum<T>(string pEnumName) where T : Enum
        {
            string enumNameNoSpace = pEnumName.Replace(" ", "").Replace("-", "");
            return (T)Enum.Parse(typeof(T), enumNameNoSpace, true);
        }

        public static void ConditionalAddToList<T>(List<T> pList, T pValue, bool pCondition)
        {
            if (pCondition)
            {
                pList.Add(pValue);
            }
        }

        public static bool ContainsIgnoreCase(string pStr, string pSubStr)
        {
            return pStr.Contains(pSubStr, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool ContainsAnyIgnoreCase(string pStr, List<string> pSubStrs)
        {
            foreach (string pSubStr in pSubStrs)
            {
                if (ContainsIgnoreCase(pStr, pSubStr)) return true;
            }

            return false;
        }

        public static bool ContainsIgnoreCase(List<string> pStrs, string pStr)
        {
            return pStrs.Any(x => x.Equals(pStr, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool ContainsSubIgnoreCase(List<string> pStrs, string pSubStr)
        {
            return pStrs.Any(x => x.Contains(pSubStr, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool EqualsIgnoreCase(string pStr1, string pStr2)
        {
            return pStr1.Equals(pStr2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
