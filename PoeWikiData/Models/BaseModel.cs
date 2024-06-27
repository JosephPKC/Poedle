namespace PoeWikiData.Models
{
    public abstract class BaseModel
    {
        public ushort Id { get; set; } = 0;
        public string Name { get; set; } = "";
    }
}
