using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Results.UniqueByAttr;
using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.States
{
    internal class UniqueByAttrStateController(UniqueItemDbLookUp pLookUp, Dictionary<uint, AnswerExpModel> pAllAnswers) : BaseStateController<UniqueByAttrResultExpModel, UniqueItemDbModel>(pLookUp, pAllAnswers)
    {
        public override UniqueByAttrResultExpModel ProcessGuess(uint pGuessId)
        {
            static UniqueByAttrResultExpModel GetGuess(UniqueItemDbModel pGuess, UniqueItemDbModel pAnswer) => UniqueByAttrResultMapper.GetResult(pGuess, pAnswer);
            return ProcessGuess(pGuessId, GetGuess);
        }

        protected override void SetDisplayHints()
        {
            // No display hints for this game
            _state.DisplayHints = [];
        }
    }
}
