using System.Data;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Models.UniqueItems;

namespace PoeWikiData.Mappers.UniqueItems
{
    internal static class UniqueItemSQLiteReader
    {
        public static IEnumerable<UniqueItemDbModel> Read(IDataReader pReader)
        {
            ICollection<UniqueItemDbModel> models = [];
            while (pReader.Read())
            {
                UniqueItemDbModel model = new()
                {
                    Id = (uint)pReader.GetInt32(0),
                    Name = pReader.GetString(1),
                    DisplayName = pReader.GetString(2),
                    ItemClass = GetItemClass((uint)pReader.GetInt32(3)),
                    BaseItem = pReader.GetString(4),
                    ReqLvl = (uint)pReader.GetInt32(5),
                    ReqDex = (uint)pReader.GetInt32(6),
                    ReqInt = (uint)pReader.GetInt32(7),
                    ReqStr = (uint)pReader.GetInt32(8)
                };
                models.Add(model);
            }
            return models;
        }

        private static StaticDataDbModel GetItemClass(uint pItemClassId)
        {
            return StaticDataMasterRef.ItemClasses.GetModelById(pItemClassId) ?? new();
        }
    }
}
