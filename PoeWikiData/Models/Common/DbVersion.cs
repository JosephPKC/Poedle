namespace PoeWikiData.Models.Common
{
    public class DbVersion
    {
        public string Major { get; set; } = string.Empty;
        public string Minor { get; set; } = string.Empty;
        public string Patch { get; set; } = string.Empty;

        public string FullText
        {
            get => $"{Major}.{Minor}.{Patch}";
        }

        public string MajorMinorText
        {
            get => $"{Major}.{Minor}.0";
        }

        public DbVersion() { }

        public DbVersion(string pMajor, string pMinor, string pPatch)
        {
            Major = pMajor;
            Minor = pMinor;
            Patch = pPatch;
        }

        public DbVersion(string pVersion)
        {
            string[] versionSplit = pVersion.Split('.');
            if (versionSplit.Length < 3) return;

            Major = versionSplit[0];
            Minor = versionSplit[1];
            Patch = versionSplit[2];
        }
    }
}
