using Poedle.Server.Shared.Answers;
using Poedle.Server.Shared.Results;
using Poedle.Server.Shared.State;
using Poedle.Server.Shared.Stats;
using Poedle.Server.UniqueByAttr.Results;
using PoeWikiData.Models.UniqueItems;
using System.Text;
using System.Text.RegularExpressions;

namespace Poedle.Server.UniqueByAttr.State
{
    internal class UniqueByAttrStateController : BaseStateController
    {
        private readonly UniqueByAttrState _state;
        private readonly UniqueItemDbLookUp _allDbModels;

        public UniqueByAttrStateController(Dictionary<uint, FullAnswerModel> pAllAnswers, Dictionary<uint, LiteAnswerModel> pAllAnswersLite, UniqueItemDbLookUp pLookUp) : base(pAllAnswers, pAllAnswersLite)
        {
            _allDbModels = pLookUp;
            _state = new();

            SetGame();
        }

        public override IEnumerable<LiteAnswerModel> GetAllAvailableAnswers()
        {

            return _state.AvailableAnswers;
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
            LiteAnswerModel liteAnswer = _allAnswersLite[pGuessId];
            _state.Results.AddFirst(result);
            Console.WriteLine("REMOVE: " + _state.AvailableAnswers.Remove(liteAnswer));
            //UpdateScore();

            // process hint
            if (_state.HintReveals.Count > 0)
            {
                string hintName = GetChosenAnswer().HintName;
                int index = _state.HintReveals.Pop();
                string toReveal = hintName[index].ToString();
                int hintIndex = index * 2;
                _state.Hint = _state.Hint.Substring(0, hintIndex) + toReveal.ToUpper() + _state.Hint.Substring(hintIndex + 1);
            }

            return result;
        }

        // For now, it is being processed on the client side.
        // Trying to do this on the server side sometimes causes the result to not show up until after due to the async nature.
        // By doing it client side, this means that on refresh the game is reset which is fine since there is no persistent data tracking.
        public override IEnumerable<BaseResult> GetAllGuessResults()
        {
            return _state.Results;
        }

        public override void UpdateScore(int pScore)
        {
            _state.Scores.Add(new()
            {
                Id = _state.ChosenAnswerId,
                Name = _allAnswers[_state.ChosenAnswerId].Label,
                Score = pScore
            });
        }

        public override void UpdateScore()
        {
            _state.Scores.Last().Score++;
        }

        public override int GetScore()
        {
            return _state.Scores.Last().Score;
        }

        public override string GetHint()
        {
            return _state.Hint;
        }

        public override StatsModel GetStats()
        {
            return StatsModelMapper.GetStats(_state.Scores);
        }

        public override void SetGame()
        {
            _state.ChosenAnswerId = ChooseRandomAnswer();
            _state.Results = [];
            _state.AvailableAnswers = new([.. _allAnswersLite.Values]);
            _state.Scores.Add(new()
            {
                Id = _state.ChosenAnswerId,
                Name = _allAnswers[_state.ChosenAnswerId].Label,
                Score = 0
            });
            _state.IsWin = false;
            SetHints();
        }

        public override bool SetIsWin(uint pGuessId)
        {
            _state.IsWin = pGuessId == _state.ChosenAnswerId;
            return IsWin();
        }

        public override bool IsWin()
        {
            return _state.IsWin;
        }

        private void SetHints()
        {
            StringBuilder hintBuilder = new();
            string hintName = GetChosenAnswer().HintName;
            List<int> reveals = [];
            for (int i = 0; i < hintName.Length; i++)
            {
                string current = hintName[i].ToString();
                if (Regex.Match(current, @"\s") != Match.Empty)
                {
                    // Space
                    hintBuilder.Append("/ ");
                }
                else if (Regex.Match(current, @"[a-zA-Z0-9]") != Match.Empty)
                {
                    // Letter or Number
                    hintBuilder.Append("_ ");
                    reveals.Add(i);
                }
                else
                {
                    // Non space, non letter/number
                    hintBuilder.Append(current + " ");
                }
            }

            Random rand = new();
            for (int i = reveals.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = reveals[i];
                reveals[i] = reveals[j];
                reveals[j] = temp;
            }

            _state.Hint = hintBuilder.ToString();
            _state.HintReveals = new(reveals);
        }
    }
}
