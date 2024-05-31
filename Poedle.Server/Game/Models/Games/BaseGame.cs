using Poedle.PoeDb.Models;

namespace Poedle.Game.Models.Games
{
    public abstract class BaseGame
    {
        public BaseDbPoeModel? ModelRef { get; set; }
        public int Score { get; set; } = 0;
        public HashSet<int> GuessedIds { get; set; } = [];
        public bool AreHintsEnabled { get; set; } = false;
    }
}
