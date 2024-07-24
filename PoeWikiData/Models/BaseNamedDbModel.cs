namespace PoeWikiData.Models
{
    public class BaseNamedDbModel : BaseDbModel, IComparable<BaseNamedDbModel>
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        public int CompareTo(BaseNamedDbModel? other)
        {
            if (other == null) return 1;

            return string.Compare(Name, other.Name);
        }
    }
}
