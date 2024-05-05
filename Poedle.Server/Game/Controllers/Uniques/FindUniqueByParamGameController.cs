using System.Text;

using Poedle.Enums;
using Poedle.Game.Mappers;
using Poedle.Game.Models.Params;
using Poedle.PoeDb;
using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;

using static Poedle.Enums.MiscEnums;

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

        private HashSet<int> _guessedIds = [];

        public UniqueParams? Params { get; private set; }

        public void StartNewGame()
        {
            _log.Log("BEGIN: STARTING A NEW GAME.");
            _guessedIds.Clear();
            // Generate a random answer and set up params
            _modelRef = _db.Unique.GetRandom();
            AnswerId = _modelRef.Id;
            Params = UniqueParamsMapper.GetParams(_modelRef);
            _log.Log($"DEV: {_modelRef.Id} / {_modelRef.Name} / {_modelRef.PageName}");
            _log.Log($"DEV: {BuildParamString(Params)}");
            _log.Log("END: STARTING A NEW GAME.");
        }

        public bool IsGuessCorrect(int pGuessId)
        {
            return pGuessId == AnswerId;
        }

        public GuessResult? MakeGuess(string pGuess)
        {
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


            if (IsGuessCorrect(pGuessId))
            {
                return new GuessResult()
                {
                    IsCorrect = true,
                    Params = Params,
                    Result = new()
                    {
                        ItemClass = ParamsResult.CORRECT,
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
            return new UniqueParamsResult()
            {
                ItemClass = pGuess.ItemClass == Params.ItemClass ? ParamsResult.CORRECT : ParamsResult.WRONG,
                LeaguesIntroduced = CheckListParam(pGuess.LeaguesIntroduced, Params.LeaguesIntroduced),
                Qualities = CheckListParam(pGuess.Qualities, Params.Qualities),
                DropSources = CheckListParam(pGuess.DropSources, Params.DropSources),
                ReqLvl = CheckNumberParam(pGuess.ReqLvl, Params.ReqLvl),
                ReqDex = CheckNumberParam(pGuess.ReqDex, Params.ReqDex),
                ReqInt = CheckNumberParam(pGuess.ReqInt, Params.ReqInt),
                ReqStr = CheckNumberParam(pGuess.ReqStr, Params.ReqStr),
            };
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

        public static string BuildParamString(UniqueParams pParams)
        {

            StringBuilder builder = new();
            // Item Class
            builder.Append($"Class: {pParams.ItemClass} / ");
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

            return builder.ToString();
        }

        public static string BuildResultString(UniqueParams pParams, UniqueParamsResult pResult)
        {

            StringBuilder builder = new();
            // Item Class
            builder.Append($"Class: {pParams.ItemClass} ({GetParamResultStatus(pResult.ItemClass)}) / ");
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
