namespace PoeWikiData.Models.StaticData
{
    public class StaticDataDbModel : BaseNamedDbModel, IEquatable<StaticDataDbModel>
    {
        public StaticDataDbModel() : base() { }

        public StaticDataDbModel(uint pId, string pName) : base()
        {
            Id = pId;
            Name = pName;
        }

        public bool Equals(StaticDataDbModel? other)
        {
            if (this == other) return true;

            if (other == null) return false;

            return Id == other.Id;
        }
    }
}
