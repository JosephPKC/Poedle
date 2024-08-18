using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.Data.Results.SkillGems
{
    internal static class SkillGemsResultMapper
    {
        public static SkillGemsResultExpModel GetResult(SkillGemDbModel pGuess, SkillGemDbModel pAnswer)
        {
            return new()
            {
                Id = pGuess.Id,
                Name = pGuess.Name,
                NameResult = GuessResultUtils.CompareStrings(pGuess.Name, pAnswer.Name),
                PrimaryAttribute = pGuess.PrimaryAttribute,
                PrimaryAttributeResult = GuessResultUtils.CompareStrings(pGuess.PrimaryAttribute, pAnswer.PrimaryAttribute),
                DexterityPercent = pGuess.DexterityPercent.ToString(),
                DexterityPercentResult = GuessResultUtils.CompareNumbers(pGuess.DexterityPercent, pAnswer.DexterityPercent, 20),
                IntelligencePercent = pGuess.IntelligencePercent.ToString(),
                IntelligencePercentResult = GuessResultUtils.CompareNumbers(pGuess.IntelligencePercent, pAnswer.IntelligencePercent, 20),
                StrengthPercent = pGuess.StrengthPercent.ToString(),
                StrengthPercentResult = GuessResultUtils.CompareNumbers(pGuess.StrengthPercent, pAnswer.StrengthPercent, 20),
                GemTags = GuessResultUtils.GetModelListString(pGuess.GemTags),
                GemTagsResult = GuessResultUtils.CompareLists(pGuess.GemTags, pAnswer.GemTags),
                TagAmount = pGuess.GemTags.Count().ToString(),
                TagAmountResult = GuessResultUtils.CompareNumbers((uint)pGuess.GemTags.Count(), (uint)pAnswer.GemTags.Count(), 5)
            };
        }
    }
}
