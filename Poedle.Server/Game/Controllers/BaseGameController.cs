using Poedle.Game.Models.Games;
using Poedle.Game.Models.GuessResults;
using Poedle.Game.Models.Params;
using Poedle.Game.Utils;
using Poedle.PoeDb;
using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;

namespace Poedle.Game.Controllers
{
    public abstract class BaseGameController(PoeDbManager pDb, DebugLogger pLog)
    {
        protected readonly PoeDbManager _db = pDb;
        protected readonly DebugLogger _log = pLog;

        protected BaseGame? _GameState;

        protected T? CastGameState<T>() where T : BaseGame
        {
            if (_GameState == null) return null;
            return (T)_GameState;
        }

        public int? AnswerId
        {
            get
            {
                return _GameState?.ModelRef?.Id;
            }
        }

        public virtual void ToggleHints()
        {
            if (_GameState == null) return;

            _GameState.AreHintsEnabled = !_GameState.AreHintsEnabled;
        }

        protected virtual bool IsGamePrepped()
        {
            return _GameState != null && _GameState.ModelRef != null && AnswerId != null;
        }

        protected virtual bool IsGuessValid(BaseDbPoeModel? pGuessModel, int pGuessId)
        {
            if (pGuessModel == null) return false;
            if (_GameState == null) return false;
            if (_GameState.GuessedIds.Contains(pGuessId)) return false;
            return true;
        }

        protected bool IsGuessCorrect(int pGuessId)
        {
            if (AnswerId == null) return false;
            return AnswerId == pGuessId;
        }

        public abstract void PrepGame();
        public abstract BaseGuessResult? MakeGuess(int pGuessId);
        public abstract int? GetGuessId(string pGuess);
    }
}
