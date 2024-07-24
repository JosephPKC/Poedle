using System.Diagnostics;
using BaseToolsUtils.Logging;
using BaseToolsUtils.Logging.Writers;
using Poedle.Server.Shared.Answers;
using Poedle.Server.Shared.Results;
using Poedle.Server.Shared.State;
using Poedle.Server.Shared.Stats;
using Poedle.Server.UniqueByAttr.State;
using PoeWikiData;
using PoeWikiData.Models.UniqueItems;

namespace Poedle.Server
{
    internal sealed class GameManager
    {
        private static readonly GameManager _instance = new();
        public static GameManager Instance { get { return _instance; } }

        private readonly ConsoleLogger _log;
        private readonly PoeDbManager _db;
        private readonly UniqueItemDbLookUp _allUniqueItemModels;
        private readonly Dictionary<uint, FullAnswerModel> _allAnswers;
        private readonly Dictionary<uint, LiteAnswerModel> _allAnswersLite;
        private readonly Dictionary<GameTypes, BaseStateController> _gameStates;

        public enum GameTypes
        {
            UniqueByAttr
        }

        static GameManager() { }

        private GameManager()
        {
            _log = new(new ConsoleWriter());
            _db = new(Path.Combine(Environment.CurrentDirectory, @"..\DbData", "PoeDb.db"), false, _log);

            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: Getting all models to cache.");

            IEnumerable<UniqueItemDbModel> allModels = _db.GetAll<UniqueItemDbModel>(true);
            _allUniqueItemModels = new(allModels);

            _allAnswers = [];
            _allAnswersLite = [];
            foreach (UniqueItemDbModel model in allModels)
            {
                _allAnswers.Add(model.Id, AnswerModelMapper.GetFullAnswer(model));
                _allAnswersLite.Add(model.Id, AnswerModelMapper.GetLiteAnswer(model));
            }

            _log.TimeStopLogAndAppend(timer, $"END: Getting all models to cache.");

            _gameStates = [];
            _gameStates.Add(GameTypes.UniqueByAttr, new UniqueByAttrStateController(_allAnswers, _allAnswersLite, _allUniqueItemModels));
        }

        public IEnumerable<LiteAnswerModel> GetAllAvailableAnswers(GameTypes pGameType)
        {
            return _gameStates[pGameType].GetAllAvailableAnswers();
        }

        public FullAnswerModel GetChosenAnswer(GameTypes pGameType)
        {
            return _gameStates[pGameType].GetChosenAnswer();
        }

        public BaseResult ProcessResult(GameTypes pGameType, uint pGuessId)
        {
            return _gameStates[pGameType].ProcessResult(pGuessId);
        }

        //public IEnumerable<BaseResult> GetAllGuessResults(GameTypes pGameType)
        //{
        //    return _gameStates[pGameType].GetAllGuessResults();
        //}

        public void UpdateScore(GameTypes pGameType, int pScore)
        {
            _gameStates[pGameType].UpdateScore(pScore);
        }

        public StatsModel GetStats(GameTypes pGameType)
        {
            return _gameStates[pGameType].GetStats();
        }

        public void SetGame(GameTypes pGameType)
        {
            _gameStates[pGameType].SetGame();
        }
    }
}
