using Poedle.Enums;
using Poedle.PoeDb;
using Poedle.PoeDb.Mappers;
using Poedle.Utils.Logger;

namespace Poedle.Game.Managers
{
    public class ScoreTracker(PoeDbManager pDb, DebugLogger pLog)
    {
        private readonly PoeDbManager _db = pDb;
        private readonly DebugLogger _log = pLog;
     
        public int CurrentScore { get; private set; }

        public void IncrementScore()
        {
            CurrentScore++;
        }

        public void SaveScore(GameTypesEnum.GameTypes pGameType)
        {
            _db.Score.Add(ScoreMapper.MapScore(DateTime.Now, pGameType, CurrentScore));
        }
        // Track # of guesses (score)
        // Save the score to db, should be date, game, score
        // Optionally, retrieve data about past scores to determine:
            // Average, min, max, and mode
            // Maybe have a nice presentable list with index being date, and then a mapping from game to score
            // or have multiple lists, one for each game type, with index being date and value being score
    }
}
