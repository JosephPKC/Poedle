using System.Diagnostics;
using System.Text;

using LiteDB;

using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;

using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.PoeDb.DbControllers
{
    public abstract class BaseDbGetter(LiteDatabase pDb, DebugLogger pLogger)
    {
        protected readonly LiteDatabase _db = pDb;
        protected readonly DebugLogger _log = pLogger;

        public int GetCount(DbColTypes pColType)
        {
            Stopwatch timer = new();
            string colName = DbParamsMap[pColType].ColName;
            _log.TimeStartLog(timer, $"BEGIN: GET COUNT {colName}.");

            ILiteCollection<BsonDocument> col = _db.GetCollection(colName);

            int result = col.Count();

            _log.TimeStopLogAndAppend(timer, $"END: GET COUNT {colName}. RESULT: {result}.");
            return result;
        }

        protected static string BuildQueryLog(string pCols, string? pWhereExp, string? pOrderExp, ushort? pLimit)
        {
            StringBuilder builder = new();
            builder.Append($"FROM \"{pCols}\"");

            if (!string.IsNullOrWhiteSpace(pWhereExp))
            {
                builder.Append($" WHERE \"{pWhereExp}\"");
            }

            if (!string.IsNullOrWhiteSpace(pOrderExp))
            {
                builder.Append($" ORDER BY \"{pOrderExp}\"");
            }

            if (pLimit != null)
            {
                builder.Append($" LIMIT {pLimit}");
            }
            
            return builder.ToString();
        }

        #region "Templates"
        protected List<T> GetAllByExpGeneric<T>(DbColTypes pColType, string? pWhereExp = null, string? pOrderExp = null, ushort? pLimit = null) where T : BaseDbModel
        {
            Stopwatch timer = new();
            string colName = DbParamsMap[pColType].ColName;
            string queryStr = BuildQueryLog(colName, pWhereExp, pOrderExp, pLimit);
            _log.TimeStartLog(timer, $"BEGIN: GET ALL {queryStr}.");

            ILiteCollection<T> col = _db.GetCollection<T>(colName);
            ILiteQueryable<T> colQuery = col.Query();

            if (!string.IsNullOrWhiteSpace(pWhereExp))
            {
                colQuery = colQuery.Where(BsonExpression.Create(pWhereExp));
            }

            if (!string.IsNullOrWhiteSpace(pOrderExp))
            {
                colQuery = colQuery.OrderBy(BsonExpression.Create(pOrderExp));
            }

            ILiteQueryableResult<T> result = pLimit == null ? colQuery : colQuery.Limit(pLimit.Value);
            List<T> resultList = result.ToList();

            _log.TimeStopLogAndAppend(timer, $"END: GET ALL {queryStr}. RESULTS: {string.Join(",", resultList.Select(x => x.Id))}.");
            return resultList;
        }

        protected T? GetByExpGeneric<T>(DbColTypes pColType, string pExpression) where T : BaseDbModel
        {
            Stopwatch timer = new();
            string colName = DbParamsMap[pColType].ColName;
            string queryStr = BuildQueryLog(colName, pExpression, null, null);
            _log.TimeStartLog(timer, $"BEGIN: GET ONE {queryStr}.");

            ILiteCollection<T> col = _db.GetCollection<T>(colName);
            T? result = col.FindOne(BsonExpression.Create(pExpression));

            _log.TimeStopLogAndAppend(timer, $"END: GET ONE {queryStr}. RESULT: {(result == null ? "null" : result.Id)}.");
            return result;
        }
        #endregion

        protected List<T> GetAllOrdered<T>(DbColTypes pColType, ushort? pLimit = null) where T : BaseDbModel
        {
            string expOrder = "$._id";
            return GetAllByExpGeneric<T>(pColType, null, expOrder, pLimit);
        }

        protected List<T> GetAllByName<T>(DbColTypes pColType, string pName) where T : BaseDbModel
        {
            string exp = $"UPPER($.Name) = UPPER(\"{pName}\")";
            return GetAllByExpGeneric<T>(pColType, exp);
        }

        protected T? GetByPageName<T>(DbColTypes pColType, string pPageName) where T : BaseDbModel
        {
            string exp = $"UPPER($.PageName)=UPPER(\"{pPageName}\")";
            return GetByExpGeneric<T>(pColType, exp);
        }

        protected T? GetById<T>(DbColTypes pColType, int pId) where T : BaseDbModel
        {
            string exp = $"$._id={pId}";
            return GetByExpGeneric<T>(pColType, exp);
        }

        protected T? GetRandom<T>(DbColTypes pColType) where T : BaseDbModel
        {
            Stopwatch timer = new();
            string colName = DbParamsMap[pColType].ColName;
            _log.TimeStartLog(timer, $"BEGIN: GET RANDOM FROM {colName}.");

            ILiteCollection<T> col = _db.GetCollection<T>(colName);

            Random rnd = new();
            int offset = rnd.Next(0, col.Count());

            T? result = col.Query().Limit(1).Offset(offset).Single();

            _log.TimeStopLogAndAppend(timer, $"END: GET RANDOM FROM {colName}. RESULT: {(result == null ? "null" : result.Id)}.");
            return result;
        }
    }
}
