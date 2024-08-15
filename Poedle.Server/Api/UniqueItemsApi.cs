using Microsoft.AspNetCore.Mvc;
using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints.ExpHints;
using Poedle.Server.Data.Results.UniqueItems;
using Poedle.Server.Data.Stats;
using Poedle.Server.States.UniqueItems;

namespace Poedle.Server.Api
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
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetAllAvailableAnswers);
        }
        #endregion

        #region "Hints"
        [HttpGet("Hints/All")]
        public AllHintsExpModel GetHints()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetHints);
        }

        #endregion

        #region "Stats"
        [HttpGet("Stats")]
        public StatsExpModel GetStats()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetStats);
        }

        [HttpPost("Stats/Clear")]
        public bool ClearStats()
        {
            ProcessApi(new HttpContextAccessor().HttpContext, _controller.ClearStats);
            return true;
        }
        #endregion

        #region "Results"
        [HttpGet("Results")]
        public IEnumerable<UniqueItemsResultExpModel> GetResults()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetResults);
        }

        [HttpPost("Results/Process/{guessId:int}")]
        public UniqueItemsResultExpModel ProcessResults(int guessId)
        {
            UniqueItemsResultExpModel ProcessGuess() => _controller.ProcessGuess((uint)guessId);
            return ProcessApi(new HttpContextAccessor().HttpContext, ProcessGuess);
        }
        #endregion

        #region "Game"
        [HttpGet("Game/IsWin")]
        public bool IsWin()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.IsWin);
        }

        [HttpPost("Game/Reset")]
        public bool SetGame()
        {
            ProcessApi(new HttpContextAccessor().HttpContext, _controller.SetGame);
            return true;
        }
        #endregion
    }
}
