using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints;
using Poedle.Server.Data.Hints.ExpHints;
using Poedle.Server.Data.Results.UniqueItems;
using PoeWikiData.Models.UniqueItems;
using System.Text.RegularExpressions;

namespace Poedle.Server.States.UniqueItems
{
    internal class UniqueItemsStateController(UniqueItemDbLookUp pLookUp, Dictionary<uint, AnswerExpModel> pAllAnswers) : BaseStateController<UniqueItemsResultExpModel, UniqueItemDbModel, UniqueItemsState>(pLookUp, pAllAnswers)
    {
        public override AllHintsExpModel GetHints()
        {
            AllHintsExpModel hints = base.GetHints();
            hints.BaseItemHint = _state.BaseItemHint;
            hints.FlavourHint = _state.FlavourTextHint;
            hints.StatModHint = _state.StatModHint;

            return hints;
        }

        public override UniqueItemsResultExpModel ProcessGuess(uint pGuessId)
        {
            static UniqueItemsResultExpModel GetGuess(UniqueItemDbModel pGuess, UniqueItemDbModel pAnswer) => UniqueItemsResultMapper.GetResult(pGuess, pAnswer);
            return ProcessGuess(pGuessId, GetGuess);
        }

        protected override UniqueItemsState InitializeState()
        {
            UniqueItemsState state = base.InitializeState();
            // 4, 8, 12, 16, ...
            state.BaseItemHintScoreMilestone = new()
            {
                StartingScore = 4,
                ScoreStep = 4,
                Exceptions = [8]
            };
            // 2, 6, 10, 14, ...
            state.StatModHintScoreMilestone = new()
            {
                StartingScore = 2,
                ScoreStep = 4
            };
            // 8
            state.FlavourTextHintScoreMilestone = new()
            {
                StartingScore = 8,
                ScoreStep = 0,
                Count = 1
            };

            return state;
        }

        protected override void UpdateOtherHint(HintTypes pHintType)
        {
            base.UpdateOtherHint(pHintType);

            switch(pHintType)
            {
                case HintTypes.BaseItem:
                    UpdateBaseItemHint();
                    break;
                case HintTypes.FlavourText:
                    UpdateFlavourTextHint();
                    break;
                case HintTypes.StatMods:
                    UpdateStatModHint();
                    break;
            }
        }

        protected override void RevealAllHints()
        {
            base.RevealAllHints();
            _state.BaseItemHint.Hint = _state.BaseItemHint.FullyRevealedHint;
            _state.FlavourTextHint.Hint = _state.FlavourTextHint.FullyRevealedHint;
            _state.StatModHint.Hint = _state.StatModHint.FullyRevealedHint;
        }

        protected override void CreateHints(UniqueItemDbModel pModel)
        {
            base.CreateHints(pModel);
            SetBaseItemHint(pModel);
            SetStatModHint(pModel);
            SetFlavourTextHint(pModel);
        }

        protected override void AddHintsToRevealQueue(ICollection<HintReveal> pCol)
        {
            base.AddHintsToRevealQueue(pCol);
            AddHintsToCollection(pCol, _state.BaseItemHintScoreMilestone, HintTypes.BaseItem);
            AddHintsToCollection(pCol, _state.StatModHintScoreMilestone, HintTypes.StatMods);
            AddHintsToCollection(pCol, _state.FlavourTextHintScoreMilestone, HintTypes.FlavourText);
        }


        #region "BaseItemHint"
        private void UpdateBaseItemHint()
        {
            if (_state.BaseItemHint.RevealQueue.Count == 0)
            {
                // No more words to reveal
                return;
            }

            int index = _state.BaseItemHint.RevealQueue.Dequeue();
            string toReveal = _state.BaseItemHint.HintElements[index];
            string[] baseItemSplit = _state.BaseItemHint.Hint.Split(" ");
            baseItemSplit[index] = toReveal;
            _state.BaseItemHint.Hint = string.Join(" ", baseItemSplit);
        }

