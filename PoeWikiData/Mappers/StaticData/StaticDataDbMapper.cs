using PoeWikiData.Models.StaticData;
using PoeWikiData.Utils;

namespace PoeWikiData.Mappers.StaticData
{
    internal static class StaticDataDbMapper
    {
        public static StaticDataDbModel GetStaticDataFromEnum<T>(T pEnum, StaticDataDbLookUp pRefList) where T : Enum
        {
            return GetStaticDataFromEnum(pEnum.ToString(), EnumUtils.GetEnumValue(pEnum), pRefList);
        }

        public static StaticDataDbModel GetStaticDataFromEnum(string pEnumName, uint pEnumValue, StaticDataDbLookUp pRefList)
        {
            if (!pRefList.HasId(pEnumValue) || !pRefList.HasName(pEnumName))
            {
                throw new Exception($"Item Aspect Enum {pEnumName}/{pEnumValue} is invalid. Make sure the db table is updated.");
            }

            return new()
            {
                Id = pRefList.GetId(pEnumName) ?? 0,
                Name = pRefList.GetName(pEnumValue) ?? string.Empty
            };
        }
    }
}
