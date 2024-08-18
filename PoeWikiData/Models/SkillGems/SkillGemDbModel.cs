using PoeWikiData.Models.StaticData;

namespace PoeWikiData.Models.UniqueItems
{
    public class SkillGemDbModel : BaseNamedDbModel
    {
        public string GemDescription { get; set; } = string.Empty;
        public IEnumerable<StaticDataDbModel> GemTags { get; set; } = [];
        public string PrimaryAttribute { get; set; } = string.Empty;
        public uint DexterityPercent { get; set; }
        public uint IntelligencePercent { get; set; }
        public uint StrengthPercent { get; set; }
    }
}
