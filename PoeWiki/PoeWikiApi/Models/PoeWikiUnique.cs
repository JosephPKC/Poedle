using System.Text.Json.Serialization;

namespace PoeWikiApi.Models
{
    public class PoeWikiUnique : BasePoeWikiModel
    {
        [JsonPropertyName("_ID")]
        public uint Id { get; set; }
        public string Name { get; set; } = "";
        // https://www.poewiki.net/wiki/Item class
        [JsonPropertyName("class")]
        public string Class { get; set; } = "";
        [JsonPropertyName("base item")]
        public string BaseItem { get; set; } = "";

        public List<string> Influences { get; set; } = [];
        [JsonPropertyName("flavourtext")]
        public string FlavourText { get; set; } = "";

        [JsonPropertyName("drop monsters")]
        public List<string> DropMonsters { get; set; } = [];
        [JsonPropertyName("acquisition tags")]
        public List<string> AquisitionTags { get; set; } = [];

        [JsonPropertyName("release version")]
        public string ReleaseVersion { get; set; } = "";
        [JsonPropertyName("removal version")]
        public string RemovalVersion { get; set; } = "";

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
        [JsonPropertyName("is eater of worlds item")]
        public bool IsEaterOfWorlds { get; set; }
        [JsonPropertyName("is searing exarch item")]
        public bool IsSearingExarch { get; set; }
        [JsonPropertyName("is fractured")]
        public bool IsFractured { get; set; }
        [JsonPropertyName("is synthesized")]
        public bool IsSynthesized { get; set; }
        [JsonPropertyName("is replica")]
        public bool IsReplica { get; set; }
        [JsonPropertyName("is unmodifiable")]
        public bool IsUnmodifiable { get; set; }
        [JsonPropertyName("is veiled")]
        public bool IsVeiled { get; set; }

        [JsonPropertyName("stat text")]
        public string StatText { get; set; } = "";
    }
}
