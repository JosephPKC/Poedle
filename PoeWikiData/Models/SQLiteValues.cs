namespace PoeWikiData.Models
{
    internal class SQLiteValues(IEnumerable<string> pValues)
    {
        private readonly List<string> _values = new(pValues);

        public bool IsEmpty()
        {
            return _values.Count == 0;
        }

        public IList<string> ToList()
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
