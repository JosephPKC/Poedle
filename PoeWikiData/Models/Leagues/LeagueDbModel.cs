using PoeWikiData.Models.Common;

namespace PoeWikiData.Models.Leagues
{
    public class LeagueDbModel : BaseNamedDbModel, IEquatable<LeagueDbModel>
    {
        public DbVersion ReleaseVersion { get; set; } = new();

        public bool Equals(LeagueDbModel? other)
        {
            if (this == other) return true;

            if (other == null) return false;

            return Id == other.Id;
        }
    }
}
