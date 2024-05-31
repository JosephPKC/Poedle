using Poedle.Enums;

using static Poedle.Enums.MiscEnums;

namespace Poedle.Game.Models.Params
{
    public class UniqueParams : BaseParams
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

    public class UniqueParamsResult : BaseParamsResult
    {
        public ParamsResult BaseItem { get; set; } = ParamsResult.CORRECT;
        public ParamsResult LeaguesIntroduced { get; set; } = ParamsResult.CORRECT;
        public ParamsResult Qualities { get; set; } = ParamsResult.CORRECT;
        public ParamsResult DropSources { get; set; } = ParamsResult.CORRECT;
        public ParamsResult DropSourcesSpecific { get; set; } = ParamsResult.CORRECT;
        public ParamsResult ReqLvl { get; set; } = ParamsResult.CORRECT;
        public ParamsResult ReqDex { get; set; } = ParamsResult.CORRECT;
        public ParamsResult ReqInt { get; set; } = ParamsResult.CORRECT;
        public ParamsResult ReqStr { get; set; } = ParamsResult.CORRECT;

    }
}
