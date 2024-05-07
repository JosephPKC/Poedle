namespace Poedle.Utils.Lists
{
    public static class MiscListUtils
    {
        public static void ConditionalAddToList<T>(List<T> pList, T pValue, bool pCondition)
        {
            if (pCondition)
            {
                pList.Add(pValue);
            }
        }
    }
}
