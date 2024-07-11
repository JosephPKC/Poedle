namespace PoeWikiData.Models.StaticData
{
    public class StaticDataDbModel : BaseDbModel
    {
        public StaticDataDbModel() : base() { }
        
        public StaticDataDbModel(uint pId, string pName) : base()
        {
            Id = pId;
            Name = pName;
        }
    }
}
