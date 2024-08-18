using Microsoft.AspNetCore.Mvc;

using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Hints.SkillGems;
using Poedle.Server.Data.Results.SkillGems;
using Poedle.Server.Data.Stats;
using Poedle.Server.States.SkillGems;

namespace Poedle.Server.Api.SkillGems
{
    [ApiController]
    [Route("Poedle/SkillGems")]
    public class SkillGemsApi : BaseApi
    {
        private readonly SkillGemsStateController _controller = GameManager.Instance.SkillGemsGame;

        #region "Answers"
        [HttpGet("Answers/AllAvailable")]
        public IEnumerable<AnswerExpModel> GetAllAvailableAnswers()
        {
            return ProcessApi(Url.RouteUrl(RouteData.Values), _controller.GetAllAvailableAnswers);
        }
        #endregion

        #region "Hints"
        [HttpGet("Hints/All")]
        public SkillGemAllHintsExpModel GetHints()
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
        public IEnumerable<SkillGemsResultExpModel> GetResults()
        {
            return ProcessApi(Url.RouteUrl(RouteData.Values), _controller.GetResults);
        }

        [HttpPost("Results/Process/{guessId:int}")]
        public SkillGemsResultExpModel ProcessResults(int guessId)
        {
            SkillGemsResultExpModel ProcessGuess() => _controller.ProcessGuess((uint)guessId);
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
