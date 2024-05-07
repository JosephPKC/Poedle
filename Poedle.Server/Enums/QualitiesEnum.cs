namespace Poedle.Enums
{
    public static class QualitiesEnum
    {
        public enum Qualities
        {
            NONE,
            ABYSSAL,
            ANOINTABLE,
            CORRUPTED,
            EATER_OF_WORLDS,
            ELDER,
            FRACTURED,
            GRANTS_SKILL,
            REPLICA,
            SEARING_EXARCH,
            SHAPER,
            SYNTHESISED,
            TRIGGERS_SKILL,
            UPGRADEABLE,
            VEILED,
        }

        private readonly static Dictionary<Qualities, string> _enumToStrExceptions = new()
        {
            { Qualities.EATER_OF_WORLDS, "Eater of Worlds" },
            { Qualities.GRANTS_SKILL, "Grants Skill" },
            { Qualities.SEARING_EXARCH, "Searing Exarch" },
            { Qualities.TRIGGERS_SKILL, "Triggers Skill" }
        };

        public static string EnumToStr(Qualities pEnum)
        {
            return EnumUtil.GetNameByValue(pEnum, _enumToStrExceptions);
        }
    }
}
