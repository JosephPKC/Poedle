using System.Text.RegularExpressions;
using System.Text;

using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints.ExpHints;
using Poedle.Server.Data.Hints;
using Poedle.Server.Data.Results;
using Poedle.Server.Data.Stats;
using PoeWikiData.Models;
using PoeWikiData.Models.LookUps;

namespace Poedle.Server.States
{
    internal abstract class BaseStateController<TResult, TModel, TState> where TResult : BaseResultExpModel where TModel : BaseNamedDbModel where TState : BaseState<TResult>, new()
    {
        protected readonly TState _state;
        protected readonly IModelIdLookUp<TModel> _lookUp;
        protected readonly Dictionary<uint, AnswerExpModel> _allAnswers;

        public BaseStateController(IModelIdLookUp<TModel> pLookUp, Dictionary<uint, AnswerExpModel> pAllAnswers)
        {
            _lookUp = pLookUp;
            _allAnswers = pAllAnswers;

            _state = InitializeState();
            SetGame();
        }

        #region "Public API"
        public virtual IEnumerable<AnswerExpModel> GetAllAvailableAnswers()
        {
            return _state.AllAvailableAnswers;
        }

        public virtual AllHintsExpModel GetHints()
        {
            return new()
            {
                NbrGuessToReveal = _state.NbrGuessesToReveal,
                NbrRevealsLeft = _state.NbrRevealsToLeft,
                NextHintType = _state.NextHintType.ToString(),
                NameHint = _state.NameHint
            };
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
            return StatsMapper.GetStats(_state.Scores, _state.Scores.Last(), _state.NbrRevealsToLeft);
        }

        public virtual bool IsWin()
        {
            return _state.IsWin;
        }

        public abstract TResult ProcessGuess(uint pGuessId);

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

            SetHints();
        }

        public virtual void ClearStats()
        {
            _state.Scores = [];
        }
        #endregion

        #region "Process Guess"
        protected TResult ProcessGuess(uint pGuessId, Func<TModel, TModel, TResult> pGetGuessResult)
        {
            TModel? guess = _lookUp.GetById(pGuessId);
            TModel? answer = _lookUp.GetById(_state.ChosenAnswerId);
            if (guess == null || answer == null)
            {
                throw new Exception($"Either guess {pGuessId} or answer {_state.ChosenAnswerId} is invalid and does not exist.");
            }
            Console.WriteLine($"DEV MODE: Guess => {pGuessId}/{guess.Name}. Answer => {answer.Id}/{answer.Name}");
            Console.WriteLine($"NbrGuessesToReveal: {_state.NbrGuessesToReveal}/ NbrRevealsToLeft: {_state.NbrRevealsToLeft}");

            // Determine win
            if (guess.Id == answer.Id)
            {
                _state.IsWin = true;
            }
            else
            {
                // Process Available Answers
                AnswerExpModel answerModel = _allAnswers[pGuessId];
                _state.AllAvailableAnswers.Remove(answerModel);
            }

            // Update Score
            _state.Scores.Last().Score++;

            // Process Hints
            UpdateHints();

            // Process Results
            TResult result = pGetGuessResult(guess, answer);
            _state.Results.AddFirst(result);

            return result;
        }

        protected void UpdateHints()
        {
            if (_state.HintRevealQueue.Count == 0)
            {
                // No more hint reveals left
                return;
            }

            if (_state.CurrentScore != _state.HintRevealQueue.Peek().ScoreMilestone)
            {
                // Has not reached the next milestone yet.
                return;
            }

            if (IsWin())
            {
                RevealAllHints();
                return;
            }

            HintReveal reveal = _state.HintRevealQueue.Dequeue();
            switch (reveal.HintType)
            {
                case HintTypes.Name:
                    UpdateNameHint();
                    break;
                case HintTypes.None:
                    break;
                default:
                    UpdateOtherHint(reveal.HintType);
                    break;
            }
        }

        protected void UpdateNameHint()
        {
            if (_state.NameHint.RevealQueue.Count == 0)
            {
                // No more letters to reveal
                return;
            }


            int index = _state.NameHint.RevealQueue.Dequeue();
            string toReveal = _state.NameHint.HintElements[index];
            int hintIndex = index * 2;
            _state.NameHint.Hint = string.Concat(_state.NameHint.Hint.AsSpan(0, hintIndex), toReveal.ToUpper(), _state.NameHint.Hint.AsSpan(hintIndex + 1));
        }

