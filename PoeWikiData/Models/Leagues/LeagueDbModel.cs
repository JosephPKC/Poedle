namespace PoeWikiData.Models.Leagues
{
    public class LeagueDbModel : BaseDbModel
    {
        public string ReleaseVersionMajor { get; set; } = "";
        public string ReleaseVersionMinor { get; set; } = "";
        public string ReleaseVersionPatch { get; set; } = "";

        public string ReleaseVersion
        {
            get => $"{ReleaseVersionMajor}.{ReleaseVersionMinor}.{ReleaseVersionPatch}";
        }
    }
}
