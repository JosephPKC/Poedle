namespace Poedle.Server.Data.Hints.Exp
{
    public class AllHintsExpModel
    {
        public uint NbrGuessToReveal { get; set; } = 0;
        public uint NbrRevealsLeft { get; set; } = 0;
        public string NextHintType { get; set; } = string.Empty;
        public SingleHintExpModel? NameHint { get; set; }
        public SingleHintExpModel? BaseItemHint { get; set; }
        public StatExpModel? StatModHint { get; set; }
        public ListHintExpModel? FlavourHint { get; set; }
    }
}
