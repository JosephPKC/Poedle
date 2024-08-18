using System.Text.Json.Serialization;

namespace PoeWikiApi.Models
{
    public class SkillGemWikiModel : BaseWikiModel
    {
        [JsonPropertyName("gem description")]
        public string GemDescription { get; set; } = string.Empty;
        [JsonPropertyName("gem tags")]
        public IEnumerable<string> GemTags { get; set; } = [];
        [JsonPropertyName("primary attribute")]
        public string PrimaryAttribute { get; set; } = string.Empty;
        [JsonPropertyName("dexterity percent")]
        public uint? DexterityPercent { get; set; }
        [JsonPropertyName("intelligence percent")]
        public uint? IntelligencePercent { get; set; }
        [JsonPropertyName("strength percent")]
        public uint? StrengthPercent { get; set; }
    }
}
