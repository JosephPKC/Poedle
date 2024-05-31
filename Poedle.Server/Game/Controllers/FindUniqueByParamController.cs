using Poedle.Game.Mappers;
using Poedle.Game.Models.Games;
using Poedle.Game.Models.GuessResults;
using Poedle.Game.Models.Params;
using Poedle.Game.Utils;
using Poedle.PoeDb;
using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;

namespace Poedle.Game.Controllers
{
    public class FindUniqueByParamController : BaseParamGameController
    {

        public FindUniqueByParamController(PoeDbManager pDb, DebugLogger pLog) : base(pDb, pLog)
        {
            _GameState = new FindUniqueByParam();
        }

        protected new FindUniqueByParam GameState
        {
            get
            {
                return CastGameState<FindUniqueByParam>() ?? new FindUniqueByParam();
            }
        }

        public override void PrepGame()
        {
            _log.Log("BEGIN: STARTING A NEW GAME.");
            GameState.GuessedIds.Clear();
            GameState.Score = 0;
            GameState.GuessedParams.Clear();
            // Generate a random answer and set up params
            GameState.ModelRef = _db.Unique.GetRandom();
            if (GameState.ModelRef == null)
            {
                throw new Exception("Failed to PrepGame: Could not get a random model.");
            }

            GameState.CorrectParams = UniqueParamsMapper.GetParams((DbUnique)GameState.ModelRef);
            _log.Log($"DEV: {GameState.ModelRef.Id} / {GameState.ModelRef.Name} / {GameState.ModelRef.PageName}");
            _log.Log($"DEV: {ParamsStringBuilder.BuildParamString((UniqueParams)GameState.CorrectParams, GameState.AreHintsEnabled)}");
            _log.Log("END: STARTING A NEW GAME.");
        }

        public override int? GetGuessId(string pGuess)
        {
            DbUnique? fromDb = _db.Unique.GetByPageName(pGuess);
            return fromDb?.Id;
        }

        public override BaseGuessResult? MakeGuess(int pGuessId)
        {
            if (!IsGamePrepped())
            {
                throw new Exception("Start a new game first: PrepGame().");
            }

            GameState.Score++;
            if (IsGuessCorrect(pGuessId))
            {
                return new GuessUniqueByParamGuessResult()
                {
                    IsCorrect = true,
                    Params = (UniqueParams?)GameState.CorrectParams,
                    Result = new()
                };
            }

            // Check validity
            DbUnique? guessUnique = _db.Unique.GetById(pGuessId);
            if (!IsGuessValid(guessUnique, pGuessId))
            {
                return null;
            }

            GameState.GuessedIds.Add(pGuessId);

            // Build out the params to determine hints
            UniqueParams guessParams = UniqueParamsMapper.GetParams(guessUnique);
            GameState.GuessedParams.Add(guessParams);
            return new GuessUniqueByParamGuessResult()
            {
                IsCorrect = false,
                Params = guessParams,
                Result = CheckParams(guessParams)
            };
        }

        private UniqueParamsResult CheckParams(UniqueParams pGuess)
        {
            if (GameState.CorrectParams == null)
            {
                throw new Exception("Start a new game first: StartNewGame().");
            }

            UniqueParams param = (UniqueParams)GameState.CorrectParams;
            return new UniqueParamsResult()
            {
                BaseItem = ParamsChecker.CheckTypes(pGuess, param),
                LeaguesIntroduced = ParamsChecker.CheckListParam(pGuess.LeaguesIntroduced, param.LeaguesIntroduced),
                Qualities = ParamsChecker.CheckListParam(pGuess.Qualities, param.Qualities),
                DropSources = ParamsChecker.CheckListParam(pGuess.DropSources, param.DropSources),
                DropSourcesSpecific = ParamsChecker.CheckListParam(pGuess.DropSourcesSpecific, param.DropSourcesSpecific),
                ReqLvl = ParamsChecker.CheckNumberParam(pGuess.ReqLvl, param.ReqLvl),
                ReqDex = ParamsChecker.CheckNumberParam(pGuess.ReqDex, param.ReqDex),
                ReqInt = ParamsChecker.CheckNumberParam(pGuess.ReqInt, param.ReqInt),
                ReqStr = ParamsChecker.CheckNumberParam(pGuess.ReqStr, param.ReqStr),
            };
        }       
    }
}
