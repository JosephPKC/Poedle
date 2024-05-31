using Poedle.PoeDb.Models;
using Poedle.Enums;

namespace Poedle.PoeDb.Mappers
{
    public static class ScoreMapper
    {
        public static DbScore MapScore(DateTime pDate, GameTypesEnum.GameTypes pGameType, int pScore)
        {
            DbScore model = new()
            {
                Date = pDate.ToUniversalTime().ToShortDateString(),
                GameType = pGameType,
                Score = pScore
            };

            return model;
        }
    }
}
