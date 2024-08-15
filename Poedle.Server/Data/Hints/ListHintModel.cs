using Poedle.Server.Data.Hints.ExpHints;

namespace Poedle.Server.Data.Hints
{
    internal class ListHintModel : HintListExpModel
    {
        public IList<string> HintElements { get; set; } = [];
        public Queue<int> RevealQueue { get; set; } = [];
        public IEnumerable<string> FullyRevealedHint { get; set; } = [];
    }
}
