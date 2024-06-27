namespace PoeWikiData.Models
{
    public class LeagueModel : BaseModel
    {
        public string ReleaseVersionMajor { get; set; } = "";
        public string ReleaseVersionMinor { get; set; } = "";
        public string ReleaseVersionPatch { get; set; } = "";
    }
}
