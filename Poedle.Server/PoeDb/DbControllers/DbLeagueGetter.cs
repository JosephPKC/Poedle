using LiteDB;

using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;

using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.PoeDb.DbControllers
{
    public class DbLeagueGetter(LiteDatabase pDb, DebugLogger pLogger) : BaseDbGetter(pDb, pLogger)
    {
        public List<DbLeague> GetAll(ushort? pLimit = null)
        {
            return GetAllOrdered<DbLeague>(DbColTypes.LEAGUES, pLimit);
        }

        public DbLeague GetById(int pId)
        {
            return GetById<DbLeague>(DbColTypes.LEAGUES, pId);
        }

        public List<DbLeague> GetByReleaseVersion(string pReleaseVersion)
        {
            string exp = $"UPPER($.ReleaseVersion)=UPPER(\"{pReleaseVersion}\")";
            return GetAllByExpGeneric<DbLeague>(DbColTypes.LEAGUES, exp);
        }

        public List<DbLeague> GetByMajorMinorVersion(string pVersion)
        {
            DbVersionUtil version = new(pVersion);
            string exp = $"UPPER($.ReleaseVersionMajor)=UPPER(\"{version.Major}\") AND UPPER($.ReleaseVersionMinor)=UPPER(\"{version.Minor}\")";
            return GetAllByExpGeneric<DbLeague>(DbColTypes.LEAGUES, exp);
        }
    }
}
