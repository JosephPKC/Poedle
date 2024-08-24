using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server.Data.Results.SkillGems
{
    internal static class SkillGemsResultMapper
    {
        public static SkillGemsResultExpModel GetResult(SkillGemDbModel pGuess, SkillGemDbModel pAnswer)
        {
            return new()
            {
                Name = new(pGuess.Name, GuessResultUtils.CompareStrings(pGuess.Name, pAnswer.Name)),
                PrimaryAttribute = new(pGuess.PrimaryAttribute, GuessResultUtils.CompareStrings(pGuess.PrimaryAttribute, pAnswer.PrimaryAttribute)),
                DexterityPercent = new(pGuess.DexterityPercent.ToString(), GuessResultUtils.CompareNumbers(pGuess.DexterityPercent, pAnswer.DexterityPercent, 20)),
                IntelligencePercent = new(pGuess.IntelligencePercent.ToString(), GuessResultUtils.CompareNumbers(pGuess.IntelligencePercent, pAnswer.IntelligencePercent, 20)),
                StrengthPercent = new(pGuess.StrengthPercent.ToString(), GuessResultUtils.CompareNumbers(pGuess.StrengthPercent, pAnswer.StrengthPercent, 20)),
                GemTags = new(GuessResultUtils.GetModelListString(pGuess.GemTags), GuessResultUtils.CompareLists(pGuess.GemTags, pAnswer.GemTags)),
                TagAmount = new(pGuess.GemTags.Count().ToString(), GuessResultUtils.CompareNumbers((uint)pGuess.GemTags.Count(), (uint)pAnswer.GemTags.Count(), 5))
            };
        }
    }
}
