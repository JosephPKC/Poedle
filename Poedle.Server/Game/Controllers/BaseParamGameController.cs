using Poedle.Game.Models.Games;
using Poedle.Game.Models.Params;
using Poedle.Game.Utils;
using Poedle.PoeDb;
using Poedle.Utils.Logger;

namespace Poedle.Game.Controllers
{
    public abstract class BaseParamGameController(PoeDbManager pDb, DebugLogger pLog) : BaseGameController(pDb, pLog)
    {
        protected BaseParamGame? GameState
        {
            get
            {
                return CastGameState<BaseParamGame>();
            }
        }

        public override void ToggleHints()
        {
            base.ToggleHints();
            if (GameState?.ModelRef == null) return;
            if (GameState?.CorrectParams == null) return;

            _log.Log($"DEV: {GameState.ModelRef.Id} / {GameState.ModelRef.Name} / {GameState.ModelRef.PageName}");
            _log.Log($"DEV: {ParamsStringBuilder.BuildParamString((UniqueParams)GameState.CorrectParams, GameState.AreHintsEnabled)}");
        }

        protected override bool IsGamePrepped()
        {
            return base.IsGamePrepped() && GameState != null && GameState.CorrectParams != null;
        }
    }
}
