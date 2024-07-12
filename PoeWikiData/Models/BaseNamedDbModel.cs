namespace PoeWikiData.Models
{
    public class BaseNamedDbModel : BaseDbModel
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
