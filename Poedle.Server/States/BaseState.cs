using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints.Full;
using Poedle.Server.Data.Hints.Shared;
using Poedle.Server.Data.Results;
using Poedle.Server.Data.Scores;

namespace Poedle.Server.States
{
    internal class BaseState<TResult> where TResult : BaseResultExpModel
    {
        // Basic Flags
        public bool IsWin { get; set; } = false;
        // Answers
        public uint ChosenAnswerId { get; set; } = 0;
        public LinkedList<AnswerExpModel> AllAvailableAnswers { get; set; } = [];
        // Hints
        public double HintDifficultyMultiplier { get; set; } = 1;
        public FullSingleHintModel NameHint { get; set; } = new();
        public Queue<HintReveal> HintRevealQueue { get; set; } = [];
        // Results
        public LinkedList<TResult> Results { get; set; } = [];
        // Scores and Stats
        public List<ScoreModel> Scores { get; set; } = [];

        public int CurrentScore
        {
            get
            {
                if (Scores.Count == 0)
                {
                    return 0;
                }
                return Scores.Last().Score;
            }
        }

        public uint NbrGuessesToReveal
        {
            get
            {
                if (HintRevealQueue.Count == 0)
                {
                    return 0;
                }
                return (uint)HintRevealQueue.Peek().ScoreMilestone - (uint)CurrentScore;
            }
        }

        public uint NbrRevealsToLeft
        {
            get
            {
                return (uint)HintRevealQueue.Count;
            }
        }

        public HintTypes NextHintType
        {
            get
            {
                if (HintRevealQueue.Count == 0)
                {
                    return HintTypes.None;
                }
                return HintRevealQueue.Peek().HintType;
            }
        }
    }
}
