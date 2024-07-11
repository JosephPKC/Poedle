using PoeWikiData.Models;
using PoeWikiData.Models.Enums;

namespace Poedle.Server.ByAttr
{
    public class ByAttrGameState
    {
        public bool UseHints { get; set; }

        public uint Score { get; set; }
    }


    public enum ResultStates
    {
        CORRECT,
        PARTIAL,
        WRONG
    }

    public class  AttributeState
    {
        // Primary fields
        public uint Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string ItemClass { get; set; } = "";
        public string BaseItem { get; set; } = "";
        // Required stats
        public string ReqLvl { get; set; } = "";
        public string ReqDex { get; set; } = "";
        public string ReqInt { get; set; } = "";
        public string ReqStr { get; set; } = "";
        // Lists
        public string LeaguesIntroduced { get; set; } = "";
        public string ItemAspects { get; set; } = "";
        public string DropTypes { get; set; } = "";
        public string DropSources { get; set; } = "";
    }

    public struct AttributeResults
    {
        public ResultStates Name { get; set; }
        public ResultStates ItemClass { get; set; }
        public ResultStates BaseItem { get; set; }
        public ResultStates LeaguesIntroduced { get; set; }
        public ResultStates ItemAspects { get; set; }
        public ResultStates DropTypes { get; set; }
        public ResultStates DropSources { get; set; }
        public ResultStates ReqLvl { get; set; }
        public ResultStates ReqDex { get; set; }
        public ResultStates ReqInt { get; set; }
        public ResultStates ReqStr { get; set; }
    }

    public struct GuessResult
    {
        public AttributeState Attributes { get; set; }
        public AttributeResults Results { get; set; }
    }
}
