using Poedle.PoeDb;
using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;

namespace Poedle.Game.Controllers
{
    public abstract class BaseGameController<T>(PoeDbManager pDb, DebugLogger pLog) where T : BaseDbModel
    {
        protected readonly PoeDbManager _db = pDb;
        protected readonly DebugLogger _log = pLog;

        protected T? _modelRef;

        public int AnswerId { get; protected set; }
    }
}
