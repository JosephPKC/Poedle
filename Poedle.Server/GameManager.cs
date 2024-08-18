using System.Diagnostics;

using BaseToolsUtils.Logging;
using BaseToolsUtils.Logging.Writers;
using Poedle.Server.Data.Answers;
using Poedle.Server.States.SkillGems;
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

        public SkillGemsStateController SkillGemsGame { get; private set; }
        public UniqueItemsStateController UniqueItemsGame { get; private set; }

        static GameManager() { }

        private GameManager()
        {
            _log = new(new ConsoleWriter());
            _db = new(Path.Combine(Environment.CurrentDirectory, @"..\DbData", "PoeDb.db"), false, _log);

            SkillGemsGame = GetSkillGemsStateController();
            UniqueItemsGame = GetUniqueItemsStateController();
        }

        private SkillGemsStateController GetSkillGemsStateController()
        {
            Stopwatch timer = new();
            _log.TimeStartLog(timer, $"BEGIN: Get all skill gems from db.");

            IEnumerable<SkillGemDbModel> allModels = _db.GetAll<SkillGemDbModel>(true);
            Dictionary<uint, AnswerExpModel> allAnswers = [];
            foreach (SkillGemDbModel model in allModels)
            {
                allAnswers.Add(model.Id, AnswerMapper.GetAnswer(model));
            }

            SkillGemDbLookup uniqueItemLookUp = new(allModels);
            SkillGemsStateController control = new(uniqueItemLookUp, allAnswers);

            _log.TimeStopLogAndAppend(timer, $"END: Get all skill gems from db.");

            return control;
        }

        private UniqueItemsStateController GetUniqueItemsStateController()
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
