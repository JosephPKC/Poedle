using System.Text.RegularExpressions;

namespace Poedle.Utils.Strings
{
    public static partial class HTMLTagCleaner
    {
        [GeneratedRegex("<span class=[\\w\\s\"-]+>[\\w\\s- ]*</span>")]
        private static partial Regex SpanClassRegex();

        public static string ParseHTMLTags(string pStr)
        {
            string str = ParseTagBrackets(pStr);
            str = ParseBrackets(str);
            str = ParseQuotes(str);
            str = ParseBreakTags(str);
            return str;
        }

        public static string ParseBreakTags(string pStr)
        {
            return pStr.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n").Replace("<br/ >", "\n");
        }

        public static string ParseTagBrackets(string pStr)
        {
            return pStr.Replace("&lt;", "<").Replace("&gt;", ">");
        }

        public static string ParseQuotes(string pStr)
        {
            return pStr.Replace("&quot;", "\"");
        }

        public static string ParseBrackets(string pStr)
        {
            return pStr.Replace("[[", "").Replace("]]", "");
        }
        
        public static string ReplaceSpanClassTags(string pStr, string pReplaceWith = "")
        {
            return SpanClassRegex().Replace(pStr, pReplaceWith);
        }
    }
}
