namespace BaseToolsUtils.Utils
{
    public static class RandomizerUtils
    {
        public static void RandomizeList<TElement>(IList<TElement> pList)
        {
            Random rand = new();
            for (int i = pList.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (pList[j], pList[i]) = (pList[i], pList[j]);
            }
        }
    }
}
