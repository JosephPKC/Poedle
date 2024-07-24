using Poedle.Server.Shared.Answers;
using Poedle.Server.Shared.Results;
using Poedle.Server.Shared.Stats;

namespace Poedle.Server.Shared.State
{
    internal abstract class BaseStateController(Dictionary<uint, FullAnswerModel> pAllAnswers, Dictionary<uint, LiteAnswerModel> pAllAnswersLite)
    {
        protected readonly Dictionary<uint, FullAnswerModel> _allAnswers = pAllAnswers;
        protected readonly Dictionary<uint, LiteAnswerModel> _allAnswersLite = pAllAnswersLite;

        public abstract IEnumerable<LiteAnswerModel> GetAllAvailableAnswers();
        public abstract FullAnswerModel GetChosenAnswer();
        public abstract BaseResult ProcessResult(uint pGuessId);
        //public abstract IEnumerable<BaseResult> GetAllGuessResults();
        public abstract void UpdateScore(int pScore);
        public abstract StatsModel GetStats();
        public abstract void SetGame();

        protected virtual uint ChooseRandomAnswer()
        {
            Random rand = new();
            int index = rand.Next(_allAnswers.Count);
            Console.WriteLine($"Count: {_allAnswers.Count}, Rand: {index}");
            return _allAnswers.Values.ToList()[index].Value;
        }
    }
}
