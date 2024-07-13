using PoeWikiData.Models.Leagues;
using PoeWikiData.Utils;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Mappers.Leagues
{
    internal static class LeagueSQLiteMapper
    {
        public static Models.SQLiteValues Map(LeagueDbModel pModel)
        {
            IEnumerable<string> values =
            [
                pModel.Id.ToString(),
                SQLiteUtils.SQLiteString(StringUtils.TitleCase(pModel.Name)),
                SQLiteUtils.SQLiteString(pModel.Name),
                SQLiteUtils.SQLiteString(pModel.ReleaseVersion.Major),
                SQLiteUtils.SQLiteString(pModel.ReleaseVersion.Minor),
                SQLiteUtils.SQLiteString(pModel.ReleaseVersion.Patch)
            ];
            return new(values);
        }
    }
}
