using BaseToolsUtils.Utils;
using PoeWikiData.Models.StaticData.Enums;
using PoeWikiData.Utils;

namespace PoeWikiData.Models.StaticData
{
    internal static class StaticDataMasterRef
    {
        public static StaticDataDbLookUp DropSources { get; private set; } = GetStaticDataLookUpFromEnum<DropSources>();
        public static StaticDataDbLookUp DropTypes { get; private set; } = GetStaticDataLookUpFromEnum<DropTypes>();
        public static StaticDataDbLookUp GemTags { get; private set; } = GetStaticDataLookUpFromEnum<GemTags>();
        public static StaticDataDbLookUp ItemAspects { get; private set; } = GetStaticDataLookUpFromEnum<ItemAspects>();
        public static StaticDataDbLookUp ItemClasses { get; private set; } = GetStaticDataLookUpFromEnum<ItemClasses>();

        private static StaticDataDbLookUp GetStaticDataLookUpFromEnum<TEnum>() where TEnum : struct, Enum
        {
            ICollection<StaticDataDbModel> result = [];
            TEnum[] values = Enum.GetValues<TEnum>();
            foreach (TEnum value in values)
            {
                StaticDataDbModel model = new()
                {
                    Id = EnumUtils.GetEnumValue(value),
                    Name = value.ToString(),
                    DisplayName = GeneralUtils.DisplayText(value.ToString())
                };
                result.Add(model);
            }
            return new(result);
        }
    }
}
