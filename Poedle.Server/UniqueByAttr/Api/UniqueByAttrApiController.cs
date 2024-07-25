using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BaseToolsUtils.Logging;
using BaseToolsUtils.Logging.Writers;
using Poedle.Server.UniqueByAttr.Results;
using Poedle.Server.Shared.Answers;
using Poedle.Server.Shared.Stats;
using PoeWikiData.Models.Common;

namespace Poedle.Server.UniqueByAttr.Api
{
    [ApiController]
    [Route("Poedle/UniqueByAttr")]
    public class UniqueByAttrApiController
    {
        private readonly ConsoleLogger _log = new(new ConsoleWriter());

        [HttpGet("AllAvailableAnswers")]
        public IEnumerable<LiteAnswerModel> GetAllAvailableAnswers()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, "BEGIN: /Poedle/UniqueByAttr/AllAvailableAnswers");

            IEnumerable<LiteAnswerModel> allAnswers = GameManager.Instance.GetAllAvailableAnswers(GameManager.GameTypes.UniqueByAttr);

            _log.TimeStopLogAndAppend(timer, "END: /Poedle/UniqueByAttr/AllAvailableAnswers");

            return allAnswers;
        }

        [HttpGet("CorrectAnswer")]
        public FullAnswerModel GetCorrectAnswer()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, "BEGIN: /Poedle/UniqueUniqueByAttrItems/CorrectAnswer");

            FullAnswerModel chosenAnswer = GameManager.Instance.GetChosenAnswer(GameManager.GameTypes.UniqueByAttr);
            LiteAnswerModel liteAnswer = AnswerModelMapper.CondenseAnswer(chosenAnswer);
            Console.WriteLine($"Answer: {liteAnswer.Label}");
            _log.TimeStopLogAndAppend(timer, "END: /Poedle/UniqueByAttr/CorrectAnswer");

            return chosenAnswer;
        }

        [HttpGet("Hints")]
        public string GetHints()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, "BEGIN: /Poedle/UniqueByAttr/Hints");

            string hint = GameManager.Instance.GetHint(GameManager.GameTypes.UniqueByAttr);
            Console.WriteLine($"Hint: {hint}");

            _log.TimeStopLogAndAppend(timer, "END: /Poedle/UniqueByAttr/Hints");

            return hint;
        }

        [HttpPost("Guess/{guessId:int}")]
        public UniqueByAttrResult ProcessResult(int guessId)
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: /Poedle/UniqueByAttr/Guess/{guessId}");

            UniqueByAttrResult result = (UniqueByAttrResult)GameManager.Instance.ProcessResult(GameManager.GameTypes.UniqueByAttr, (uint)guessId);

            _log.TimeStopLogAndAppend(timer, $"END: /Poedle/UniqueByAttr/Guess/{guessId}");

            return result;
        }

        [HttpGet("Guess/AllResults")]
        public IEnumerable<UniqueByAttrResult> GetAllResults()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, "BEGIN: /Poedle/UniqueByAttr/Guess/AllResults");

            IEnumerable<UniqueByAttrResult> result = (IEnumerable<UniqueByAttrResult>)GameManager.Instance.GetAllGuessResults(GameManager.GameTypes.UniqueByAttr);

            _log.TimeStopLogAndAppend(timer, "END: /Poedle/UniqueByAttr/Guess/AllResults");

            return result;
        }

        //[HttpPost("Score/Update/{score:int}")]
        //public void UpdateScore(int score)
        //{
        //    Stopwatch timer = new();
        //    _log.TimeStartLog(timer, $"BEGIN: /Poedle/UniqueByAttr/Score/Update/{score}");

        //    GameManager.Instance.UpdateScore(GameManager.GameTypes.UniqueByAttr, score);

        //    _log.TimeStopLogAndAppend(timer, $"END: /Poedle/UniqueByAttr/Score/Update/{score}");
        //}

        [HttpPost("Score/Update")]
        public void UpdateScore()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: /Poedle/UniqueByAttr/Score/Update");

            GameManager.Instance.UpdateScore(GameManager.GameTypes.UniqueByAttr);

            _log.TimeStopLogAndAppend(timer, $"END: /Poedle/UniqueByAttr/Score/Update");
        }

        [HttpGet("Score")]
        public int GetScore()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: /Poedle/UniqueByAttr/Score");

            int score = GameManager.Instance.GetScore(GameManager.GameTypes.UniqueByAttr);

            _log.TimeStopLogAndAppend(timer, $"END: /Poedle/UniqueByAttr/Score");

            return score;
        }

        [HttpGet("Score/Stats")]
        public StatsModel GetStats()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: /Poedle/UniqueByAttr/Score/Stats");

            StatsModel stats = GameManager.Instance.GetStats(GameManager.GameTypes.UniqueByAttr);
            Console.WriteLine($"Total Games: {stats.TotalGames}");
            Console.WriteLine($"Best Score: {stats.BestScore} / {stats.BestAnswers}");
            Console.WriteLine($"Worst Score: {stats.WorstScore} / {stats.WorstAnswers}");
            Console.WriteLine($"Top/Bottom: {stats.TopAnswers} / {stats.BottomAnswers}");
            Console.WriteLine($"Average: {stats.TotalAverage} / {string.Join(", ", stats.AveragesPerAnswer)}");

            _log.TimeStopLogAndAppend(timer, $"END: /Poedle/UniqueByAttr/Score/Stats");

            return stats;
        }

        [HttpPost("Game/Set")]
        public void SetGame()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, "BEGIN: /Poedle/UniqueByAttr/Game/Set");

            GameManager.Instance.SetGame(GameManager.GameTypes.UniqueByAttr);

            _log.TimeStopLogAndAppend(timer, "END: /Poedle/UniqueByAttr/Game/Set");
        }

        [HttpPost("Game/SetIsWin/{guessId:int}")]
        public bool SetIsWin(int guessId)
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: /Poedle/UniqueByAttr/Game/SetIsWin/{guessId}");

            bool isWin = GameManager.Instance.SetIsWin(GameManager.GameTypes.UniqueByAttr, (uint)guessId);

            _log.TimeStopLogAndAppend(timer, $"END: /Poedle/UniqueByAttr/Game/SetIsWin/{guessId}");

            return isWin;
        }

        [HttpGet("Game/IsWin")]
        public bool IsWin()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: /Poedle/UniqueByAttr/Game/IsWin");

            bool isWin = GameManager.Instance.IsWin(GameManager.GameTypes.UniqueByAttr);

            _log.TimeStopLogAndAppend(timer, $"END: /Poedle/UniqueByAttr/Game/IsWin");
            Console.WriteLine("Is WIn: " + isWin);
            return isWin;
        }
    }
}
