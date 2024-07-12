namespace PoeWikiData.Schema
{
    internal class PoeDbSchema
    {
        public string Table { get; set; } = string.Empty;
        public IEnumerable<string> Columns { get; set; } = [];
    }
}
