using PoeWikiData.Models.Leagues;
using PoeWikiData.Models.StaticData;

namespace PoeWikiData.Models.UniqueItems
{
    public class UniqueItemDbModel : BaseDbModel
    {
        // Primary fields
        public StaticDataDbModel ItemClass { get; set; } = new();
        public string BaseItem { get; set; } = "";
        // Required stats
        public uint ReqLvl { get; set; } = 0;
        public uint ReqDex { get; set; } = 0;
        public uint ReqInt { get; set; } = 0;
        public uint ReqStr { get; set; } = 0;
        // Lists
        public List<LeagueDbModel> LeaguesIntroduced { get; set; } = [];
        public List<StaticDataDbModel> ItemAspects { get; set; } = [];
        public List<StaticDataDbModel> DropTypes { get; set; } = [];
        public List<StaticDataDbModel> DropSources { get; set; } = [];
    }
}
