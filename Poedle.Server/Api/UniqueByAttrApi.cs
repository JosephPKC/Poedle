using Microsoft.AspNetCore.Mvc;
using Poedle.Server.Data.Answers;
using Poedle.Server.Data.Results.UniqueByAttr;
using Poedle.Server.Data.Stats;
using Poedle.Server.States;

namespace Poedle.Server.Api
{
    [ApiController]
    [Route("Poedle/UniqueByAttr")]
    public class UniqueByAttrApi : BaseApi
    {
        private readonly UniqueByAttrStateController _controller = GameManager.Instance.UniqueByAttr;

        #region "Answers"
        [HttpGet("Answers/AllAvailable")]
        public IEnumerable<AnswerExpModel> GetAllAvailableAnswers()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetAllAvailableAnswers);
        }

        [HttpGet("Answers/Chosen")]
        public AnswerExpModel GetChosenAnswer()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetChosenAnswer);
        }
        #endregion

        #region "Hints"
        [HttpGet("Hints/Display")]
        public IEnumerable<string> GetDisplayHints()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetDisplayHints);
        }

        [HttpGet("Hints/Name")]
        public string GetNameHints()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetNameHint);
        }

        [HttpGet("Hints/NbrGuessRemaining")]
        public int GetNumberfOGuessesForHint()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetNumberOfGuessesForHint);
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
        public IEnumerable<UniqueByAttrResultExpModel> GetResults()
        {
            return ProcessApi(new HttpContextAccessor().HttpContext, _controller.GetResults);
        }

        [HttpPost("Results/Process/{guessId:int}")]
        public UniqueByAttrResultExpModel ProcessResults(int guessId)
        {
            UniqueByAttrResultExpModel ProcessGuess() => _controller.ProcessGuess((uint)guessId);
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
