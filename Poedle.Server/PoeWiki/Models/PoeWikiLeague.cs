using System.Text.Json.Serialization;

namespace Poedle.PoeWiki.Models
{
    public class PoeWikiLeague : BasePoeWikiModel
    {
        [JsonPropertyName("release version")]
        public string ReleaseVersion { get; set; } = "";
    }
}