        protected virtual void UpdateOtherHint(HintTypes pHintType)
        {
            // Specialized games can have their own set of hints.
            // Update those hints here.
        }

        protected virtual void RevealAllHints()
        {
            _state.NameHint.Hint = _state.NameHint.FullyRevealedHint;
        }
        #endregion

        #region "Set Hints"
        protected virtual void SetHints()
        {
            TModel? answer = _lookUp.GetById(_state.ChosenAnswerId);
            if (answer == null)
            {
                return;
            }

            // Initialize the hints
            CreateHints(answer);
            SetHintRevealQueue();
        }

        /// <summary>
        /// Creates the 'hangman' hint for the name
        /// </summary>
        protected virtual void SetNameHint(TModel pModel)
        {
            // Create the 'hangman' text by:
            // Separating all words via '/'
            // Replacing all characters and numbers with '_'
            // Making sure there are spaces in between each character.
            // Atziri's Acuity -> _ _ _ _ _ _ ' _ / _ _ _ _ _ _
            StringBuilder hintBuilder = new();
            StringBuilder fullyRevealedBuilder = new();
            List<int> reveals = [];
            for (int i = 0; i < pModel.Name.Length; i++)
            {
                string current = pModel.Name[i].ToString();
                if (Regex.Match(current, @"\s") != Match.Empty)
                {
                    // Space
                    hintBuilder.Append("/ ");
                    fullyRevealedBuilder.Append("/ ");
                }
                else if (Regex.Match(current, @"[a-zA-Z0-9]") != Match.Empty)
                {
                    // Letter or Number
                    hintBuilder.Append("_ ");
                    fullyRevealedBuilder.Append(current + " ");
                    reveals.Add(i);
                }
                else
                {
                    // Non space, non letter/number
                    hintBuilder.Append(current + " ");
                    fullyRevealedBuilder.Append(current + " ");
                }
            }

            // Set the letter reveal queue, randomized
            // Remove letters at the end after randomization accordingly to the cut off.
            Random rand = new();
            for (int i = reveals.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (reveals[j], reveals[i]) = (reveals[i], reveals[j]);
            }

            int toCutOff = (int)Math.Round(reveals.Count * _state.NameHint.RevealCutOff, MidpointRounding.ToZero);
            int count = reveals.Count - toCutOff;

            _state.NameHint.RevealQueue = new(reveals[..count]);
            _state.NameHint.Hint = hintBuilder.ToString();
            _state.NameHint.FullyRevealedHint = fullyRevealedBuilder.ToString().ToUpper();
            _state.NameHint.HintElements = pModel.Name.Select((x) => x.ToString()).ToArray();
            _state.NameHintScoreMilestones.Count = _state.NameHint.RevealQueue.Count;
        }

        protected virtual void CreateHints(TModel pModel)
        {
            SetNameHint(pModel);
            // Set specialized game hints here.
        }

        protected virtual void SetHintRevealQueue()
        {
            ICollection<HintReveal> queue = [];
            AddHintsToRevealQueue(queue);
            // Order it by score milestones
            _state.HintRevealQueue = new(queue.OrderBy((x) => x.ScoreMilestone));
        }

        protected virtual void AddHintsToRevealQueue(ICollection<HintReveal> pCol)
        {
            AddHintsToCollection(pCol, _state.NameHintScoreMilestones, HintTypes.Name);
            // Add other hint reveals to queue
        }

        protected static void AddHintsToCollection(ICollection<HintReveal> pCol, HintScoreMilestones pMilestones, HintTypes pHintType)
        {
            foreach (int milestone in pMilestones.ToEnumerable())
            {
                pCol.Add(new()
                {
                    HintType = pHintType,
                    ScoreMilestone = milestone
                });
            }
        }
        #endregion

        #region "Misc Helpers"
        protected AnswerExpModel ChooseRandomAnswer()
        {
            Random rand = new();
            int index = rand.Next(_allAnswers.Count);
            AnswerExpModel answer = _allAnswers.Values.ToList()[index];
            Console.WriteLine($"DEV MODE: Answer => {answer.Value}/{answer.Label}");
            return answer;
        }

        protected virtual TState InitializeState()
        {
            return new()
            {
                NameHint = new()
                {
                    RevealCutOff = 0.4 // Only 60% of a name is revealed.
                },
                // 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, ...
                NameHintScoreMilestones = new()
                {
                    // EndingScore depends on the answer length.
                    StartingScore = 1,
                    ScoreStep = 2
                }
            };
        }
        #endregion
    }
}
