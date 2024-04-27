using System.Text.Json.Serialization;

namespace PoeWikiApi.Models
{
    public class PoeWikiPassive : BasePoeWikiModel
    {
        [JsonPropertyName("_ID")]
        public uint Id { get; set; }
        // Prefix helps us determine what KIND of notable or keystone it is:
        //  affliction_ -> cluster jewel
        //  blight_ -> blight-specific anoint
        //  atlas_ -> atlas tree
        //  eternal_/vaal_/maraketh_/karui_/templar_ -> timeless
        [JsonPropertyName("id")]
        public string NameId { get; set; } = "";
        public string Name { get; set; } = "";
        [JsonPropertyName("flavour text")]
        public string FlavourText { get; set; } = "";
        [JsonPropertyName("ascendancy class")]
        public string AscendancyClass { get; set; } = "";

        [JsonPropertyName("is notable")]
        public bool IsNotable { get; set; }
        [JsonPropertyName("is keystone")]
        public bool IsKeystone { get; set; }
        [JsonPropertyName("is jewel socket")]
        public bool IsJewelSocket { get; set; }

        [JsonPropertyName("stat text")]
        public string StatText { get; set; } = "";
    }
}
