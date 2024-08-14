using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Results;
using Poedle.Server.Data.Scores;

namespace Poedle.Server.States
{
    internal class GameState<TResult> where TResult : BaseResultExpModel
    {
        // Basic Flags
        public bool IsWin { get; set; } = false;
        // Answers
        public uint ChosenAnswerId { get; set; } = 0;
        public LinkedList<AnswerExpModel> AllAvailableAnswers { get; set; } = [];
        // Hints
        public IEnumerable<string> DisplayHints { get; set; } = [];
        public string NameHint { get; set; } = string.Empty;
        public Stack<int> HintReveals { get; set; } = [];
        public int HintThreshold { get; set; } = 0;
        // Results
        public LinkedList<TResult> Results { get; set; } = [];
        // Scores and Stats
        public List<ScoreModel> Scores { get; set; } = [];
    }
}
