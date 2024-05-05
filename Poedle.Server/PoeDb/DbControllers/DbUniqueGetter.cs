using LiteDB;

using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;

using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.PoeDb.DbControllers
{
    public class DbUniqueGetter(LiteDatabase pDb, DebugLogger pLogger) : BaseDbGetter(pDb, pLogger)
    {
        public List<DbUnique> GetAll(ushort? pLimit = null)
        {
            return GetAllOrdered<DbUnique>(DbColTypes.UNIQUES, pLimit);
        }

        public DbUnique GetById(int pId)
        {
            return GetById<DbUnique>(DbColTypes.UNIQUES, pId);
        }

        public DbUnique GetByPageName(string pPageName)
        {
            return GetByPageName<DbUnique>(DbColTypes.UNIQUES, pPageName);
        }

        public DbUnique GetRandom()
        {
            return GetRandom<DbUnique>(DbColTypes.UNIQUES);
        }
    }
}
