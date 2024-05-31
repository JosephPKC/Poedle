using Poedle.PoeDb;
using Poedle.Game.Controllers;
using Poedle.Utils.Logger;

namespace Poedle.Server.Game.Managers.Uniques
{
    public class FindUniqueByParamManager(PoeDbManager pDb, DebugLogger pLog)
    {
        private readonly PoeDbManager _db = pDb;
        private readonly DebugLogger _log = pLog;
        private readonly FindUniqueByParamController _controls = new(pDb, pLog);

        // Responsibilities:
        // Handle the controller and be the main interface between UI and the controls
        // Keep track of scores and maybe do fancy graphs
        // Determine the guess inputs
        // Accept guesses from UI
        // Return the params result/updates to UI
        // This means many of our functions need to be handlers for UI events

        public void StartGame()
        {
            _controls.PrepGame();
        }


    }
}
