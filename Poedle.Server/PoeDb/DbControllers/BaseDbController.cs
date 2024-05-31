using System.Diagnostics;
using System.Text;

using LiteDB;

using Poedle.PoeDb.Models;
using Poedle.PoeWiki.Models;
using Poedle.Utils.Logger;

using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.PoeDb.DbControllers
{
    public abstract class BaseDbController(LiteDatabase pDb, DebugLogger pLogger)
    {
        protected readonly LiteDatabase _db = pDb;
        protected readonly DebugLogger _log = pLogger;

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

        #region "GET"
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
        #endregion

        #region "DELETE"
        protected void DeleteAll<T>(DbColTypes pColType) where T : BaseDbModel
        {
            Stopwatch timer = new();
            string colName = GetColName(pColType);
            _log.TimeStartLog(timer, $"BEGIN: DELETE ALL FROM {colName.ToUpper()}.");

            ILiteCollection<T> col = _db.GetCollection<T>(colName);
            DeleteAll(col);

            _log.TimeStopLogAndAppend(timer, $"END: DELETE ALL FROM {colName.ToUpper()}.");
        }

        protected void DeleteAll<T>(ILiteCollection<T> pCol) where T : BaseDbModel
        {
            int docsDeleted = pCol.DeleteAll();
            _log.Log($"DELETED {docsDeleted} docs.");
        }
        #endregion

        #region "ADD"
        #region "Generic"
        protected void AddAllGeneric<T>(DbColTypes pColType, List<T> pToAdd, Action<T>? pPostProcess = null, Action<ILiteCollection<T>>? pPostProcessAll = null) where T : BaseDbModel
        {
            Stopwatch timer = new();
            string colName = GetColName(pColType);
            _log.TimeStartLog(timer, $"BEGIN: ADD ALL TO {colName.ToUpper()}.");

            ILiteCollection<T> col = _db.GetCollection<T>(GetColName(pColType));
            AddAllGeneric(col, pToAdd, pPostProcess, pPostProcessAll);

            _log.TimeStopLogAndAppend(timer, $"END: ADD ALL TO {colName.ToUpper()}.");
        }

        protected void AddAllGeneric<T>(ILiteCollection<T> pCol, List<T> pToAdd, Action<T>? pPostProcess = null, Action<ILiteCollection<T>>? pPostProcessAll = null) where T : BaseDbModel
        {
            foreach (T val in pToAdd)
            {
                AddGeneric(pCol, val, pPostProcess);
            }
            pPostProcessAll?.Invoke(pCol);
        }

        protected void AddGeneric<T>(DbColTypes pColType, T pToAdd, Action<T>? pPostProcess = null) where T : BaseDbModel
        {
            Stopwatch timer = new();
            string colName = GetColName(pColType);
            _log.TimeStartLog(timer, $"BEGIN: ADD TO {colName.ToUpper()}.");

            ILiteCollection<T> col = _db.GetCollection<T>(GetColName(pColType));
            AddGeneric(col, pToAdd, pPostProcess);

            _log.TimeStopLogAndAppend(timer, $"END: ADD TO {colName.ToUpper()}.");
        }

        protected void AddGeneric<T>(ILiteCollection<T> pCol, T pToAdd, Action<T>? pPostProcess = null) where T : BaseDbModel
        {
            if (pToAdd == null)
            {
                return;
            }
            pPostProcess?.Invoke(pToAdd);
            pCol.Insert(pToAdd);
        }
        #endregion

        #region "From Wiki Api Generic"
        protected void AddAllFromApiGeneric<D, W>(DbColTypes pColType, List<W> pAllFromWikiApi, Func<W, D> pGetDbModel, Action<D, W>? pPostProcess = null, Action<ILiteCollection<D>>? pPostProcessAll = null) where D : BaseDbPoeModel where W : BasePoeWikiModel
        {
            Stopwatch timer = new();
            string colName = GetColName(pColType);
            _log.TimeStartLog(timer, $"BEGIN: ADD ALL TO {colName.ToUpper()}.");

            ILiteCollection<D> col = _db.GetCollection<D>(GetColName(pColType));
            AddAllFromApiGeneric(col, pAllFromWikiApi, pGetDbModel, pPostProcess, pPostProcessAll);

            _log.TimeStopLogAndAppend(timer, $"END: ADD ALL TO {colName.ToUpper()}.");
        }

        protected void AddAllFromApiGeneric<D, W>(ILiteCollection<D> pCol, List<W> pAllFromWikiApi, Func<W, D> pGetDbModel, Action<D, W>? pPostProcess = null, Action<ILiteCollection<D>>? pPostProcessAll = null) where D : BaseDbPoeModel where W : BasePoeWikiModel
        {
            foreach (W apiData in pAllFromWikiApi)
            {
                D dbData = pGetDbModel(apiData);
                AddFromApiGeneric(pCol, apiData, dbData, pPostProcess);
            }
            pPostProcessAll?.Invoke(pCol);
        }

        protected void AddFromApiGeneric<D, W>(DbColTypes pColType, W pFromWikiApi, D pDbModel, Action<D, W>? pPostProcess = null) where D : BaseDbPoeModel where W : BasePoeWikiModel
        {
            Stopwatch timer = new();
            string colName = GetColName(pColType);
            _log.TimeStartLog(timer, $"BEGIN: ADD TO {colName.ToUpper()}.");

            ILiteCollection<D> col = _db.GetCollection<D>(GetColName(pColType));
            AddFromApiGeneric(col, pFromWikiApi, pDbModel, pPostProcess);

            _log.TimeStopLogAndAppend(timer, $"END: ADD TO {colName.ToUpper()}.");
        }

        protected void AddFromApiGeneric<D, W>(ILiteCollection<D> pCol, W pFromWikiApi, D pDbModel, Action<D, W>? pPostProcess = null) where D : BaseDbPoeModel where W : BasePoeWikiModel
        {
            if (pDbModel == null)
            {
                _log.Log($"DB: DATA IS NULL: {pFromWikiApi.Name}.");
                return;
            }

            pPostProcess?.Invoke(pDbModel, pFromWikiApi);
            _log.Log($"DB INSERT: {pDbModel.Name} / {pDbModel.PageName}.");
            pCol.Insert(pDbModel);
        }
        #endregion
        #endregion

        #region "RESET"
        protected void ResetAll<D, W>(DbColTypes pColType, Func<List<W>> pGetAllFromApi, Func<W, D> pGetDbModel, Action<D, W>? pPostProcess = null, Action<ILiteCollection<D>>? pPostProcessAll = null) where D : BaseDbPoeModel where W : BasePoeWikiModel
        {
            Stopwatch timer = new();
            string colName = GetColName(pColType);
            _log.TimeStartLog(timer, $"BEGIN: RESET {colName.ToUpper()}.");

            ILiteCollection<D> col = _db.GetCollection<D>(colName);
            DeleteAll(col);
            List<W> allFromWikiApi = pGetAllFromApi();
            AddAllFromApiGeneric(col, allFromWikiApi, pGetDbModel, pPostProcess, pPostProcessAll);

            _log.TimeStopLogAndAppend(timer, $"END: RESET {colName.ToUpper()}.");
        }

        protected void ResetAll<T>(DbColTypes pColType, Action<ILiteCollection<T>>? pPostProcessAll = null) where T : BaseDbModel
        {
            Stopwatch timer = new();
            string colName = GetColName(pColType);
            _log.TimeStartLog(timer, $"BEGIN: RESET {colName.ToUpper()}.");

            ILiteCollection<T> col = _db.GetCollection<T>(colName);
            DeleteAll(col);
            pPostProcessAll?.Invoke(col);

            _log.TimeStopLogAndAppend(timer, $"END: RESET {colName.ToUpper()}.");
        }
        #endregion
    }
}
