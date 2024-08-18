using System.Data;

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

        public static IEnumerable<LeagueDbModel> Read(IDataReader pReader)
        {
            ICollection<LeagueDbModel> models = [];
            while (pReader.Read())
            {
                LeagueDbModel model = new()
                {
                    Id = (uint)pReader.GetInt32(0),
                    Name = pReader.GetString(1),
                    DisplayName = pReader.GetString(2),
                    ReleaseVersion = new(pReader.GetString(3), pReader.GetString(4), pReader.GetString(5))
                };
                models.Add(model);
            }
            return models;
        }
    }
}
