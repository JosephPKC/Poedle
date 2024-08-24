using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.Data.Results.UniqueItems
{
    internal static class UniqueItemsResultMapper
    {
        public static UniqueItemsResultExpModel GetResult(UniqueItemDbModel pGuess, UniqueItemDbModel pAnswer)
        {
            return new()
            {
                Name = new(pGuess.Name, GetNameResult(pGuess, pAnswer)),
                ItemClass = new(pGuess.ItemClass.DisplayName, GuessResultUtils.CompareEnumValues(pGuess.ItemClass.Id, pAnswer.ItemClass.Id)),
                ReqLvl = new(pGuess.ReqLvl.ToString(), GuessResultUtils.CompareNumbers(pGuess.ReqLvl, pAnswer.ReqLvl, 20)),
                ReqDex = new(pGuess.ReqDex.ToString(), GuessResultUtils.CompareNumbers(pGuess.ReqDex, pAnswer.ReqDex, 20)),
                ReqInt = new(pGuess.ReqInt.ToString(), GuessResultUtils.CompareNumbers(pGuess.ReqInt, pAnswer.ReqInt, 20)),
                ReqStr = new(pGuess.ReqStr.ToString(), GuessResultUtils.CompareNumbers(pGuess.ReqStr, pAnswer.ReqStr, 20)),
                LeaguesIntroduced = new(GuessResultUtils.GetModelListString(pGuess.LeaguesIntroduced), GuessResultUtils.CompareLists(pGuess.LeaguesIntroduced, pAnswer.LeaguesIntroduced)),
                ItemAspects = new(GuessResultUtils.GetModelListString(pGuess.ItemAspects), GuessResultUtils.CompareLists(pGuess.ItemAspects, pAnswer.ItemAspects)),
                DropSources = new(GuessResultUtils.GetModelListString(pGuess.DropSources), GuessResultUtils.CompareLists(pGuess.DropSources, pAnswer.DropSources)),
                DropTypes = new(GuessResultUtils.GetModelListString(pGuess.DropTypes), GuessResultUtils.CompareLists(pGuess.DropTypes, pAnswer.DropTypes))
            };
        }

        private static ResultStates GetNameResult(UniqueItemDbModel pGuess, UniqueItemDbModel pAnswer)
        {
            // Check Display Name first
            ResultStates result = GuessResultUtils.CompareStrings(pGuess.DisplayName, pAnswer.DisplayName);
            if (result == ResultStates.Correct)
            {
                return ResultStates.Correct;
            }
            // Then, check Name if Display Name do not match.
            // If match, then it is a partial at best.
            result = GuessResultUtils.CompareStrings(pGuess.Name, pAnswer.Name);
            return result == ResultStates.Correct ? ResultStates.Partial : ResultStates.Wrong;
        }
    }
}
