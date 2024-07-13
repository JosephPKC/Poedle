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
                    ReleaseVersion = new(pReader.GetString(3), pReader.GetString(4), pReader.GetString(5))
                };
                models.Add(model);
            }
            return models;
        }
    }
}
