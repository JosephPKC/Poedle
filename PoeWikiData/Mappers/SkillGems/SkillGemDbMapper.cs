using PoeWikiApi.Models;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Models.UniqueItems;

namespace PoeWikiData.Mappers.SkillGems
{
    internal static class SkillGemDbMapper
    {
        public static SkillGemDbModel Map(SkillGemWikiModel pModel)
        {
            return new()
            {
                Id = pModel.Id,
                Name = pModel.PageName, // Skill Gems have no Name property in wiki.
                DisplayName = pModel.PageName,
                GemDescription = pModel.GemDescription,
                GemTags = GetGemTags(pModel.GemTags),
                PrimaryAttribute = pModel.PrimaryAttribute,
                DexterityPercent = pModel.DexterityPercent ?? 0,
                IntelligencePercent = pModel.IntelligencePercent ?? 0,
                StrengthPercent = pModel.StrengthPercent ?? 0
            };
        }

        private static IEnumerable<StaticDataDbModel> GetGemTags(IEnumerable<string> pTags)
        {
            ICollection<StaticDataDbModel> result = [];
            foreach (string tag in pTags)
            {
                StaticDataDbModel? staticData = StaticDataMasterRef.GemTags.GetByName(tag);
                if (staticData != null)
                {
                    result.Add(staticData);
                }
            }
            return result;
        }
    }
}
