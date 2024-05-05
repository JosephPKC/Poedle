namespace Poedle.PoeDb.Models
{
    public abstract class BaseDbPoeModel : BaseDbModel
    {
        public string Name { get; set; } = "";
        public string PageName { get; set; } = "";
    }
}
