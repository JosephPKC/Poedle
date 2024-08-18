using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints;
using Poedle.Server.Data.Hints.Shared;
using Poedle.Server.Data.Hints.SkillGems;
using Poedle.Server.Data.Results.SkillGems;
using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.States.SkillGems
{
    internal class SkillGemsStateController(SkillGemDbLookup pLookUp, Dictionary<uint, AnswerExpModel> pAllAnswers) : BaseStateController<SkillGemsResultExpModel, SkillGemDbModel, SkillGemsState, SkillGemAllHintsExpModel>(pLookUp, pAllAnswers)
    {
        public override SkillGemAllHintsExpModel GetHints()
        {
            SkillGemAllHintsExpModel hints = base.GetHints();
            hints.DescriptionHint = HintMapper.Map(_state.DescriptionHint);

            return hints;
        }

        public override SkillGemsResultExpModel ProcessGuess(uint pGuessId)
        {
            return ProcessGuess(pGuessId, SkillGemsResultMapper.GetResult);
        }

        protected override void UpdateOtherHint(HintTypes pHintType)
        {
            base.UpdateOtherHint(pHintType);

            switch (pHintType)
            {
                case HintTypes.Description:
                    UpdateDescriptionHint();
                    break;
            }
        }

        protected override void RevealAllHints()
        {
            base.RevealAllHints();
            _state.DescriptionHint.Hint = _state.DescriptionHint.FullReveal;
        }

        protected override void CreateHints(SkillGemDbModel pModel)
        {
            base.CreateHints(pModel);
            SetDescriptionHint(pModel);
        }

        protected override IEnumerable<HintTypes> GetSpecialHintRevealTypes()
        {
            List<HintTypes> specialReveals = [];

            specialReveals.AddRange(Enumerable.Repeat(HintTypes.Description, _state.DescriptionHint.RevealQueue.Count));

            return specialReveals;
        }
        protected override void ProcessHintReveal(int pCurrent, ICollection<HintReveal> pCol, Queue<HintTypes> pNameHintQueue, Queue<HintTypes> pSpecialHintQueue)
        {
            // Keep doing name reveals until only 3 are left
            // There is only one special reveal
            if (pNameHintQueue.Count > 3)
            {
                ProcessAndAddHintReveal(pCol, pNameHintQueue, GetMilestone(pCurrent, _state.HintDifficultyMult));
            }
            else
            {
                ProcessAndAddHintReveal(pCol, pSpecialHintQueue, GetMilestone(pCurrent, _state.HintDifficultyMult));
            }
        }

        protected override SkillGemsState InitializeState()
        {
            SkillGemsState state = base.InitializeState();
            state.HintDifficultyMult = 2;
            return state;
        }

        #region "BaseItemHint"
        private void UpdateDescriptionHint()
        {
            static string UpdateHint(int pIndex, string pToReveal, string pCurrentHint) => pToReveal;

            _state.DescriptionHint.Hint = GetUpdatedHint(_state.DescriptionHint, _state.DescriptionHint.Hint, UpdateHint);
        }

        private void SetDescriptionHint(SkillGemDbModel pModel)
        {
            void ProcessHintElement(string pHintElement, int pIndex, ICollection<string> pHintCol, List<int> pHintReveals)
            {
                pHintCol.Add(ReplaceText(pHintElement, @"[\w\W]"));
                pHintReveals.Add(pIndex);
            }

            void SetHints(Queue<int> pHintReveals, ICollection<string> pHintCol, IList<string> pHintElements)
            {
                _state.DescriptionHint.RevealQueue = pHintReveals;
                _state.DescriptionHint.Hint = string.Join(" ", pHintCol);
                _state.DescriptionHint.HintElements = pHintElements;
            }

            SetHint([pModel.GemDescription], _state.DescriptionHint.RevealCutOff, ProcessHintElement, SetHints);
        }
        #endregion
    }
}