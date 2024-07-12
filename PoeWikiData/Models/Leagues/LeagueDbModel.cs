namespace PoeWikiData.Models.Leagues
{
    public class LeagueDbModel : BaseNamedDbModel
    {
        public uint ReleaseVersionMajor { get; set; } = 0;
        public uint ReleaseVersionMinor { get; set; } = 0;
        public uint ReleaseVersionPatch { get; set; } = 0;

        public string ReleaseVersion
        {
            get => $"{ReleaseVersionMajor}.{ReleaseVersionMinor}.{ReleaseVersionPatch}";
        }
    }
}
