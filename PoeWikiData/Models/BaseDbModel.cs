namespace PoeWikiData.Models
{
    public abstract class BaseDbModel : BaseModel
    {
        public uint Id { get; set; } = 0;
        public string Name { get; set; } = "";
    }
}
