using System.Text.RegularExpressions;

namespace PoeWikiData.Utils
{
    internal static partial class HtmlTextCleaner
    {
        #region "Regex"

        [GeneratedRegex("<span class=\"[\\w\\s-]+\">")]
        private static partial Regex SpanClassStartTag();
        [GeneratedRegex("</span>")]
        private static partial Regex SpanClassEndTag();
        #endregion

        #region "Basic Tags"
        public static string ParseBasicHTMLTags(string pStr)
        {
            string str = ParseAmps(pStr);
            str = ParseTagBrackets(str);
            str = ParseBrackets(str);
            str = ParseQuotes(str);
            str = ParseBreakTags(str);
            return str;
        }

        public static string ParseAmps(string pStr)
        {
            return pStr.Replace("&amp;", "&");
        }

        public static string ParseTagBrackets(string pStr)
        {
            return pStr.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&#60;", "<").Replace("&#62;", ">");
        }

        public static string ParseQuotes(string pStr)
        {
            return pStr.Replace("&quot;", "\"");
        }

        public static string ParseBreakTags(string pStr)
        {
            return pStr.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n").Replace("<br/ >", "\n");
        }

        public static string ParseBrackets(string pStr)
        {
            return pStr.Replace("[[", string.Empty).Replace("]]", string.Empty);
        }

        public static string ParseDoubleBracketTags(string pStr)
        {
            return pStr.Replace("<<", "<").Replace(">>", ">");
        }
        #endregion

        /// <summary>
        /// Format: [[word1|word2]]
        /// Extracts the format and replaces it with word2.
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static string ReplaceBracketGroupWithSecond(string pStr)
        {
            string firstWordInBracket = "[\\w\\s]+\\|";
            string doubleWordsInBrackets = $"\\[\\[{firstWordInBracket}([\\w\\s,]+)\\]\\]";
            // Extract [[word1|word2]], and replace it with word2.
            Match match = Regex.Match(pStr, doubleWordsInBrackets);
            if (!match.Success) return pStr;

            return Regex.Replace(pStr, doubleWordsInBrackets, "$1");
        }

        #region "Span Class"
        /// <summary>
        /// Format: <span class="class name">inner text</span>
        /// Extracts the format and replaces it with inner text.
        /// </summary>
        /// <param name="pStr"></param>
        /// <param name="pClassName"></param>
        /// <returns></returns>
        public static string ReplaceSpanClassWithInnerText(string pStr, string pClassName)
        {
            string spanClassStartTag = $"<span class=\"{pClassName}\">";
            string spanClassEndTag = "<\\/span>";
            string spanClass = $"{spanClassStartTag}([\\w\\s\\d\\<\\>\\(\\)\":,.-]+){spanClassEndTag}";
            Match match = Regex.Match(pStr, spanClass);
            if (!match.Success) return pStr;

            return Regex.Replace(pStr, spanClass, "$1");
        }

        /// <summary>
        /// Format: <span class="class name">...</span>
        /// Replaces the whole format with pReplace.
        /// </summary>
        /// <param name="pStr"></param>
        /// <param name="pClassName"></param>
        /// <param name="pReplace"></param>
        /// <returns></returns>
        public static string ReplaceSpanClass(string pStr, string pClassName, string pReplace = "")
        {
            return Regex.Replace(pStr, $"<span class=\"{pClassName}\">[\\w\\s\\d\\<\\>\\(\\)\":,.-]*<\\/span>", pReplace);
        }

        /// <summary>
        /// Removes the <span class></span> tags (just the tags).
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static string RemoveSpanClassTags(string pStr)
        {
            // Clean front tags
            pStr = SpanClassStartTag().Replace(pStr, string.Empty);
            // Clean end tags
            return SpanClassEndTag().Replace(pStr, string.Empty);
        }
        #endregion

        #region "Table Class
        /// <summary>
        /// Format: <table class="...">header & inner text</table>
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static string ReplaceTableClassWithHeader(string pStr)
        {
            string spanTable = "<table[\\w\\s\\d\\[\\]\\<\\>\\(\\);,.\"':/@#$%&+=-]+<\\/table>";

            string tableHeaderStartTag = "<th class=\"mw-customtoggle-31\">";
            string tableHeaderEndTag = "</th>";
            string tableHeaderClass = $"{tableHeaderStartTag}[\\w\\s\\d\\[\\]\\<\\>\\(\\);,.\"':/@#$%&+=-]*{tableHeaderEndTag}";
            Match m = Regex.Match(pStr, spanTable);
            if (!m.Success)
            {
                return pStr;
            }

            m = Regex.Match(pStr, tableHeaderClass);
            if (!m.Success)
            {
                return pStr;
            }

            string header = m.Groups[0].Value;
            // Clean up table tags
            header = Regex.Replace(header, tableHeaderStartTag, string.Empty);
            header = Regex.Replace(header, tableHeaderEndTag, string.Empty);
            // Clean up additional span class tags
            header = RemoveSpanClassTags(header);
            return Regex.Replace(pStr, spanTable, header);
        }
        #endregion

        #region "Abbr"
        public static string ReplaceAbbrWithInnerText(string pStr)
        {
            string abbrStartTag = "<abbr title=\"[\\w\\d\\s.]+\">";
            string abbrEndTag = "<\\/abbr>";
            string abbrTitle = $"{abbrStartTag}[\\w\\d\\s%]+{abbrEndTag}";
            Match m = Regex.Match(pStr, abbrTitle);
            if (!m.Success)
            {
                return pStr;
            }

            string inner = m.Groups[0].Value;
            // Clean up tags
            inner = Regex.Replace(inner, abbrStartTag, string.Empty);
            inner = Regex.Replace(inner, abbrEndTag, string.Empty);
            return Regex.Replace(pStr, abbrTitle, inner);
        }

        #endregion

        public static string CleanTextEmbed(string pStr, string pEmbedText)
        {
            if (!StringUtils.ContainsIgnoreCase(pStr, $"({pEmbedText})|")) return pStr;
            return Regex.Replace(pStr, $"^[\\w\\s]+\\({pEmbedText}\\)\\|", string.Empty);
        }
    }
}
