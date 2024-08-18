using PoeWikiData.Models;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Mappers.Links
{
    internal static class LinkSQLiteMapper
    {
        public static SQLiteValues MapLink(BaseDbModel pModel, BaseDbModel pLink)
        {
            List<string> values =
            [
                pModel.Id.ToString(),
                pLink.Id.ToString()
            ];
            return new(values);
        }

        public static SQLiteValues MapLink<T>(BaseDbModel pModel, T pLink, uint pOrder)
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
