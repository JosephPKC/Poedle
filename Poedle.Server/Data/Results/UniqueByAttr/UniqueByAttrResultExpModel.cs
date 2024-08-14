namespace Poedle.Server.Data.Results.UniqueByAttr
{
    public class UniqueByAttrResultExpModel : BaseResultExpModel
    {
        public string ItemClass { get; set; } = string.Empty;
        public ResultStates ItemClassResult { get; set; }
        public string BaseItem { get; set; } = string.Empty;
        public ResultStates BaseItemResult { get; set; }
        public string ReqLvl { get; set; } = string.Empty;
        public ResultStates ReqLvlResult { get; set; }
        public string ReqDex { get; set; } = string.Empty;
        public ResultStates ReqDexResult { get; set; }
        public string ReqInt { get; set; } = string.Empty;
        public ResultStates ReqIntResult { get; set; }
        public string ReqStr { get; set; } = string.Empty;
        public ResultStates ReqStrResult { get; set; }
        public string LeaguesIntroduced { get; set; } = string.Empty;
        public ResultStates LeaguesIntroducedResult { get; set; }
        public string ItemAspects { get; set; } = string.Empty;
        public ResultStates ItemAspectsResult { get; set; }
        public string DropSources { get; set; } = string.Empty;
        public ResultStates DropSourcesResult { get; set; }
        public string DropTypes { get; set; } = string.Empty;
        public ResultStates DropTypesResult { get; set; }
    }
}
