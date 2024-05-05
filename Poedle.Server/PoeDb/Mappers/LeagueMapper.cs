using Poedle.PoeDb.Models;
using Poedle.PoeWiki.Models;

namespace Poedle.PoeDb.Mappers
{
    public static class LeagueMapper
    {
        public static DbLeague MapLeague(PoeWikiLeague pLeague)
        {
            DbLeague league = new()
            {
                ReleaseVersion = pLeague.ReleaseVersion
            };

            BaseModelMapper.SetBasePoeFields(league, pLeague);

            league.Name = CleanLeagueName(league.Name);
            league.PageName = CleanLeagueName(league.PageName);

            DbVersionUtil version = new(pLeague.ReleaseVersion);
            league.ReleaseVersionMajor = version.Major;
            league.ReleaseVersionMinor = version.Minor;
            league.ReleaseVersionPatch = version.Patch;

            return league;
        }

        private static string CleanLeagueName(string pName)
        {
            // Remove the 'league' suffix at the end of each name.
            return pName.Replace(" league", "");
        }
    }
}
