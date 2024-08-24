namespace Poedle.Server.Data.Results.UniqueItems
{
    public class UniqueItemsResultExpModel : BaseResultExpModel
    {
        public BaseResult ItemClass { get; set; } = new();
        public BaseResult ReqLvl { get; set; } = new();
        public BaseResult ReqDex { get; set; } = new();
        public BaseResult ReqInt { get; set; } = new();
        public BaseResult ReqStr { get; set; } = new();
        public BaseResult LeaguesIntroduced { get; set; } = new();
        public BaseResult ItemAspects { get; set; } = new();
        public BaseResult DropSources { get; set; } = new();
        public BaseResult DropTypes { get; set; } = new();
    }
}
