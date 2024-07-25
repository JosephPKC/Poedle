using Poedle.Server.Shared.Answers;
using Poedle.Server.Shared.Scores;

namespace Poedle.Server.Shared.State
{
    public class BaseState
    {
        public uint ChosenAnswerId { get; set; } = 0;
        public List<ScoreModel> Scores { get; set; } = [];
        public LinkedList<LiteAnswerModel> AvailableAnswers { get; set; } = [];
        public string Hint { get; set; } = string.Empty;
        public Stack<int> HintReveals { get; set; } = [];
        public bool IsWin { get; set; } = false;
    }
}
