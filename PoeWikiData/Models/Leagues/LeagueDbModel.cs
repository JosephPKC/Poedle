using PoeWikiData.Models.Common;

namespace PoeWikiData.Models.Leagues
{
    public class LeagueDbModel : BaseNamedDbModel
    {
        public DbVersion ReleaseVersion { get; set; } = new();
    }
}
