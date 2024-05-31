using Poedle.Game.Models.Params;
using Poedle.Utils.Strings;

using static Poedle.Enums.MiscEnums;

namespace Poedle.Game.Utils
{
    public static class ParamsChecker
    {
        public static ParamsResult CheckTypes(UniqueParams pGuess, UniqueParams pAnswer)
        {
            if (MiscStringUtils.EqualsIgnoreCase(pGuess.BaseItem, pAnswer.BaseItem))
            {
                return ParamsResult.CORRECT;
            }

            if (pGuess.ItemClass == pAnswer.ItemClass)
            {
                return ParamsResult.PARTIAL;
            }
            return ParamsResult.WRONG;
        }

        public static ParamsResult CheckListParam<T>(List<T> pGuessList, List<T> pAnswerList)
        {

            if (pGuessList.SequenceEqual(pAnswerList))
            {
                return ParamsResult.CORRECT;
            }

            return pGuessList.Intersect(pAnswerList).Any() ? ParamsResult.PARTIAL : ParamsResult.WRONG;
        }

        public static ParamsResult CheckNumberParam(uint pGuessNum, uint pAnswerNum)
        {
            if (pGuessNum == pAnswerNum)
            {
                return ParamsResult.CORRECT;
            }

            return pGuessNum > pAnswerNum ? ParamsResult.LOWER : ParamsResult.HIGHER;
        }
    }
}
