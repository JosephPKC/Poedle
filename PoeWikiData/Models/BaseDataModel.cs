namespace PoeWikiData.Models
{
    public abstract class BaseDataModel
    {
        public ushort Id { get; set; } = 0;
        public string Name { get; set; } = "";
    }
}
