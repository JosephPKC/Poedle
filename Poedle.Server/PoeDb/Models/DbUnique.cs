using Poedle.Enums;

namespace Poedle.PoeDb.Models
{
    public class DbUnique : BaseDbPoeModel
    {
        public ItemClassesEnum.ItemClasses ItemClass { get; set; } = ItemClassesEnum.ItemClasses.NONE;
        public string BaseItem { get; set; } = "";
        public List<LeaguesEnum.Leagues> LeaguesIntroduced { get; set; } = [];
        public List<string> FlavourText { get; set; } = [];
        public List<string> StatText { get; set; } = [];

        public ushort ReqLvl { get; set; }
        public ushort ReqDex { get; set; }
        public ushort ReqInt { get; set; }
        public ushort ReqStr { get; set; }

        public List<QualitiesEnum.Qualities> Qualities { get; set; } = [];
        public List<DropSourcesEnum.DropSources> DropSources { get; set; } = [];
        public List<DropSourcesSpecificEnum.DropSourcesSpecific> DropSourcesSpecific { get; set; } = [];
    }
}
