using PoeWikiApi.Models;
using PoeWikiData.Models.Leagues;

namespace PoeWikiData.Mappers.Leagues
{
    internal static class LeagueDbMapper
    {
        public static LeagueDbModel Map(LeagueWikiModel pModel)
        {
            (uint, uint, uint) releaseVersionSplit = GetSplitVersion(pModel.ReleaseVersion);
            return new()
            {
                Id = pModel.Id,
                Name = GetCleanedName(pModel.Name),
                ReleaseVersionMajor = releaseVersionSplit.Item1,
                ReleaseVersionMinor = releaseVersionSplit.Item2,
                ReleaseVersionPatch = releaseVersionSplit.Item3,
            };
        }

        private static string GetCleanedName(string pName)
        {
            return pName.Replace(" league", "");
        }

        private static (uint, uint, uint) GetSplitVersion(string pVersion)
        {
            string[] versionSplit = pVersion.Split(".");
            string major = "", minor = "", patch = "";
            if (versionSplit.Length > 0)
            {
                major = versionSplit[0];
            }

            if (versionSplit.Length > 1)
            {
                minor = versionSplit[1];
            }

            if (versionSplit.Length > 2)
            {
                patch = versionSplit[2];
            }

            return (uint.Parse(major), uint.Parse(minor), uint.Parse(patch));
        }
    }
}
