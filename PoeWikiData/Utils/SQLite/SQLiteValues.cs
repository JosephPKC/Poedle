namespace PoeWikiData.Utils.SQLite
{
    internal class SQLiteValues(IEnumerable<string> pValues)
    {
        private readonly List<string> _values = new(pValues);

        public int Count()
        {
            return _values.Count;
        }

        public bool IsEmpty()
        {
            return _values.Count == 0;
        }

        public List<string> ToList()
        {
            List<string> valuesList = new(_values);
            return valuesList;
        }

        public override string ToString()
        {
            return string.Join(", ", _values);
        }
    }
}
