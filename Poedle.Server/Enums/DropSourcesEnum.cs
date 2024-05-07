namespace Poedle.Enums
{
    public static class DropSourcesEnum
    {
        public enum DropSources
        {
            NONE,
            BOSS_DROP,
            SPECIAL,
            SPECIAL_DROP_AREA,
            SPECIAL_ENCOUNTER,
            UPGRADE,
            VENDOR_RECIPE,
        }

        private readonly static Dictionary<DropSources, string> _enumToStrExceptions = new()
        {
            { DropSources.BOSS_DROP, "Boss Drop" },
            { DropSources.SPECIAL_DROP_AREA, "Special Drop Area" },
            { DropSources.SPECIAL_ENCOUNTER, "Special Encounter" },
            { DropSources.VENDOR_RECIPE, "Vendor Recipe" }
        };

        public static string EnumToStr(DropSources pEnum)
        {
            return EnumUtil.GetNameByValue(pEnum, _enumToStrExceptions);
        }
    }
}
