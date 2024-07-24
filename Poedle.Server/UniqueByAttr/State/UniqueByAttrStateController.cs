using Poedle.Server.Shared.Answers;
using Poedle.Server.Shared.Results;
using Poedle.Server.Shared.State;
using Poedle.Server.Shared.Stats;
using Poedle.Server.UniqueByAttr.Results;
using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.UniqueByAttr.State
{
    internal class UniqueByAttrStateController : BaseStateController
    {
        private readonly UniqueByAttrState _state;
        private readonly UniqueItemDbLookUp _allDbModels;

        public UniqueByAttrStateController(Dictionary<uint, FullAnswerModel> pAllAnswers, Dictionary<uint, LiteAnswerModel> pAllAnswersLite, UniqueItemDbLookUp pLookUp) : base(pAllAnswers, pAllAnswersLite)
        {
            _allDbModels = pLookUp;
            _state = new()
            {
                ChosenAnswerId = ChooseRandomAnswer(),
                Scores = [],
                Results = []
            };
        }

        public override IEnumerable<LiteAnswerModel> GetAllAvailableAnswers()
        {
            return [.. _allAnswersLite.Values];
        }

        public override FullAnswerModel GetChosenAnswer()
        {
            return _allAnswers[_state.ChosenAnswerId];
        }

        public override BaseResult ProcessResult(uint pGuessId)
        {
            UniqueItemDbModel? guess = _allDbModels.GetById(pGuessId);
            UniqueItemDbModel? answer = _allDbModels.GetById(_state.ChosenAnswerId);
            if (guess == null || answer == null)
            {
                throw new Exception($"Either guess {pGuessId} or answer {_state.ChosenAnswerId} is invalid and does not exist.");
            }
            UniqueByAttrResult result =  UniqueByAttrResultMapper.GetGuessResult(guess, answer);
            _state.Results.AddFirst(result);
            return result;
        }

        // For now, it is being processed on the client side.
        // Trying to do this on the server side sometimes causes the result to not show up until after due to the async nature.
        // By doing it client side, this means that on refresh the game is reset which is fine since there is no persistent data tracking.
        //public override IEnumerable<BaseResult> GetAllGuessResults()
        //{
        //    return _state.Results;
        //}

        public override void UpdateScore(int pScore)
        {
            _state.Scores.Add(new()
            {
                Id = _state.ChosenAnswerId,
                Name = _allAnswers[_state.ChosenAnswerId].Label,
                Score = pScore
            });
        }

        public override StatsModel GetStats()
        {
            return StatsModelMapper.GetStats(_state.Scores);
        }

        public override void SetGame()
        {
            _state.ChosenAnswerId = ChooseRandomAnswer();
            _state.Results = [];
        }
    }
}
