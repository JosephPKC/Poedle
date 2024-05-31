using System.Text.RegularExpressions;

namespace Poedle.PoeDb
{
    public partial class DbVersionUtil
    {
        [GeneratedRegex("[\\d]+\\.[\\d]+\\.[\\d]+")]
        private static partial Regex VersionFormatRegex();

        public string Major { get; set; } = "0";
        public string Minor { get; set; } = "0";
        public string Patch { get; set; } = "0";

        public string VersionText
        {
            get
            {
                return $"{MajorMinorText}.{Patch}";
            }
        }

        public string MajorMinorText
        {
            get
            {
                return $"{Major}.{Minor}";
            }
        }

        public DbVersionUtil()
        {

        }

        public DbVersionUtil(string pVersion)
        {
            if (string.IsNullOrWhiteSpace(pVersion))
            {
                // Use defaults
                return;
            }
            if (VersionFormatRegex().Match(pVersion) == Match.Empty)
            {
                throw new ArgumentException($"{pVersion} needs to be in Major.Minor.Patch format.");
            }

            string[] versionSplit = pVersion.Split(".");
            Major = versionSplit[0];
            Minor = versionSplit[1];
            Patch = versionSplit[2];
        }

        public bool Equals(DbVersionUtil? other)
        {
            if (other == null) return false;

            return Major == other.Major && Minor == other.Minor && Patch == other.Patch;
        }
    }
}
