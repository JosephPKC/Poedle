namespace Poedle.Enums
{
    public static class DropSourcesEnum
    {
        public enum DropSources
        {
            NONE,
            BOSS_DROP
        }

        private readonly static Dictionary<DropSources, string> _enumToStrExceptions = new()
        {
            { DropSources.BOSS_DROP, "Boss Drop" }
        };

        private readonly static Dictionary<string, DropSources> _strToEnumExceptions = new()
        {
            { "Boss Drop", DropSources.BOSS_DROP }
        };

        public static string EnumToStr(DropSources pEnum)
        {
            return EnumUtil.GetNameByValue(pEnum, _enumToStrExceptions);
        }

        public static DropSources StrToEnum(string pEnum)
        {
            return EnumUtil.GetEnumByName(pEnum, _strToEnumExceptions);
        }
    }
}
