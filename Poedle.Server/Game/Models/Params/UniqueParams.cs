using Poedle.Enums;

using static Poedle.Enums.MiscEnums;

namespace Poedle.Game.Models.Params
{
    public class UniqueParams
    {
        public ItemClassesEnum.ItemClasses ItemClass { get; set; } = ItemClassesEnum.ItemClasses.NONE;
        public string BaseItem { get; set; } = "";
        public List<LeaguesEnum.Leagues> LeaguesIntroduced { get; set; } = [];
        public List<QualitiesEnum.Qualities> Qualities { get; set; } = [];
        public List<DropSourcesEnum.DropSources> DropSources { get; set; } = [];
        public List<DropSourcesSpecificEnum.DropSourcesSpecific> DropSourcesSpecific { get; set; } = [];

        public uint ReqLvl { get; set; }
        public uint ReqDex { get; set; }
        public uint ReqInt { get; set; }
        public uint ReqStr { get; set; }
    }

    public class UniqueParamsResult
    {
        public ParamsResult BaseItem { get; set; }
        public ParamsResult LeaguesIntroduced { get; set; }
        public ParamsResult Qualities { get; set; }
        public ParamsResult DropSources { get; set; }
        public ParamsResult DropSourcesSpecific { get; set; }
        public ParamsResult ReqLvl { get; set; }
        public ParamsResult ReqDex { get; set; }
        public ParamsResult ReqInt { get; set; }
        public ParamsResult ReqStr { get; set; }

    }
}
