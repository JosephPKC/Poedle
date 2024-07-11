using PoeWikiData.Models.Leagues;
using PoeWikiData.Utils;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Mappers.Leagues
{
    internal static class LeagueSQLiteMapper
    {
        public static SQLiteValues Map(LeagueDbModel pModel)
        {
            List<string> values =
            [
                SQLiteHelper.SQLiteString(null),
                SQLiteHelper.SQLiteString(pModel.Name),
                SQLiteHelper.SQLiteString(pModel.ReleaseVersionMajor),
                SQLiteHelper.SQLiteString(pModel.ReleaseVersionMinor),
                SQLiteHelper.SQLiteString(pModel.ReleaseVersionPatch),
            ];
            return new(values);
        }
    }
}
