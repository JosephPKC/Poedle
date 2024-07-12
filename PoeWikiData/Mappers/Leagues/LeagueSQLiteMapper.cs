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
                pModel.ReleaseVersionMajor.ToString(),
                pModel.ReleaseVersionMinor.ToString(),
                pModel.ReleaseVersionPatch.ToString()
            ];
            return new(values);
        }
    }
}
