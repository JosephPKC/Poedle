using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.Data.Results.UniqueItems
{
    internal static class UniqueItemsResultMapper
    {
        public static UniqueItemsResultExpModel GetResult(UniqueItemDbModel pGuess, UniqueItemDbModel pAnswer)
        {
            return new()
            {
                Id = pGuess.Id,
                Name = pGuess.Name,
                NameResult = GetNameResult(pGuess, pAnswer),
                ItemClass = pGuess.ItemClass.DisplayName,
                ItemClassResult = GuessResultUtils.CompareEnumValues(pGuess.ItemClass.Id, pAnswer.ItemClass.Id),
                ReqLvl = pGuess.ReqLvl.ToString(),
                ReqLvlResult = GuessResultUtils.CompareNumbers(pGuess.ReqLvl, pAnswer.ReqLvl, 20),
                ReqDex = pGuess.ReqDex.ToString(),
                ReqDexResult = GuessResultUtils.CompareNumbers(pGuess.ReqDex, pAnswer.ReqDex, 20),
                ReqInt = pGuess.ReqInt.ToString(),
                ReqIntResult = GuessResultUtils.CompareNumbers(pGuess.ReqInt, pAnswer.ReqInt, 20),
                ReqStr = pGuess.ReqStr.ToString(),
                ReqStrResult = GuessResultUtils.CompareNumbers(pGuess.ReqStr, pAnswer.ReqStr, 20),
                LeaguesIntroduced = GuessResultUtils.GetModelListString(pGuess.LeaguesIntroduced),
                LeaguesIntroducedResult = GuessResultUtils.CompareLists(pGuess.LeaguesIntroduced, pAnswer.LeaguesIntroduced),
                ItemAspects = GuessResultUtils.GetModelListString(pGuess.ItemAspects),
                ItemAspectsResult = GuessResultUtils.CompareLists(pGuess.ItemAspects, pAnswer.ItemAspects),
                DropSources = GuessResultUtils.GetModelListString(pGuess.DropSources),
                DropSourcesResult = GuessResultUtils.CompareLists(pGuess.DropSources, pAnswer.DropSources),
                DropTypes = GuessResultUtils.GetModelListString(pGuess.DropTypes),
                DropTypesResult = GuessResultUtils.CompareLists(pGuess.DropTypes, pAnswer.DropTypes)
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
