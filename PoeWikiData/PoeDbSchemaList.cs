namespace PoeWikiData
{
    internal static class PoeDbSchemaList
    {
        public enum PoeDbTypes
        {
            None,
            DropSources,
            DropTypes,
            ItemAspects,
            ItemClasses,
            Leagues,
            UniqueItems,
            UniqueItems_DropSources,
            UniqueItems_DropTypes,
            UniqueItems_ItemAspects,
            UniqueItems_LeaguesIntroduced
        }

        public static Dictionary<PoeDbTypes, PoeDbSchema> SchemaList { get; set; } = new() {
            {
                PoeDbTypes.DropSources, new PoeDbSchema()
                {
                    TableName = "DropSources",
                    Columns =
                    [
                        "DropSourceId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbTypes.DropTypes, new PoeDbSchema()
                {
                    TableName = "DropTypes",
                    Columns =
                    [
                        "DropTypeId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbTypes.ItemAspects, new PoeDbSchema()
                {
                    TableName = "ItemAspects",
                    Columns =
                    [
                        "ItemAspectId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbTypes.ItemClasses, new PoeDbSchema()
                {
                    TableName = "ItemClasses",
                    Columns =
                    [
                        "ItemClassId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            { 
                PoeDbTypes.Leagues,  new PoeDbSchema() 
                {
                    TableName = "Leagues",
                    Columns = 
                    [
                        "LeagueId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "ReleaseVersionMajor INTEGER NOT NULL",
                        "ReleaseVersionMinor INTEGER NOT NULL",
                        "ReleaseVersionPatch INTEGER NOT NULL"
                    ]
                }
            },
            {
                PoeDbTypes.UniqueItems, new PoeDbSchema()
                {
                    TableName = "UniqueItems",
                    Columns = 
                    [
                        "UniqueItemId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "ItemClassId INTEGER REFERENCES ItemClasses (ItemClassId) ON DELETE CASCADE ON UPDATE CASCADE",
                        "BaseItem TEXT COLLATE NOCASE NOT NULL",
                        "ReqLvl INTEGER NOT NULL",
                        "ReqDex INTEGER NOT NULL",
                        "ReqInt INTEGER NOT NULL",
                        "ReqStr INTEGER NOT NULL"
                    ]
                }
            },
            {
                PoeDbTypes.UniqueItems_DropSources, new PoeDbSchema()
                {
                    TableName = "UniqueItems_DropSources",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "DropSourceId INTEGER REFERENCES DropSources (DropSourceId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                    ]
                }
            },
            {
                PoeDbTypes.UniqueItems_DropTypes, new PoeDbSchema()
                {
                    TableName = "UniqueItems_DropTypes",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "DropTypeId INTEGER REFERENCES DropTypes (DropTypesId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                    ]
                }
            },
            {
                PoeDbTypes.UniqueItems_ItemAspects, new PoeDbSchema()
                {
                    TableName = "UniqueItems_ItemAspects",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "ItemAspectId INTEGER REFERENCES ItemAspects (ItemAspectId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                    ]
                }
            },
            {
                PoeDbTypes.UniqueItems_LeaguesIntroduced, new PoeDbSchema()
                {
                    TableName = "UniqueItems_LeaguesIntroduced",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "LeagueId INTEGER REFERENCES Leagues (LeagueId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                    ]
                }
            }
        };
    }

    internal class PoeDbSchema
    {
        public string TableName { get; set; } = "";
        public List<string> Columns { get; set; } = [];
    }
}
