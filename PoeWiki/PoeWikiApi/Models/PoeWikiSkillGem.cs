using System.Text.Json.Serialization;

namespace PoeWikiApi.Models
{
    public class PoeWikiSkillGem : BasePoeWikiModel
    {
        [JsonPropertyName("_ID")]
        public uint Id { get; set; }
        [JsonPropertyName("_pageName")]
        public string PageName { get; set; } = "";
        [JsonPropertyName("gem description")]
        public string GemDescription { get; set; } = "";

        [JsonPropertyName("primary attribute")]
        public string PrimaryAttribute { get; set; } = "";
        [JsonPropertyName("dexterity percent")]
        public ushort DexPercent { get; set; }
        [JsonPropertyName("intelligence percent")]
        public ushort IntPercent { get; set; }
        [JsonPropertyName("strength percent")]
        public ushort StrPercent { get; set; }

        [JsonPropertyName("is awakened support gem")]
        public bool IsAwakened { get; set; }
        [JsonPropertyName("is vaal skill gem")]
        public bool IsVaal { get; set; }

        [JsonPropertyName("gem tags")]
        public List<string> GemTags { get; set; } = [];
    }
}
