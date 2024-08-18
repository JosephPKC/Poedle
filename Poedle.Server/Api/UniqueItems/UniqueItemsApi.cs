using Microsoft.AspNetCore.Mvc;
using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints.UniqueItems;
using Poedle.Server.Data.Results.UniqueItems;
using Poedle.Server.Data.Stats;
using Poedle.Server.States.UniqueItems;

namespace Poedle.Server.Api.UniqueItems
{
    [ApiController]
    [Route("Poedle/UniqueItems")]
    public class UniqueItemsApi : BaseApi
    {
        private readonly UniqueItemsStateController _controller = GameManager.Instance.UniqueItemsGame;

        #region "Answers"
        [HttpGet("Answers/AllAvailable")]
        public IEnumerable<AnswerExpModel> GetAllAvailableAnswers()
        {
            return ProcessApi(Url.RouteUrl(RouteData.Values), _controller.GetAllAvailableAnswers);
        }
        #endregion

        #region "Hints"
        [HttpGet("Hints/All")]
        public UniqueItemAllHintsExpModel GetHints()
        {
            return ProcessApi(Url.RouteUrl(RouteData.Values), _controller.GetHints);
        }
        #endregion

        #region "Stats"
        [HttpGet("Stats")]
        public StatsExpModel GetStats()
        {
            return ProcessApi(Url.RouteUrl(RouteData.Values), _controller.GetStats);
        }

        [HttpPost("Stats/Clear")]
        public bool ClearStats()
        {
            ProcessApi(Url.RouteUrl(RouteData.Values), _controller.ClearStats);
            return true;
        }
        #endregion

        #region "Results"
        [HttpGet("Results")]
        public IEnumerable<UniqueItemsResultExpModel> GetResults()
        {
            return ProcessApi(Url.RouteUrl(RouteData.Values), _controller.GetResults);
        }

        [HttpPost("Results/Process/{guessId:int}")]
        public UniqueItemsResultExpModel ProcessResults(int guessId)
        {
            UniqueItemsResultExpModel ProcessGuess() => _controller.ProcessGuess((uint)guessId);
            return ProcessApi(Url.RouteUrl(RouteData.Values), ProcessGuess);
        }
        #endregion

        #region "Game"
        [HttpGet("Game/IsWin")]
        public bool IsWin()
        {
            return ProcessApi(Url.RouteUrl(RouteData.Values), _controller.IsWin);
        }

        [HttpPost("Game/Reset")]
        public bool SetGame()
        {
            ProcessApi(Url.RouteUrl(RouteData.Values), _controller.SetGame);
            return true;
        }
        #endregion
    }
}
