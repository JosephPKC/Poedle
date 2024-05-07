using System.Text;

using Poedle.Enums;
using Poedle.Game.Mappers;
using Poedle.Game.Models.Params;
using Poedle.PoeDb;
using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;
using Poedle.Utils.Strings;
using static Poedle.Enums.MiscEnums;
using static Poedle.PoeDb.DbQueryParams;

namespace Poedle.Game.Controllers.Uniques
{
    public class FindUniqueByParamGameController(PoeDbManager pDb, DebugLogger pLog) : BaseGameController<DbUnique>(pDb, pLog), IIsUniqueGame
    {
        public struct GuessResult
        {
            public bool IsCorrect { get; set; }
            public UniqueParams Params { get; set; }
            public UniqueParamsResult Result { get; set; }
        }

        private readonly HashSet<int> _guessedIds = [];

        private UniqueParams? _params;

        public bool AreHintsEnabled { get; set; } = false;

        public void StartNewGame()
        {
            _log.Log("BEGIN: STARTING A NEW GAME.");
            _guessedIds.Clear();
            // Generate a random answer and set up params
            _modelRef = _db.Unique.GetRandom();
            AnswerId = _modelRef.Id;
            _params = UniqueParamsMapper.GetParams(_modelRef);
            _log.Log($"DEV: {_modelRef.Id} / {_modelRef.Name} / {_modelRef.PageName}");
            _log.Log($"DEV: {BuildParamString(_params, AreHintsEnabled)}");
            _log.Log("END: STARTING A NEW GAME.");
        }

        public void ToggleHints()
        {
            AreHintsEnabled = !AreHintsEnabled;
            _log.Log($"DEV: {_modelRef.Id} / {_modelRef.Name} / {_modelRef.PageName}");
            _log.Log($"DEV: {BuildParamString(_params, AreHintsEnabled)}");
        }

        public bool IsGuessCorrect(int pGuessId)
        {
            return pGuessId == AnswerId;
        }

        public GuessResult? MakeGuess(string pGuess)
        {
            if (_params == null)
            {
                throw new Exception("Start a new game first: StartNewGame().");
            }

            DbUnique fromDb = _db.Unique.GetByPageName(pGuess);
            if (fromDb == null)
            {
                return null;
            }

            return MakeGuess(fromDb.Id);
        }

        public GuessResult? MakeGuess(int pGuessId)
        {
            // Check if guess is correct
            // If yes, then you win!
            // Return result which is the params and their correctness.
            // Guess is wrong, but thats ok
            // Check if guess is valid first (maybe we take in the id instead, easier)
            // Id is auto incremented starting at 1, so it matches the count reliably.
            // We can check for correctness by comparing it to the collection length
            // Get the unique model from db by id
            // Get the params for the model
            // Compare the parms with the answer params
            // For each field, compare to determine if it is:
            // Correct - Complete Match
            // Wrong - Complete Mismatch
            // Partial - Partial Match (only for parameters that are lists)
            // Higher - Partial Match and answer is higher (only for integer params)
            // Lower - Partial Match and answer is lower (only for integer params)
            // Return the result - param correctness
            if (_params == null)
            {
                throw new Exception("Start a new game first: StartNewGame().");
            }

            if (IsGuessCorrect(pGuessId))
            {
                return new GuessResult()
                {
                    IsCorrect = true,
                    Params = _params,
                    Result = new()
                    {
                        BaseItem = ParamsResult.CORRECT,
                        LeaguesIntroduced = ParamsResult.CORRECT,
                        Qualities = ParamsResult.CORRECT,
                        DropSources = ParamsResult.CORRECT,
                        ReqLvl = ParamsResult.CORRECT, 
                        ReqDex = ParamsResult.CORRECT,
                        ReqInt = ParamsResult.CORRECT,
                        ReqStr = ParamsResult.CORRECT
                    }
                };
            }

            // Check validity
            DbUnique guessUnique = _db.Unique.GetById(pGuessId);
            if (guessUnique == null)
            {
                return null;
            }

            if (_guessedIds.Contains(pGuessId))
            {
                return null;
            }

            _guessedIds.Add(pGuessId);

            // Build out the params to determine hints
            UniqueParams guessParams = UniqueParamsMapper.GetParams(guessUnique);

            return new GuessResult()
            {
                IsCorrect = false,
                Params = guessParams,
                Result = CheckParams(guessParams)
            };
        }

