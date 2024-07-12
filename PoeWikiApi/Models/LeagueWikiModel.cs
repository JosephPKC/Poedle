using System.Text.Json.Serialization;

namespace PoeWikiApi.Models
{
    public class LeagueWikiModel : BaseWikiModel
    {
        [JsonPropertyName("release version")]
        public string ReleaseVersion { get; set; } = string.Empty;
    }
}
