
using System.Text.Json.Serialization;

namespace PoeWikiApi.Models
{
    public class UniqueItemWikiModel : BaseWikiModel
    {
        [JsonPropertyName("class")]
        public string ItemClass { get; set; } = string.Empty;
        [JsonPropertyName("base item")]
        public string BaseItem { get; set; } = string.Empty;
        public IEnumerable<string> Influences { get; set; } = [];
        [JsonPropertyName("flavour text")]
        public string FlavourText { get; set; } = string.Empty;
        [JsonPropertyName("drop monsters")]
        public IEnumerable<string> DropMonsters { get; set; } = [];
        [JsonPropertyName("drop text")]
        public string DropText { get; set; } = string.Empty;
        [JsonPropertyName("release version")]
        public string ReleaseVersion { get; set; } = string.Empty;
        [JsonPropertyName("implicit stat text")]
        public string ImplicitStatText { get; set; } = string.Empty;
        [JsonPropertyName("explicit stat text")]
        public string ExplicitStatText { get; set; } = string.Empty;

        [JsonPropertyName("required level")]
        public uint ReqLvl { get; set; }
        [JsonPropertyName("required dexterity")]
        public uint ReqDex { get; set; }
        [JsonPropertyName("required intelligence")]
        public uint ReqInt { get; set; }
        [JsonPropertyName("required strength")]
        public uint ReqStr { get; set; }

        [JsonPropertyName("is corrupted")]
        public bool IsCorrupted { get; set; }
        [JsonPropertyName("is fractured")]
        public bool IsFractured { get; set; }
        [JsonPropertyName("is replica")]
        public bool IsReplica { get; set; }
        [JsonPropertyName("is synthesised")]
        public bool IsSynthesised { get; set; }
        [JsonPropertyName("is eater of worlds item")]
        public bool IsEaterOfWorlds { get; set; }
        [JsonPropertyName("is searing exarch item")]
        public bool IsSearingExarch { get; set; }
        [JsonPropertyName("is veiled")]
        public bool IsVeiled { get; set; }
    }
}
