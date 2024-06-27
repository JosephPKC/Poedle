using System.Text.Json.Serialization;

namespace PoeWikiApi.Models
{
    public class LeagueModel : BaseModel
    {
        [JsonPropertyName("release version")]
        public string ReleaseVersion { get; set; } = "";
    }
}
