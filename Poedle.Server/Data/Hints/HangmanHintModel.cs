using Poedle.Server.Data.Hints.ExpHints;

namespace Poedle.Server.Data.Hints
{
    internal class HangmanHintModel : HintExpModel
    {
        public double RevealCutOff { get; set; } = 0;
        public IList<string> HintElements { get; set; } = [];
        public Queue<int> RevealQueue { get; set; } = [];
        public string FullyRevealedHint { get; set; } = string.Empty;
    }
}
