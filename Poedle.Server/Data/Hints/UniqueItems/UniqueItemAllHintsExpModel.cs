using Poedle.Server.Data.Hints.Exp;

namespace Poedle.Server.Data.Hints.UniqueItems
{
    public class UniqueItemAllHintsExpModel : BaseAllHintsExpModel
    {
        public SingleHintExpModel? BaseItemHint { get; set; }
        public StatExpModel? StatModHint { get; set; }
        public ListHintExpModel? FlavourHint { get; set; }
    }
}
