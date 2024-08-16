using Poedle.Server.Data.Hints.Full;
using Poedle.Server.Data.Hints.Shared;
using Poedle.Server.Data.Results.UniqueItems;

namespace Poedle.Server.States.UniqueItems
{
    internal class UniqueItemsState : BaseState<UniqueItemsResultExpModel>
    {
        // Specific Hints
        public FullSingleHintModel BaseItemHint { get; set; } = new();
        public FullStatHintModel StatModHint { get; set; } = new();
        public FullListHintModel FlavourTextHint { get; set; } = new();
    }
}
