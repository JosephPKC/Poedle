namespace Poedle.Utils.Strings
{
    public static class MiscStringUtils
    {
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
