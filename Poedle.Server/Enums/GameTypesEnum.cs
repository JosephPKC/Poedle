namespace Poedle.Enums
{
    public static class GameTypesEnum
    {
        public enum GameTypes
        {
            NONE,
            GUESS_UNIQUE_BY_PARAM
        }

        private readonly static Dictionary<GameTypes, string> _enumToStrExceptions = new()
        {
            { GameTypes.GUESS_UNIQUE_BY_PARAM, "Guess Unique By Param" }
        };

        public static string EnumToStr(GameTypes pEnum)
        {
            return EnumUtil.GetNameByValue(pEnum, _enumToStrExceptions);
        }
    }
}
