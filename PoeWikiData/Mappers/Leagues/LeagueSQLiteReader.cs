using System.Data;
using PoeWikiData.Models.Leagues;

namespace PoeWikiData.Mappers.Leagues
{
    internal class LeagueSQLiteReader
    {
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
                    ReleaseVersionMajor = (uint)pReader.GetInt32(3),
                    ReleaseVersionMinor = (uint)pReader.GetInt32(4),
                    ReleaseVersionPatch = (uint)pReader.GetInt32(5)
                };
                models.Add(model);
            }
            return models;
        }
    }
}
