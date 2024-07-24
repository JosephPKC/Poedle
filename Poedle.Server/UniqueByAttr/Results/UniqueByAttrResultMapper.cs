using Poedle.Server.Shared.Results;
using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.UniqueByAttr.Results
{
    public static class UniqueByAttrResultMapper
    {
        public static UniqueByAttrResult GetGuessResult(UniqueItemDbModel pGuess, UniqueItemDbModel pAnswer)
        {
            return new()
            {
                Id = pGuess.Id,
                Name = pGuess.Name,
                NameResult = GuessUtils.CompareStrings(pGuess.DisplayName, pAnswer.DisplayName),
                ItemClass = pGuess.ItemClass.DisplayName,
                ItemClassResult = GuessUtils.CompareEnumValues(pGuess.ItemClass.Id, pAnswer.ItemClass.Id),
                BaseItem = pGuess.BaseItem,
                BaseItemResult = GuessUtils.CompareStrings(pGuess.BaseItem, pAnswer.BaseItem),
                ReqLvl = pGuess.ReqLvl.ToString(),
                ReqLvlResult = GuessUtils.CompareNumbers(pGuess.ReqLvl, pAnswer.ReqLvl, 20),
                ReqDex = pGuess.ReqDex.ToString(),
                ReqDexResult = GuessUtils.CompareNumbers(pGuess.ReqDex, pAnswer.ReqDex, 20),
                ReqInt = pGuess.ReqInt.ToString(),
                ReqIntResult = GuessUtils.CompareNumbers(pGuess.ReqInt, pAnswer.ReqInt, 20),
                ReqStr = pGuess.ReqStr.ToString(),
                ReqStrResult = GuessUtils.CompareNumbers(pGuess.ReqStr, pAnswer.ReqStr, 20),
                LeaguesIntroduced = GuessUtils.GetModelListString(pGuess.LeaguesIntroduced),
                LeaguesIntroducedResult = GuessUtils.CompareLists(pGuess.LeaguesIntroduced, pAnswer.LeaguesIntroduced),
                ItemAspects = GuessUtils.GetModelListString(pGuess.ItemAspects),
                ItemAspectsResult = GuessUtils.CompareLists(pGuess.ItemAspects, pAnswer.ItemAspects),
                DropSources = GuessUtils.GetModelListString(pGuess.DropSources),
                DropSourcesResult = GuessUtils.CompareLists(pGuess.DropSources, pAnswer.DropSources),
                DropTypes = GuessUtils.GetModelListString(pGuess.DropTypes),
                DropTypesResult = GuessUtils.CompareLists(pGuess.DropTypes, pAnswer.DropTypes)
            };
        }
    }
}
