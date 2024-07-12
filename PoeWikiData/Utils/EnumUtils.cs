namespace PoeWikiData.Utils
{
    internal static class EnumUtils
    {
        public static T GetEnum<T>(string pEnumName) where T : Enum
        {
            string enumNameNoSpace = pEnumName.Replace(" ", "").Replace("-", "");
            return (T)Enum.Parse(typeof(T), enumNameNoSpace, true);
        }

        public static uint GetEnumValue<T>(T pEnum) where T : Enum
        {
            return (uint)(int)(object)pEnum;
        }
    }
}
