namespace PoeWikiData.Utils
{
    internal static class ListUtils
    {
        public static void ConditionalAddToList<T>(ICollection<T> pList, T pValue, bool pCondition)
        {
            if (pCondition)
            {
                pList.Add(pValue);
            }
        }
    }
}
