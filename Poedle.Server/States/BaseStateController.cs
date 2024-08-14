using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Results;
using Poedle.Server.Data.Stats;
using System.Text.RegularExpressions;
using System.Text;

using PoeWikiData.Models;
using PoeWikiData.Models.LookUps;

namespace Poedle.Server.States
{
    internal abstract class BaseStateController<TResult, TModel> where TResult : BaseResultExpModel where TModel : BaseNamedDbModel
    {
        protected readonly GameState<TResult> _state;
        protected readonly IModelIdLookUp<TModel> _lookUp;
        protected readonly Dictionary<uint, AnswerExpModel> _allAnswers;

        public BaseStateController(IModelIdLookUp<TModel> pLookUp, Dictionary<uint, AnswerExpModel> pAllAnswers)
        {
            _state = new();
            _state.HintThreshold = 3;
            _lookUp = pLookUp;
            _allAnswers = pAllAnswers;

            SetGame();
        }

        public abstract TResult ProcessGuess(uint pGuessId);

        public virtual IEnumerable<AnswerExpModel> GetAllAvailableAnswers()
        {
            return _state.AllAvailableAnswers;
        }

        public virtual AnswerExpModel GetChosenAnswer()
        {
            return _allAnswers[_state.ChosenAnswerId];
        }

        public virtual IEnumerable<string> GetDisplayHints()
        {
            return _state.DisplayHints;
        }

        public virtual string GetNameHint()
        {
            return _state.NameHint;
        }

        public virtual int GetNumberOfGuessesForHint()
        {
            if (_state.Scores.Count == 0)
            {
                return 0;
            }
            // Ex: Threshold: 3
            // 0 Guesses -> 0 % 3 = 0 -> 3 - 0 = 3 Remaining
            // 1 Guess -> 1 % 3 = 1 -> 3 - 1 = 2 Remaining
            // 2 Guess -> 2 % 3 = 2 -> 3 - 2 = 1 Remaining
            // 3 Guess -> 3 % 3 = 0 -> 3 - 0 = 3 Remaining
            // 4 Guess -> 4 % 3 = 1 -> 3 - 1 = 2 Remaining...
            int score = _state.Scores.Last().Score;
            int remainder = score % _state.HintThreshold;
            return _state.HintThreshold - remainder;
        }

        public virtual IEnumerable<TResult> GetResults()
        {
            return _state.Results;
        }

        public virtual StatsExpModel GetStats()
        {
            if (_state.Scores.Count == 0)
            {
                return new();
            }
            return StatsMapper.GetStats(_state.Scores, _state.Scores.Last());
        }

        public virtual bool IsWin()
        {
            return _state.IsWin;
        }

        public virtual void SetGame()
        {
            AnswerExpModel chosenAnswer = ChooseRandomAnswer();
            _state.ChosenAnswerId = chosenAnswer.Value;
            _state.AllAvailableAnswers = new([.. _allAnswers.Values]);
            _state.Results = [];
            _state.Scores.Add(new()
            {
                Id = chosenAnswer.Value,
                Name = chosenAnswer.Label,
                Score = 0
            });
            _state.IsWin = false;

            SetDisplayHints();
            SetHints();
        }

        public virtual void ClearStats()
        {
            _state.Scores = [];
        }

        protected abstract void SetDisplayHints();

        protected virtual AnswerExpModel ChooseRandomAnswer()
        {
            Random rand = new();
            int index = rand.Next(_allAnswers.Count);
            AnswerExpModel answer = _allAnswers.Values.ToList()[index];
            Console.WriteLine($"DEV MODE: Answer => {answer.Value}/{answer.Label}");
            return answer;
        }

        protected virtual TResult ProcessGuess(uint pGuessId, Func<TModel, TModel, TResult> pGetGuessResult)
        {
            TModel? guess = _lookUp.GetById(pGuessId);
            TModel? answer = _lookUp.GetById(_state.ChosenAnswerId);
            if (guess == null || answer == null)
            {
                throw new Exception($"Either guess {pGuessId} or answer {_state.ChosenAnswerId} is invalid and does not exist.");
            }
            Console.WriteLine($"DEV MODE: Guess => {pGuessId}/{guess.Name}. Answer => {answer.Id}/{answer.Name}");

            // Determine win
            if (guess.Id == answer.Id)
            {
                _state.IsWin = true;
            }

            // Process Available Answers
            AnswerExpModel answerModel = _allAnswers[pGuessId];
            _state.AllAvailableAnswers.Remove(answerModel);

            // Update Score
            _state.Scores.Last().Score++;

            // Process Named Hint
            UpdateNamedHint();

            // Process Results
            TResult result = pGetGuessResult(guess, answer);
            _state.Results.AddFirst(result);

            return result;
        }

        protected virtual void UpdateNamedHint()
        {
            if (_state.HintReveals.Count == 0)
            {
                return;
            }

            if (_state.Scores.Last().Score % _state.HintThreshold != 0)
            {
                return;
            }

            TModel? answer = _lookUp.GetById(_state.ChosenAnswerId);
            if (answer == null)
            {
                return;
            }

            int index = _state.HintReveals.Pop();
            string toReveal = answer.Name[index].ToString();
            int hintIndex = index * 2;
            _state.NameHint = string.Concat(_state.NameHint.AsSpan(0, hintIndex), toReveal.ToUpper(), _state.NameHint.AsSpan(hintIndex + 1));
        }

        protected virtual void SetHints()
        {
            TModel? answer = _lookUp.GetById(_state.ChosenAnswerId);
            if (answer == null)
            {
                return;
            }

            StringBuilder hintBuilder = new();
            List<int> reveals = [];
            for (int i = 0; i < answer.Name.Length; i++)
            {
                string current = answer.Name[i].ToString();
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
                (reveals[j], reveals[i]) = (reveals[i], reveals[j]);
            }

            _state.NameHint = hintBuilder.ToString();
            _state.HintReveals = new(reveals);
        }
    }
}
