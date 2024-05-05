using System.Text.Json.Serialization;

namespace Poedle.PoeWiki.Models
{
    public class PoeWikiUnique : BasePoeWikiModel
    {
        [JsonPropertyName("class")]
        public string ItemClass { get; set; } = "";
        [JsonPropertyName("base item")]
        public string BaseItem { get; set; } = "";
        public List<string> Influences { get; set; } = [];
        [JsonPropertyName("flavour text")]
        public string FlavourText { get; set; } = "";
        [JsonPropertyName("drop monsters")]
        public List<string> DropMonsters { get; set; } = [];
        [JsonPropertyName("drop text")]
        public string DropText { get; set; } = "";
        [JsonPropertyName("release version")]
        public string ReleaseVersion { get; set; } = "";
        [JsonPropertyName("stat text")]
        public string StatText { get; set; } = "";

        [JsonPropertyName("required level")]
        public ushort ReqLvl { get; set; }
        [JsonPropertyName("required dexterity")]
        public ushort ReqDex { get; set; }
        [JsonPropertyName("required intelligence")]
        public ushort ReqInt { get; set; }
        [JsonPropertyName("required strength")]
        public ushort ReqStr { get; set; }

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
