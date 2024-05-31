using LiteDB;


using Poedle.Enums;
using Poedle.PoeDb.Mappers;
using Poedle.PoeDb.Models;
using Poedle.PoeWiki.Models;
using Poedle.PoeWiki;
using Poedle.Utils.Logger;
using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.PoeDb.DbControllers
{
    public class DbScoreController(LiteDatabase pDb, DebugLogger pLogger) : BaseDbController(pDb, pLogger)
    {
        #region "GET"
        public List<DbScore> GetAll(ushort? pLimit = null)
        {
            return GetAllOrdered<DbScore>(DbColTypes.SCORES, pLimit);
        }

        public List<DbScore> GetAllByDate(DateTime pDate)
        {
            string utcDate = pDate.ToUniversalTime().ToShortDateString();
            string exp = $"$.Date={utcDate}";
            return GetAllByExpGeneric<DbScore>(DbColTypes.SCORES, exp, null, null);
        }

        public DbScore? GetByDateAndGame(DateTime pDate, GameTypesEnum.GameTypes pGameType)
        {
            string utcDate = pDate.ToUniversalTime().ToShortDateString();
            string exp = $"$.Date={utcDate} AND UPPER($.GameType)=UPPER({pGameType})";
            return GetByExpGeneric<DbScore>(DbColTypes.SCORES, exp);
        }
        #endregion

        #region "ADD"
        public void Add(DbScore pModel)
        {
            AddGeneric(DbColTypes.SCORES, pModel);
        }
        #endregion

        #region "RESET"
        public void ResetAll()
        {
            static void PostProcessAll(ILiteCollection<DbScore> x) => x.EnsureIndex("Date");

            ResetAll<DbScore>(DbColTypes.SCORES, PostProcessAll);
        }
        #endregion
    }
}
