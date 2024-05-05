namespace Poedle.Utils.Strings
{
    public static class StringCleaner
    {
        public static string CleanUpEndlines(string pStr)
        {
            return pStr.Trim().Replace("\n\n", "\n");
        }

        public static List<string> SeparateStringLines(string pStr, bool pbCleanNull = true, bool pbCleanEndlines = true)
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
    }
}
