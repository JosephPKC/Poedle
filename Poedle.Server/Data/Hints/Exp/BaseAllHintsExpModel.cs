namespace Poedle.Server.Data.Hints.Exp
{
    public abstract class BaseAllHintsExpModel
    {
        public uint NbrGuessToReveal { get; set; } = 0;
        public uint NbrRevealsLeft { get; set; } = 0;
        public string NextHintType { get; set; } = string.Empty;
        public SingleHintExpModel? NameHint { get; set; }
    }
}
