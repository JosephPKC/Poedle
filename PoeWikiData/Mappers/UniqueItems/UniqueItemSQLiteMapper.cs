using PoeWikiData.Models;
using PoeWikiData.Models.UniqueItems;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Mappers.UniqueItems
{
    internal static class UniqueItemSQLiteMapper
    {
        public static SQLiteValues Map(UniqueItemDbModel pModel)
        {
            List<string> values =
            [
                pModel.Id.ToString(),
                SQLiteUtils.SQLiteString(pModel.Name),
                SQLiteUtils.SQLiteString(pModel.DisplayName),
                SQLiteUtils.SQLiteString(pModel.ItemClass.Id.ToString()),
                SQLiteUtils.SQLiteString(pModel.BaseItem),
                pModel.ReqLvl.ToString(),
                pModel.ReqDex.ToString(),
                pModel.ReqInt.ToString(),
                pModel.ReqStr.ToString()
            ];
            return new(values);
        }

        public static SQLiteValues MapLink(UniqueItemDbModel pModel, BaseDbModel pLink)
        {
            List<string> values =
            [
                pModel.Id.ToString(),
                pLink.Id.ToString()
            ];
            return new(values);
        }

        public static SQLiteValues MapLink<T>(UniqueItemDbModel pModel, T pLink, uint pOrder)
        {
            List<string> values =
            [
                pModel.Id.ToString(),
                SQLiteUtils.SQLiteString(pLink?.ToString()),
                pOrder.ToString()
            ];
            return new(values);
        }
    }
}
