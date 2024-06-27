using PoeWikiData.Models.Enums;

namespace PoeWikiData.Models
{
    public class UniqueItemModel : BaseModel
    {
        // Primary fields
        public ItemClasses ItemClass { get; set; } = ItemClasses.None;
        public string BaseItem { get; set; } = "";
        // Required stats
        public ushort ReqLvl { get; set; } = 0;
        public ushort ReqDex { get; set; } = 0;
        public ushort ReqInt { get; set; } = 0;
        public ushort ReqStr { get; set; } = 0;
        // Lists
        public List<string> LeaguesIntroduced { get; set; } = [];
        public List<ItemAspects> ItemAspects { get; set; } = [];
        public List<DropTypes> DropTypes { get; set; } = [];
        public List<DropSources> DropSources { get; set; } = [];
    }
}