        private UniqueParamsResult CheckParams(UniqueParams pGuess)
        {
            if (_params == null)
            {
                throw new Exception("Start a new game first: StartNewGame().");
            }

            return new UniqueParamsResult()
            {
                BaseItem = CheckTypes(pGuess, _params),
                LeaguesIntroduced = CheckListParam(pGuess.LeaguesIntroduced, _params.LeaguesIntroduced),
                Qualities = CheckListParam(pGuess.Qualities, _params.Qualities),
                DropSources = CheckListParam(pGuess.DropSources, _params.DropSources),
                DropSourcesSpecific = CheckListParam(pGuess.DropSourcesSpecific, _params.DropSourcesSpecific),
                ReqLvl = CheckNumberParam(pGuess.ReqLvl, _params.ReqLvl),
                ReqDex = CheckNumberParam(pGuess.ReqDex, _params.ReqDex),
                ReqInt = CheckNumberParam(pGuess.ReqInt, _params.ReqInt),
                ReqStr = CheckNumberParam(pGuess.ReqStr, _params.ReqStr),
            };
        }

        private static ParamsResult CheckTypes(UniqueParams pGuess, UniqueParams pAnswer) 
        {
            if (MiscStringUtils.EqualsIgnoreCase(pGuess.BaseItem, pAnswer.BaseItem))
            {
                return ParamsResult.CORRECT;
            }

            if (pGuess.ItemClass == pAnswer.ItemClass)
            {
                return ParamsResult.PARTIAL;
            }
            return ParamsResult.WRONG;
        }

        private static ParamsResult CheckListParam<T>(List<T> pGuessList, List<T> pAnswerList)
        {

            if (pGuessList.SequenceEqual(pAnswerList))
            {
                return ParamsResult.CORRECT;
            }

            return pGuessList.Intersect(pAnswerList).Any() ? ParamsResult.PARTIAL : ParamsResult.WRONG;
        }

        private static ParamsResult CheckNumberParam(uint pGuessNum, uint pAnswerNum)
        {
            if (pGuessNum == pAnswerNum)
            {
                return ParamsResult.CORRECT;
            }

            return pGuessNum > pAnswerNum ? ParamsResult.LOWER : ParamsResult.HIGHER;
        }

        public static string BuildParamString(UniqueParams pParams, bool pEnableHints)
        {

            StringBuilder builder = new();
            // Base Item
            builder.Append($"Base: {pParams.BaseItem} / ");
            // Leagues Introduced
            builder.Append($"Leagues: {GetListString(pParams.LeaguesIntroduced)} / ");
            // Req Stats
            builder.Append($"ReqLvl: {pParams.ReqLvl} / ");
            builder.Append($"ReqDex: {pParams.ReqDex} / ");
            builder.Append($"ReqInt: {pParams.ReqInt} / ");
            builder.Append($"ReqStr: {pParams.ReqStr} / ");
            // Qualities
            builder.Append($"Quals: {GetListString(pParams.Qualities)} / ");
            // Drops
            builder.Append($"Drops: {GetListString(pParams.DropSources)} / ");

            if (pEnableHints)
            {
                builder.Append($"Specifics: {GetListString(pParams.DropSourcesSpecific)} / ");
            }

            return builder.ToString();
        }

        public static string BuildResultString(UniqueParams pParams, UniqueParamsResult pResult, bool pEnableHints)
        {

            StringBuilder builder = new();
            // Base Item
            builder.Append($"Base: {pParams.BaseItem} ({GetParamResultStatus(pResult.BaseItem)}) / ");
            // Leagues Introduced
            builder.Append($"Leagues: {GetListString(pParams.LeaguesIntroduced)} ({GetParamResultStatus(pResult.LeaguesIntroduced)}) / ");
            // Req Stats
            builder.Append($"ReqLvl: {pParams.ReqLvl} ({GetParamResultStatus(pResult.ReqLvl)}) / ");
            builder.Append($"ReqDex: {pParams.ReqDex} ({GetParamResultStatus(pResult.ReqDex)}) / ");
            builder.Append($"ReqInt: {pParams.ReqInt} ({GetParamResultStatus(pResult.ReqInt)}) / ");
            builder.Append($"ReqStr: {pParams.ReqStr} ({GetParamResultStatus(pResult.ReqStr)}) / ");
            // Qualities
            builder.Append($"Quals: {GetListString(pParams.Qualities)} ({GetParamResultStatus(pResult.Qualities)}) / ");
            // Drops
            builder.Append($"Drops: {GetListString(pParams.DropSources)} ({GetParamResultStatus(pResult.DropSources)}) / ");

            if (pEnableHints)
            {
                builder.Append($"Specifics: {GetListString(pParams.DropSourcesSpecific)} ({GetParamResultStatus(pResult.DropSourcesSpecific)}) / ");
            }

            return builder.ToString();
        }

        private static string GetListString<T>(List<T> pList)
        {
            if (pList.Count == 0)
            {
                return "None";
            }
            return $"{string.Join(",", pList)}";
        }

        private static string GetParamResultStatus(ParamsResult pResult)
        {
            return $"{pResult.ToString()[0]}";
        }
    }
}
