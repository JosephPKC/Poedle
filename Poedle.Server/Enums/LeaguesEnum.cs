namespace Poedle.Enums
{
    public static class LeaguesEnum
    {
        public enum Leagues
        {
            NONE,
            BASE_GAME,
            ANARCHY,
            ONSLAUGHT,
            DOMINATION,
            NEMESIS,
            AMBUSH,
            INVASION,
            RAMPAGE,
            BEYOND,
            TORMENT,
            BLOODLINES,
            WARBANDS,
            TEMPEST,
            TALISMAN,
            PERANDUS,
            PROPHECY,
            ESSENCE,
            BREACH,
            LEGACY,
            HARBINGER,
            ABYSS,
            BESTIARY,
            INCURSION,
            DELVE,
            BETRAYAL,
            SYNTHESIS,
            LEGION,
            BLIGHT,
            METAMORPH,
            DELIRIUM,
            HARVEST,
            HEIST,
            RITUAL,
            ULTIMATUM,
            EXPEDITION,
            SCOURGE,
            ARCHNEMESIS,
            SENTINEL,
            KALANDRA,
            SANCTUM,
            CRUCIBLE,
            ANCESTOR,
            AFFLICTION,
            NECROPOLIS
        }

        private readonly static Dictionary<Leagues, string> _enumToStrExceptions = new()
        {
            { Leagues.BASE_GAME, "Base Game" }
        };

        private readonly static Dictionary<string, Leagues> _strToEnumExceptions = new()
        {
            { "Base Game", Leagues.BASE_GAME }
        };

        public static string EnumToStr(Leagues pEnum)
        {
            return EnumUtil.GetNameByValue(pEnum, _enumToStrExceptions);
        }

        public static Leagues StrToEnum(string pEnum)
        {
            return EnumUtil.GetEnumByName(pEnum, _strToEnumExceptions);
        }
    }
}
