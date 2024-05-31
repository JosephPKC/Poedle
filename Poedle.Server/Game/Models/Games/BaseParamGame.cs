using Poedle.Game.Models.Params;

namespace Poedle.Game.Models.Games
{
    public abstract class BaseParamGame : BaseGame
    {
        public BaseParams? CorrectParams { get; set; }
        public List<BaseParams> GuessedParams { get; set; } = [];
    }
}
