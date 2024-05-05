namespace Poedle.PoeDb.Models
{
    public class DbLeague : BaseDbPoeModel
    {
        public string ReleaseVersionMajor { get; set; } = "";
        public string ReleaseVersionMinor { get; set; } = "";
        public string ReleaseVersionPatch { get; set; } = "";
        public string ReleaseVersion { get; set; } = "";
    }
}
