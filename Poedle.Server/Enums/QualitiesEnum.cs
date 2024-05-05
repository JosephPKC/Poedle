namespace Poedle.Enums
{
    public static class QualitiesEnum
    {
        public enum Qualities
        {
            NONE,
            CORRUPTED,
            FRACTURED,
            REPLICA,
            SYNTHESISED,
            VEILED,
            EATER_OF_WORLDS,
            SEARING_EXARCH,
            ELDER,
            SHAPER
        }

        private readonly static Dictionary<Qualities, string> _enumToStrExceptions = new()
        {
            { Qualities.EATER_OF_WORLDS, "Eater of Worlds" },
            { Qualities.SEARING_EXARCH, "Searing Exarch" }
        };

        private readonly static Dictionary<string, Qualities> _strToEnumExceptions = new()
        {
            { "Eater of Worlds", Qualities.EATER_OF_WORLDS },
            { "Searing Exarch", Qualities.SEARING_EXARCH }
        };

        public static string EnumToStr(Qualities pEnum)
        {
            return EnumUtil.GetNameByValue(pEnum, _enumToStrExceptions);
        }

        public static Qualities StrToEnum(string pEnum)
        {
            return EnumUtil.GetEnumByName(pEnum, _strToEnumExceptions);
        }
    }
}
