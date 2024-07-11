using PoeWikiData.Models.UniqueItems;
using PoeWikiData.Utils;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Mappers.UniqueItems
{
    internal static class UniqueItemSQLiteMapper
    {
        public static SQLiteValues Map(UniqueItemDbModel pModel)
        {
            List<string> values =
            [
                SQLiteHelper.SQLiteString(null),
                SQLiteHelper.SQLiteString(pModel.Name),
                SQLiteHelper.SQLiteString(pModel.ItemClass.Id.ToString()),
                SQLiteHelper.SQLiteString(pModel.BaseItem),
                pModel.ReqLvl.ToString(),
                pModel.ReqDex.ToString(),
                pModel.ReqInt.ToString(),
                pModel.ReqStr.ToString()
            ];
            return new(values);
        }
    }
}
