using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints;
using Poedle.Server.Data.Hints.Shared;
using Poedle.Server.Data.Hints.UniqueItems;
using Poedle.Server.Data.Results.UniqueItems;
using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.States.UniqueItems
{
    internal class UniqueItemsStateController(UniqueItemDbLookUp pLookUp, Dictionary<uint, AnswerExpModel> pAllAnswers) : BaseStateController<UniqueItemsResultExpModel, UniqueItemDbModel, UniqueItemsState, UniqueItemAllHintsExpModel>(pLookUp, pAllAnswers)
    {
        public override UniqueItemAllHintsExpModel GetHints()
        {
            UniqueItemAllHintsExpModel hints = base.GetHints();
            hints.BaseItemHint = HintMapper.Map(_state.BaseItemHint);
            hints.FlavourHint = HintMapper.Map(_state.FlavourTextHint);
            hints.StatModHint = HintMapper.Map(_state.StatModHint);

            return hints;
        }

        public override UniqueItemsResultExpModel ProcessGuess(uint pGuessId)
        {
            return ProcessGuess(pGuessId, UniqueItemsResultMapper.GetResult);
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
            _state.BaseItemHint.Hint = _state.BaseItemHint.FullReveal;
            _state.FlavourTextHint.Hint = _state.FlavourTextHint.FullReveal;
            _state.StatModHint.Hint = _state.StatModHint.FullReveal;
        }

        protected override void CreateHints(UniqueItemDbModel pModel)
        {
            base.CreateHints(pModel);
            SetBaseItemHint(pModel);
            SetStatModHint(pModel);
            SetFlavourTextHint(pModel);
        }

        protected override IEnumerable<HintTypes> GetSpecialHintRevealTypes()
        {
            List<HintTypes> specialReveals = [];

            specialReveals.AddRange(Enumerable.Repeat(HintTypes.BaseItem, _state.BaseItemHint.RevealQueue.Count));
            specialReveals.AddRange(Enumerable.Repeat(HintTypes.FlavourText, _state.FlavourTextHint.RevealQueue.Count));
            specialReveals.AddRange(Enumerable.Repeat(HintTypes.StatMods, _state.StatModHint.RevealQueue.Count));

            return specialReveals;
        }

        #region "BaseItemHint"
        private void UpdateBaseItemHint()
        {
            static string UpdateHint(int pIndex, string pToReveal, string pCurrentHint)
            {
                string[] hintSplit = pCurrentHint.Split(" ");
                hintSplit[pIndex] = pToReveal;
                return string.Join(" ", hintSplit);
            };

            _state.BaseItemHint.Hint = GetUpdatedHint(_state.BaseItemHint, _state.BaseItemHint.Hint, UpdateHint);
        }

        private void SetBaseItemHint(UniqueItemDbModel pModel)
        {
            void ProcessHintElement(string pHintElement, int pIndex, ICollection<string> pHintCol, List<int> pHintReveals)
            {
                pHintCol.Add(ReplaceText(pHintElement, @"[\w\W]"));
                pHintReveals.Add(pIndex);
            }

            void SetHints(Queue<int> pHintReveals, ICollection<string> pHintCol, IList<string> pHintElements)
            {
                _state.BaseItemHint.RevealQueue = pHintReveals;
                _state.BaseItemHint.Hint = string.Join(" ", pHintCol);
                _state.BaseItemHint.HintElements = pHintElements;
            }

            string[] baseItemSplit = pModel.BaseItem.Split(" ");
            SetHint(baseItemSplit, _state.BaseItemHint.RevealCutOff, ProcessHintElement, SetHints);
        }
        #endregion

        #region "FlavourTextHint"
        private void UpdateFlavourTextHint()
        {
            static IEnumerable<string> UpdateHint(int pIndex, string pToReveal, IEnumerable<string> pCurrentHint)
            {
                List<string> textList = pCurrentHint.ToList();
                textList[pIndex] = pToReveal;
                return textList;
            };

            _state.FlavourTextHint.Hint = GetUpdatedHint(_state.FlavourTextHint, _state.FlavourTextHint.Hint, UpdateHint);
        }

        private void SetFlavourTextHint(UniqueItemDbModel pModel)
        {
            void ProcessHintElement(string pHintElement, int pIndex, ICollection<string> pHintCol, List<int> pHintReveals)
            {
                pHintCol.Add(ReplaceText(pHintElement, @"[\S]"));
                pHintReveals.Add(pIndex);
            }

            void SetHints(Queue<int> pHintReveals, ICollection<string> pHintCol, IList<string> pHintElements)
            {
                _state.FlavourTextHint.RevealQueue = pHintReveals;
                _state.FlavourTextHint.Hint = pHintCol;
                _state.FlavourTextHint.HintElements = pHintElements;
            }

            IList<string> flavourTextList = pModel.FlavourText.ToList();
            SetHint(flavourTextList, _state.FlavourTextHint.RevealCutOff, ProcessHintElement, SetHints);
        }
        #endregion

        #region "StatModHint"
        private void UpdateStatModHint()
        {
            static IEnumerable<string> UpdateHint(int pIndex, string pToReveal, IEnumerable<string> pCurrentHint)
            {
                List<string> textList = pCurrentHint.ToList();
                textList[pIndex] = pToReveal;
                return textList;
            };

            _state.StatModHint.Hint = GetUpdatedHint(_state.StatModHint, _state.StatModHint.Hint, UpdateHint);
        }

        private void SetStatModHint(UniqueItemDbModel pModel)
        {
            List<string> statMods = [];
            if (pModel.ImplicitStatText != null && pModel.ImplicitStatText.Any())
            {
                statMods.AddRange(pModel.ImplicitStatText);
                _state.StatModHint.NbrImplicits = (uint)pModel.ImplicitStatText.Count();
            }

            if (pModel.ExplicitStatText != null && pModel.ExplicitStatText.Any())
            {
                statMods.AddRange(pModel.ExplicitStatText);
            }

            void ProcessHintElement(string pHintElement, int pIndex, ICollection<string> pHintCol, List<int> pHintReveals)
            {
                //pHintCol.Add(ReplaceText(pHintElement, @"[\S]"));
                pHintCol.Add("_");
                pHintReveals.Add(pIndex);
            }

            void SetHints(Queue<int> pHintReveals, ICollection<string> pHintCol, IList<string> pHintElements)
            {
                _state.StatModHint.RevealQueue = pHintReveals;
                _state.StatModHint.Hint = pHintCol;
                _state.StatModHint.HintElements = pHintElements;
            }

            SetHint(statMods, _state.StatModHint.RevealCutOff, ProcessHintElement, SetHints);
        }
        #endregion
    }
}
