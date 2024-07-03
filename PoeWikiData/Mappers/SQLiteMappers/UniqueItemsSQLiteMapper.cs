using PoeWikiApi.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Mappers.SQLiteMappers
{
    internal static class UniqueItemsSQLiteMapper
    {
        public static List<string> Map(UniqueItemWikiModel pModel, Dictionary<string, string> pItemClasses)
        {
            return 
            [
                SQLiteHelper.SQLiteString(null),
                SQLiteHelper.SQLiteString(pModel.Name),
                SQLiteHelper.SQLiteString(GetItemClassId(pModel, pItemClasses)),
                SQLiteHelper.SQLiteString(pModel.BaseItem),
                pModel.ReqLvl.ToString(),
                pModel.ReqDex.ToString(),
                pModel.ReqInt.ToString(),
                pModel.ReqStr.ToString()
            ];
        }

        private static string? GetItemClassId(UniqueItemWikiModel pModel, Dictionary<string, string> pItemClasses)
        {
            if (!pItemClasses.TryGetValue(pModel.ItemClass, out string? value))
            {
                return null;
            }

            return value;
        }
    }
}
