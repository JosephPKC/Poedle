using PoeWikiData.Models;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Utils;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Mappers.StaticData
{
    internal class StaticDataSQLiteMapper
    {
        public static SQLiteValues Map(StaticDataDbModel pModel)
        {
            IEnumerable<string> values =
            [
                SQLiteUtils.SQLiteString(pModel.Id.ToString()),
                SQLiteUtils.SQLiteString(pModel.Name),
                SQLiteUtils.SQLiteString(StringUtils.DisplayText(pModel.Name)),
            ];
            return new(values);
        }
    }
}
