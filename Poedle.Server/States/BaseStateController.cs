using System.Text.RegularExpressions;

using BaseToolsUtils.Utils;
using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints;
using Poedle.Server.Data.Results;
using Poedle.Server.Data.Stats;
using PoeWikiData.Models;
using PoeWikiData.Models.LookUps;
using Poedle.Server.Data.Hints.Shared;
using Poedle.Server.Data.Hints.Full;
using Poedle.Server.Data.Hints.Exp;

namespace Poedle.Server.States
{
    internal abstract partial class BaseStateController<TResult, TModel, TState, TAllHints> where TResult : BaseResultExpModel where TModel : BaseNamedDbModel where TState : BaseState<TResult>, new() where TAllHints : BaseAllHintsExpModel, new()
    {
        [GeneratedRegex(@"\s")]
        private static partial Regex WhitespaceOnly();
        [GeneratedRegex(@"[\w]")]
        private static partial Regex CharNumsOnly();

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

        public virtual TAllHints GetHints()
        {
            return new()
            {
                NbrGuessToReveal = _state.NbrGuessesToReveal,
                NbrRevealsLeft = _state.NbrRevealsToLeft,
                NextHintType = GetHintTypeText(_state.NextHintType),
                NameHint = HintMapper.Map(_state.NameHint)
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
                Name = chosenAnswer.AbbrName,
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
            if (IsWin())
            {
                RevealAllHints();
                return;
            }

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
            static string UpdateHint(int pIndex, string pToReveal, string pCurrentHint)
            {
                int hintIndex = pIndex * 2;
                return string.Concat(pCurrentHint.AsSpan(0, hintIndex), pToReveal.ToUpper(), pCurrentHint.AsSpan(hintIndex + 1));
            };

            _state.NameHint.Hint = GetUpdatedHint(_state.NameHint, _state.NameHint.Hint, UpdateHint);
        }

        protected static string GetUpdatedHint(IFullHint pFullHint, string pCurrentHint, Func<int, string, string, string> pUpdateHint)
        {
            if (pFullHint.IsComplete)
            {
                return pCurrentHint;
            }

            int index = pFullHint.RevealQueue.Dequeue();
            string toReveal = pFullHint.HintElements[index];
            return pUpdateHint(index, toReveal, pCurrentHint);
        }

        protected static IEnumerable<string> GetUpdatedHint(IFullHint pFullHint, IEnumerable<string> pCurrentHint, Func<int, string, IEnumerable<string>, IEnumerable<string>> pUpdateHint)
        {
            if (pFullHint.IsComplete)
            {
                return pCurrentHint;
            }

            int index = pFullHint.RevealQueue.Dequeue();
            string toReveal = pFullHint.HintElements[index];
            return pUpdateHint(index, toReveal, pCurrentHint);
        }

        protected virtual void UpdateOtherHint(HintTypes pHintType)
        {
            // Specialized games can have their own set of hints.
            // Update those hints here.
        }

        protected virtual void RevealAllHints()
        {
            _state.NameHint.Hint = _state.NameHint.FullReveal;
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
            static void ProcessHintElement(string pHintElement, int pIndex, ICollection<string> pHintCol, List<int> pHintReveals)
            {
                if (WhitespaceOnly().Match(pHintElement) != Match.Empty)
                {
                    // Space
                    pHintCol.Add("/ ");
                }
                else if (CharNumsOnly().Match(pHintElement) != Match.Empty)
                {
                    // Letter or Number
                    pHintCol.Add("_ ");
                    pHintReveals.Add(pIndex);
                }
                else
                {
                    // Non space, non letter/number
                    pHintCol.Add(pHintElement + " ");
                }
            }

            void SetHints(Queue<int> pHintReveals, ICollection<string> pHintCol, IList<string> pHintElements)
            {

                _state.NameHint.RevealQueue = pHintReveals;
                _state.NameHint.Hint = string.Join("", pHintCol);
                _state.NameHint.HintElements = pHintElements;
            }

            string[] nameInChars = pModel.Name.Select((x) => x.ToString()).ToArray();
            SetHint(nameInChars, _state.NameHint.RevealCutOff, ProcessHintElement, SetHints, RandomizerUtils.RandomizeList);
        }

        protected virtual void CreateHints(TModel pModel)
        {
            SetNameHint(pModel);
            // Set specialized game hints here.
        }

        protected static void SetHint(IList<string> pHintElements, double pCutOff, Action<string, int, ICollection<string>, List<int>> pProcessHintElement, Action<Queue<int>, ICollection<string>, IList<string>> pSetHints, Action<List<int>>? pPostProcessReveals = null)
        {
            ICollection<string> hint = [];
            List<int> reveals = [];
            // Process the Hint Elements to create the Hints and Reveals.
            for (int i = 0; i < pHintElements.Count; i++)
            {
                pProcessHintElement(pHintElements[i], i, hint, reveals);
            }

            pPostProcessReveals?.Invoke(reveals);

            // Calculate Cut Off and apply to Reveals.
            int toCutOff = (int)Math.Round(reveals.Count * pCutOff, MidpointRounding.ToZero);
            int count = reveals.Count - toCutOff;

            // Set the Hints
            pSetHints(new(reveals[..count]), hint, pHintElements);
        }

        protected virtual void SetHintRevealQueue()
        {
            ICollection<HintReveal> queue = BuildRevealQueue();
            _state.HintRevealQueue = new(queue);
        }

        protected virtual ICollection<HintReveal> BuildRevealQueue()
        {
            ICollection<HintReveal> revealCol = [];

            List<HintTypes> nameReveals = [];
            nameReveals.AddRange(Enumerable.Repeat(HintTypes.Name, _state.NameHint.RevealQueue.Count));
            Queue<HintTypes> nameRevealsQueue = new(nameReveals);

            List<HintTypes> specialReveals = (List<HintTypes>)GetSpecialHintRevealTypes();
            if (specialReveals.Count > 0)
            {
                RandomizerUtils.RandomizeList(specialReveals);
            }
            
            Queue<HintTypes> specialRevealsQueue = new(specialReveals);

            int i = 1;
            while (nameRevealsQueue.Count > 0 && specialRevealsQueue.Count > 0)
            {
                ProcessHintReveal(i, revealCol, nameRevealsQueue, specialRevealsQueue);
                i++;
            }

            if (nameRevealsQueue.Count > 0)
            {
                while (nameRevealsQueue.Count > 0)
                {
                    ProcessAndAddHintReveal(revealCol, nameRevealsQueue, GetMilestone(i, _state.HintDifficultyMult));
                    i++;
                }
            }

            if (specialRevealsQueue.Count > 0)
            {
                while (specialRevealsQueue.Count > 0)
                {
                    ProcessAndAddHintReveal(revealCol, specialRevealsQueue, GetMilestone(i, _state.HintDifficultyMult));
                    i++;
                }
            }

            return revealCol;
        }

        protected static int GetMilestone(double pCurrent, double pMult)
        {
            return (int)(pCurrent * pMult);
        }

        protected virtual IEnumerable<HintTypes> GetSpecialHintRevealTypes()
        {
            return [];
        }

        protected virtual void ProcessHintReveal(int pCurrent, ICollection<HintReveal> pCol, Queue<HintTypes> pNameHintQueue, Queue<HintTypes> pSpecialHintQueue)
        {
            // Starting with name hints, it alternates between name hints and special hints every milestone.
            if (pCurrent % 2 == 0)
            {
                ProcessAndAddHintReveal(pCol, pSpecialHintQueue, GetMilestone(pCurrent, _state.HintDifficultyMult));
            }
            else
            {
                ProcessAndAddHintReveal(pCol, pNameHintQueue, GetMilestone(pCurrent, _state.HintDifficultyMult));
            }
        }

        protected static void ProcessAndAddHintReveal(ICollection<HintReveal> pCol, Queue<HintTypes> pQueue, int pMilestone)
        {
            HintTypes hintType = pQueue.Dequeue();
            pCol.Add(GetHintReveal(pMilestone, hintType));
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

        protected static HintReveal GetHintReveal(int pScore, HintTypes pType)
        {
            return new()
            {
                ScoreMilestone = pScore,
                HintType = pType
            };
        }

        protected virtual TState InitializeState()
        {
            return new()
            {
                HintDifficultyMult = 2.5,
                NameHint = new()
                {
                    RevealCutOff = 0.5 // Only 50% of a name is revealed.
                }
            };
        }

        protected virtual string ReplaceText(string pText, string pPattern, string? pReplacement = null)
        {
            string rep = pReplacement ?? DefaultReplacementText();
            return Regex.Replace(pText, pPattern, rep);
        }

        protected virtual string DefaultReplacementText()
        {
            return "-";
        }

        protected virtual string GetHintTypeText(HintTypes pHintType)
        {
            return pHintType switch
            {
                HintTypes.BaseItem => "Base Item",
                HintTypes.FlavourText => "Flavour Text",
                HintTypes.Name => "Name",
                HintTypes.StatMods => "Stat Text",
                HintTypes.Description => "Description",
                _ => "",
            };
        }
        #endregion
    }
}
