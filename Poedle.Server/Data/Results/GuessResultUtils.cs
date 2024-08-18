using PoeWikiData.Models;

namespace Poedle.Server.Data.Results
{
    internal static class GuessResultUtils
    {
        public static ResultStates CompareEnumValues(uint pGuess, uint pActual)
        {

            return pGuess == pActual ? ResultStates.Correct : ResultStates.Wrong;
        }

        public static ResultStates CompareStrings(string pGuess, string pActual)
        {

            return pGuess == pActual ? ResultStates.Correct : ResultStates.Wrong;
        }

        public static ResultStates CompareLists<TData>(IEnumerable<TData> pGuess, IEnumerable<TData> pActual)
        {
            if (pGuess.SequenceEqual(pActual))
            {
                return ResultStates.Correct;
            }

            return pGuess.Intersect(pActual).Any() ? ResultStates.Partial : ResultStates.Wrong;
        }

        public static ResultStates CompareNumbers(uint pGuess, uint pActual, uint pThreshold)
        {
            if (pGuess == pActual)
            {
                return ResultStates.Correct;
            }

            if (Math.Abs(pGuess - pActual) <= pThreshold)
            {
                // If within threshold
                return pGuess > pActual ? ResultStates.BitHigh : ResultStates.BitLow;
            }

            return pGuess > pActual ? ResultStates.TooHigh : ResultStates.TooLow;
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
