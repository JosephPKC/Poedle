using Poedle.Server.Data.Hints.ExpHints;

namespace Poedle.Server.Data.Hints
{
    internal class SingleHintModel : HintExpModel
    {
        public string HintElement { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
        public string FullyRevealedHint { get; set; } = string.Empty;
    }
}
