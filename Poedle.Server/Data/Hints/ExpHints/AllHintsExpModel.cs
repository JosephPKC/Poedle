namespace Poedle.Server.Data.Hints.ExpHints
{
    public class AllHintsExpModel
    {
        public uint NbrGuessToReveal { get; set; } = 0;
        public uint NbrRevealsLeft { get; set; } = 0;
        public string NextHintType { get; set; } = string.Empty;
        public HintExpModel? NameHint { get; set; }
        public HintExpModel? BaseItemHint { get; set; }
        public HintListExpModel? StatModHint { get; set; }
        public HintExpModel? FlavourHint { get; set; }
    }
}
