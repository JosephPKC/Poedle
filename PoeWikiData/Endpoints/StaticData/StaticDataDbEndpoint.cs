using PoeWikiData.Models.StaticData;
using PoeWikiData.Schema;

namespace PoeWikiData.Endpoints.StaticData
{
    internal class StaticDataDbEndpoint
    {
        //public void UpdateDropSources()
        //{
        //    UpdateStaticData("Drop Sources", PoeDbSchemaTypes.DropSources, Enum.GetValues<DropSources>());
        //}

        //public void UpdateDropTypes()
        //{
        //    UpdateStaticData("Drop Types", PoeDbSchemaTypes.DropTypes, Enum.GetValues<DropTypes>());
        //}

        //public void UpdateItemAspects()
        //{
        //    UpdateStaticData("Item Aspects", PoeDbSchemaTypes.ItemAspects, Enum.GetValues<ItemAspects>());
        //}

        //public void UpdateItemClasses()
        //{
        //    UpdateStaticData("Item Classes", PoeDbSchemaTypes.ItemClasses, Enum.GetValues<ItemClasses>());
        //}

        //private void UpdateStaticData<T>(string pOperationName, PoeDbSchemaTypes pSchemaType, T[] pAllEnumValues, bool pIsForceUpdate = false) where T : Enum
        //{
        //    Stopwatch timer = new();
        //    _log.TimeStartLog(timer, $"BEGIN: UPDATE {pOperationName}");

        //    if (pAllEnumValues.Length == 0)
        //    {
        //        string toDo = pIsForceUpdate ? "Forcefully updating (table will be empty)." : "Skipping.";
        //        _log.Log($"Enum of type {typeof(T).Name} has no values given. {toDo}");
        //        if (!pIsForceUpdate)
        //        {
        //            _log.TimeStopLogAndAppend(timer, $"END: SKIPPED UPDATE {pOperationName}");
        //            return;
        //        }
        //    }

        //    ResetTable(pSchemaType);
        //    InsertAllStaticData(pSchemaType, pAllEnumValues);

        //    _log.TimeStopLogAndAppend(timer, $"END: UPDATE {pOperationName}");
        //}

        //private void InsertAllStaticData<T>(PoeDbSchemaTypes pSchemaType, T[] pAllEnumValues) where T : Enum
        //{
        //    List<SQLiteValues> allSQLValues = [];
        //    foreach (T enumVal in pAllEnumValues)
        //    {
        //        StaticDataDbModel model = new(BaseUtils.GetEnumValue(enumVal), enumVal.ToString());
        //        SQLiteValues values = StaticDataSQLiteMapper.Map(model);
        //        allSQLValues.Add(values);
        //    }

        //    PoeDbSchema schema = PoeDbSchemaList.SchemaList[pSchemaType];
        //    _db.InsertAllIntoTable(schema.TableName, null, allSQLValues);
        //}

        //public StaticDataDbLookUp SelectAllDropSources()
        //{
        //    return _db.SelectStaticData(PoeDbSchemaList.SchemaList[PoeDbSchemaTypes.DropSources].TableName);
        //}

        //public StaticDataDbLookUp SelectAllDropTypes()
        //{

        //    return _db.SelectStaticData(PoeDbSchemaList.SchemaList[PoeDbSchemaTypes.DropTypes].TableName);
        //}

        //public StaticDataDbLookUp SelectAllItemAspects()
        //{
        //    return _db.SelectStaticData(PoeDbSchemaList.SchemaList[PoeDbSchemaTypes.ItemAspects].TableName);
        //}

        //public StaticDataDbLookUp SelectAllItemClasses()
        //{
        //    return _db.SelectStaticData(PoeDbSchemaList.SchemaList[PoeDbSchemaTypes.ItemClasses].TableName);
        //}
    }
}
