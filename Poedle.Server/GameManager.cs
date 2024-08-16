using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using BaseToolsUtils.Logging;
using BaseToolsUtils.Logging.Writers;
using Poedle.Server.Data.Answers;
using Poedle.Server.States.UniqueItems;
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

        public UniqueItemsStateController UniqueItemsGame { get; private set; }

        static GameManager() { }

        private GameManager()
        {
            _log = new(new ConsoleWriter());
            _db = new(Path.Combine(Environment.CurrentDirectory, @"..\DbData", "PoeDb.db"), false, _log);

            UniqueItemsGame = GetUniqueByAttrStateController();
        }

        private UniqueItemsStateController GetUniqueByAttrStateController()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: Get all unique items from db.");

            IEnumerable<UniqueItemDbModel> allModels = _db.GetAll<UniqueItemDbModel>(true);
            Dictionary<uint, AnswerExpModel> allAnswers = [];
            foreach (UniqueItemDbModel model in allModels)
            {
                allAnswers.Add(model.Id, AnswerMapper.GetAnswer(model, model.BaseItem));
            }

            UniqueItemDbLookUp uniqueItemLookUp = new(allModels);
            UniqueItemsStateController control = new(uniqueItemLookUp, allAnswers);

            _log.TimeStopLogAndAppend(timer, $"END: Get all unique items from db.");

            return control;
        }
    }
}
