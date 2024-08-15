using Poedle.Server.Data.Hints;
using Poedle.Server.Data.Hints.ExpHints;
using Poedle.Server.Data.Results.UniqueItems;

namespace Poedle.Server.States.UniqueItems
{
    internal class UniqueItemsState : BaseState<UniqueItemsResultExpModel>
    {
        // Specific Hints
        public HangmanHintModel BaseItemHint { get; set; } = new();
        public HintScoreMilestones BaseItemHintScoreMilestone { get; set; } = new();
        public ListHintModel StatModHint { get; set; } = new();
        public HintScoreMilestones StatModHintScoreMilestone { get; set; } = new();
        public SingleHintModel FlavourTextHint { get; set; } = new();
        public HintScoreMilestones FlavourTextHintScoreMilestone { get; set; } = new();
    }
}
