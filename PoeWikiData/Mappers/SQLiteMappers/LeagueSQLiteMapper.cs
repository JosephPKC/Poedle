using PoeWikiApi.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Mappers.SQLiteMappers
{
    internal static class LeagueSQLiteMapper
    {
        public static List<string> Map(LeagueWikiModel pModel)
        {
            (string, string, string) releaseVersionSplit = GetSplitVersion(pModel.ReleaseVersion);
            return 
            [
                SQLiteHelper.SQLiteString(null),
                SQLiteHelper.SQLiteString(GetCleanedName(pModel.Name)),
                SQLiteHelper.SQLiteString(releaseVersionSplit.Item1),
                SQLiteHelper.SQLiteString(releaseVersionSplit.Item2),
                SQLiteHelper.SQLiteString(releaseVersionSplit.Item3),
            ];
        }

        private static string GetCleanedName(string pName)
        {
            return pName.Replace(" league", "");
        }

        private static (string, string, string) GetSplitVersion(string pVersion)
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

            return (major, minor, patch);
        }
    }
}
