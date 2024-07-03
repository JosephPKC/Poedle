using Microsoft.AspNetCore.Mvc;
using PoeWikiData;
using PoeWikiData.Models;

namespace Poedle.Server.ByAttr
{
    [ApiController]
    [Route("poedle/byattr")]
    public class ByAttrGameStateController : ControllerBase
    {
        private ByAttrGameState _state;
        private readonly ILogger<ByAttrGameStateController> _logger;
        // link to a db manager to retrieve ddl
        //private readonly PoeDbManager _manager = PoeDbManager;

        public ByAttrGameStateController(ILogger<ByAttrGameStateController> logger)
        {
            _state = new();
            _logger = logger;
        }

        //#region "Gets"
        //[HttpGet("AllItemsCondensed")]
        //public IEnumerable<ItemCondensed> GetAllItemsCondensed()
        //{
        //    return _manager.SelectAllItemsCondensed();
        //}

        //[HttpGet("AllItemAttributes")]
        //public IEnumerable<UniqueItemDataModel> GetAllItemAttributes()
        //{
        //    return _manager.SelectAllItems();
        //}

        //[HttpGet("CorrectAnswer")]
        //public ItemCondensed GetCorrectAnswer()
        //{
        //    return _manager.GetRandom();
        //}

        //[HttpGet("Hints")]
        //public IEnumerable<string> GetHints()
        //{
        //    return ["TEST1", "HINT"];
        //}

        //[HttpGet("Guess/{guessId:int}/{answerId:int}")]
        //public GuessResult GetGuessResult(int guessId, int answerId)
        //{
        //    // in the actual thing, these will be db manager calls to get the actual data model from id
        //    UniqueItemDataModel actual = _manager.GetItem(answerId);
        //    UniqueItemDataModel guess = _manager.GetItem(guessId);
        //    Console.WriteLine($"ReqLvl {guess.ReqLvl} vs. Actual {actual.ReqLvl} -> {compareNumbers(guess.ReqLvl, actual.ReqLvl)}");
        //    return new()
        //    {
        //        Attributes = new()
        //        {
        //            Name = guess.Name,
        //            ItemClass = guess.ItemClass.ToString(),
        //            BaseItem = guess.BaseItem,
        //            LeaguesIntroduced = string.Join(",", guess.LeaguesIntroduced),
        //            ItemAspects = string.Join(",", guess.ItemAspects),
        //            DropTypes = string.Join(",", guess.DropTypes),
        //            DropSources = string.Join(",", guess.DropSources),
        //            ReqLvl = guess.ReqLvl.ToString(),
        //            ReqDex = guess.ReqDex.ToString(),
        //            ReqInt = guess.ReqInt.ToString(),
        //            ReqStr = guess.ReqStr.ToString()
        //        },
        //        Results = new()
        //        {
        //            Name = compareStrings(guess.Name, actual.Name),
        //            ItemClass = compareEnumValues((int)guess.ItemClass, (int)actual.ItemClass),
        //            BaseItem = compareStrings(guess.BaseItem, actual.BaseItem),
        //            LeaguesIntroduced = compareLists(guess.LeaguesIntroduced, actual.LeaguesIntroduced),
        //            ItemAspects = compareLists(guess.ItemAspects, actual.ItemAspects),
        //            DropTypes = compareLists(guess.DropTypes, actual.DropTypes),
        //            DropSources = compareLists(guess.DropSources, actual.DropSources),
        //            ReqLvl = compareNumbers(guess.ReqLvl, actual.ReqLvl),
        //            ReqDex = compareNumbers(guess.ReqDex, actual.ReqDex),
        //            ReqInt = compareNumbers(guess.ReqInt, actual.ReqInt),
        //            ReqStr = compareNumbers(guess.ReqStr, actual.ReqStr)
        //        }
        //    };
        //}

        //[HttpGet("Score")]
        //public uint GetScore()
        //{
        //    return _state.Score;
        //}
        //#endregion

        //#region "Posts"
        //[HttpPost("UpdateScore")]
        //public void UpdateScore()
        //{
        //    // nothing for now
        //}

        //private ResultStates compareEnumValues(int guess, int actual)
        //{

        //    return guess == actual ? ResultStates.CORRECT : ResultStates.WRONG;
        //}

        //private ResultStates compareStrings(string guess, string actual)
        //{

        //    return guess == actual ? ResultStates.CORRECT : ResultStates.WRONG;
        //}

        //private ResultStates compareLists<T>(List<T> guess, List<T> actual)
        //{
        //    if (guess.SequenceEqual(actual)) return ResultStates.CORRECT;

        //    return guess.Intersect(actual).Any() ? ResultStates.PARTIAL : ResultStates.WRONG;
        //}

        //private ResultStates compareNumbers(ushort guess, ushort actual)
        //{
        //    if (guess == actual) return ResultStates.CORRECT;

        //    return Math.Abs(guess - actual) <= 20 ? ResultStates.PARTIAL : ResultStates.WRONG;
        //}
        //#endregion
    }
}
