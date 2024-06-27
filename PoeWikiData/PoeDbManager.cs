using System.Data.SQLite;

using BaseToolsUtils.Logging;
using PoeWikiData.Endpoints;
using PoeWikiData.Models;
using PoeWikiData.Models.Enums;
using PoeWikiData.Utils;

namespace PoeWikiData
{
    public sealed class PoeDbManager
    {
        private readonly SQLiteConnection _sqlite;
        private readonly PoeDbCache _cache;

        #region "Endpoints"
        public LeagueEndpoint Leagues { get; private set; }
        public UniqueItemEndpoint Uniques { get; private set; }
        #endregion

        public PoeDbManager(string pDbFilePath, bool pIsNewDb, ConsoleLogger pLogger)
        {
            _sqlite = new SQLiteConnection($"Data Source={pDbFilePath};New={pIsNewDb};");
            _cache = new();

            Leagues = new(_sqlite, pLogger);
            Uniques = new(_sqlite, pLogger);
        }





        public List<ItemCondensed> SelectAllItemsCondensed()
        {
            return new()
            {
                new ItemCondensed()
                {
                    Label = "Abhorent Interrogation",
                    Value = 1
                },
                new ItemCondensed()
                {
                    Label = "Actum",
                    Value = 2
                },
                new ItemCondensed()
                {
                    Label = "Belt of the Deceiver",
                    Value = 3
                },
                new ItemCondensed()
                {
                    Label = "Ulaman's Gaze",
                    Value = 4
                }
            };
        }

        public List<UniqueItemModel> SelectAllItems()
        {
            return new()
            {
                new UniqueItemModel()
                {
                    Id = 1,
                    ItemClass = ItemClasses.Gloves,
                    BaseItem = "ambush mitts",
                    LeaguesIntroduced = ["harvest"],
                    ReqLvl = 45,
                    ReqDex = 35,
                    ReqInt = 35,
                    ReqStr = 0,
                    ItemAspects = [],
                    DropTypes = [],
                    DropSources = [],
                    Name = "Abhorent Interrogation"
                },
                new UniqueItemModel()
                {
                    Id = 2,
                    ItemClass = ItemClasses.OneHandedAxe,
                    BaseItem = "butcher axe",
                    LeaguesIntroduced = ["heist"],
                    ReqLvl = 63,
                    ReqDex = 76,
                    ReqInt = 0,
                    ReqStr = 149,
                    ItemAspects = [],
                    DropTypes = [DropTypes.SpecialDropArea],
                    DropSources = [DropSources.CurioDisplay],
                    Name = "Actum"
                },
                 new UniqueItemModel()
                {
                    Id = 3,
                    ItemClass = ItemClasses.Belt,
                    BaseItem = "heavy belt",
                    LeaguesIntroduced = ["bloodlines", "torment"],
                    ReqLvl = 20,
                    ReqDex = 0,
                    ReqInt = 0,
                    ReqStr = 0,
                    ItemAspects = [],
                    DropTypes = [],
                    DropSources = [],
                    Name = "Belt of the Deceiver"
                },
                new UniqueItemModel()
                {
                    Id = 4,
                    ItemClass = ItemClasses.AbyssJewel,
                    BaseItem = "searching eye jewel",
                    LeaguesIntroduced = ["ultimatum"],
                    ReqLvl = 40,
                    ReqDex = 0,
                    ReqInt = 0,
                    ReqStr = 0,
                    ItemAspects = [],
                    DropTypes = [],
                    DropSources = [],
                    Name = "Ulaman's Gaze"
                }
            };
        }

        public ItemCondensed GetRandom()
        {
            Random rnd = new();
            var items = SelectAllItems();
            var itemsC = SelectAllItemsCondensed();
            int r = rnd.Next(0, items.Count - 1);
            return itemsC[r];
        }

        public UniqueItemModel GetItem(int id)
        {
            var items = SelectAllItems();
            return items[id - 1];
        }
    }
}
