using PoeWikiApi.Models;
using PoeWikiData.Models.Leagues;

namespace PoeWikiData.Mappers.Leagues
{
    internal static class LeagueDbMapper
    {
        public static LeagueDbModel Map(LeagueWikiModel pModel)
        {
            return new()
            {
                Id = pModel.Id,
                Name = GetCleanedName(pModel.Name),
                ReleaseVersion = new(pModel.ReleaseVersion)
            };
        }

        private static string GetCleanedName(string pName)
        {
            return pName.Replace(" league", "");
        }
    }
}