        private void SetBaseItemHint(UniqueItemDbModel pModel)
        {
            // This is a word-based hangman text
            // The base item is split into words.
            // Replace each word with ___.
            string[] baseItemSplit = pModel.BaseItem.Split(" ");
            ICollection<string> hint = [];
            List<int> reveals = [];
            for (int i = 0; i < baseItemSplit.Length; i++)
            {
                string baseItem = baseItemSplit[i];
                hint.Add(Regex.Replace(baseItem, @"[a-zA-Z0-9]", "_"));
                reveals.Add(i);
            }

            int toCutOff = (int)Math.Round(reveals.Count * _state.NameHint.RevealCutOff, MidpointRounding.ToZero);
            int count = reveals.Count - toCutOff;

            _state.BaseItemHint.RevealQueue = new(reveals[..count]);
            _state.BaseItemHint.Hint = string.Join(" ", hint);
            _state.BaseItemHint.FullyRevealedHint = pModel.BaseItem;
            _state.BaseItemHint.HintElements = baseItemSplit;
            _state.BaseItemHintScoreMilestone.Count = _state.BaseItemHint.RevealQueue.Count;
        }
        #endregion

        #region "FlavourTextHint"
        private void UpdateFlavourTextHint()
        {
            if (_state.FlavourTextHint.IsComplete)
            {
                // Already done
                return;
            }

            _state.FlavourTextHint.Hint = _state.FlavourTextHint.HintElement;
            _state.FlavourTextHint.IsComplete = true;
        }

        private void SetFlavourTextHint(UniqueItemDbModel pModel)
        {
            // This is a single-line text
            // It is ___.
            IEnumerable<string> flavourTexts = pModel.FlavourText;
            string flavourTextJoined = string.Join(" ", flavourTexts);
            string hint = Regex.Replace(flavourTextJoined, @"[\w\W]", "_");

            _state.FlavourTextHint.IsComplete = false;
            _state.FlavourTextHint.Hint = hint;
            _state.FlavourTextHint.FullyRevealedHint = flavourTextJoined;
            _state.FlavourTextHint.HintElement = flavourTextJoined;
            _state.FlavourTextHintScoreMilestone.Count = 1;
        }
        #endregion

        #region "StatModHint"
        private void UpdateStatModHint()
        {
            if (!_state.StatModHint.Hint.Any())
            {
                // No more stats to reveal
                return;
            }

            int index = _state.StatModHint.RevealQueue.Dequeue();
            string toReveal = _state.StatModHint.HintElements[index];
            List<string> statModList = _state.StatModHint.Hint.ToList();
            statModList[index] = toReveal;
            _state.StatModHint.Hint = statModList;
        }

        private void SetStatModHint(UniqueItemDbModel pModel)
        {
            // This is a multi-line text
            // Each line is a ___.
            List<string> statMods = [];
            if (pModel.ImplicitStatText != null && pModel.ImplicitStatText.Any())
            {
                statMods.AddRange(pModel.ImplicitStatText);
            }

            if (pModel.ExplicitStatText != null && pModel.ExplicitStatText.Any())
            {
                statMods.AddRange(pModel.ExplicitStatText);
            }

            ICollection<string> hint = [];
            List<int> reveals = [];
            for (int i = 0; i < statMods.Count; i++)
            {
                string statMod = statMods[i];
                hint.Add(Regex.Replace(statMod, @"[\w\W]", "_"));
                reveals.Add(i);
            }

            _state.StatModHint.RevealQueue = new(reveals);
            _state.StatModHint.Hint = hint;
            _state.StatModHint.FullyRevealedHint = statMods;
            _state.StatModHint.HintElements = statMods;
            _state.StatModHintScoreMilestone.Count = _state.StatModHint.RevealQueue.Count;
        }
        #endregion
    }
}
