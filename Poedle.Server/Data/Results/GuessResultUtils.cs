using PoeWikiData.Models;

namespace Poedle.Server.Data.Results
{
    internal static class GuessResultUtils
    {
        public static ResultStates CompareEnumValues(uint pGuess, uint pActual)
        {

            return pGuess == pActual ? ResultStates.CORRECT : ResultStates.WRONG;
        }

        public static ResultStates CompareStrings(string pGuess, string pActual)
        {

            return pGuess == pActual ? ResultStates.CORRECT : ResultStates.WRONG;
        }

        public static ResultStates CompareLists<TData>(IEnumerable<TData> pGuess, IEnumerable<TData> pActual)
        {
            if (pGuess.SequenceEqual(pActual)) return ResultStates.CORRECT;

            return pGuess.Intersect(pActual).Any() ? ResultStates.PARTIAL : ResultStates.WRONG;
        }

        public static ResultStates CompareNumbers(uint pGuess, uint pActual, uint pThreshold)
        {
            if (pGuess == pActual) return ResultStates.CORRECT;

            return Math.Abs(pGuess - pActual) <= pThreshold ? ResultStates.PARTIAL : ResultStates.WRONG;
        }

        public static string GetModelListString<TModel>(IEnumerable<TModel> pModels) where TModel : BaseNamedDbModel
        {
            if (!pModels.Any())
            {
                return "None";
            }
            return string.Join(",", pModels.Select((x) => x.DisplayName));
        }
    }
}
