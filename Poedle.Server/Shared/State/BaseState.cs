using Poedle.Server.Shared.Scores;

namespace Poedle.Server.Shared.State
{
    public class BaseState
    {
        public uint ChosenAnswerId { get; set; } = 0;
        public List<ScoreModel> Scores { get; set; } = [];
    }
}
