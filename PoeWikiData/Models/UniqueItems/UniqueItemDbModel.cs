using PoeWikiData.Models.Leagues;
using PoeWikiData.Models.StaticData;

namespace PoeWikiData.Models.UniqueItems
{
    public class UniqueItemDbModel : BaseNamedDbModel
    {
        // Primary fields
        public StaticDataDbModel ItemClass { get; set; } = new();
        public string BaseItem { get; set; } = string.Empty;
        // Required stats
        public uint ReqLvl { get; set; } = 0;
        public uint ReqDex { get; set; } = 0;
        public uint ReqInt { get; set; } = 0;
        public uint ReqStr { get; set; } = 0;
        // Lists
        public IEnumerable<LeagueDbModel> LeaguesIntroduced { get; set; } = [];
        public IEnumerable<StaticDataDbModel> ItemAspects { get; set; } = [];
        public IEnumerable<StaticDataDbModel> DropTypes { get; set; } = [];
        public IEnumerable<StaticDataDbModel> DropSources { get; set; } = [];
        // Other
        public IEnumerable<string> FlavourText { get; set; } = [];
        public IEnumerable<string> ImplicitStatText { get; set; } = [];
        public IEnumerable<string> ExplicitStatText { get; set; } = [];
    }
}
