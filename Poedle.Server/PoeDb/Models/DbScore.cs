using Poedle.Enums;

namespace Poedle.PoeDb.Models
{
    public class DbScore : BaseDbModel
    {
        public string Date { get; set; } = "";
        public GameTypesEnum.GameTypes GameType { get; set; } = GameTypesEnum.GameTypes.NONE;
        public int Score { get; set; } = 0;
    }
}
