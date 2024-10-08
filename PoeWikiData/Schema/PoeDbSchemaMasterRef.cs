﻿namespace PoeWikiData.Schema
{
    internal class PoeDbSchemaMasterRef
    {
        public static Dictionary<PoeDbSchemaTypes, PoeDbSchema> SchemaList { get; set; } = new() {
            {
                PoeDbSchemaTypes.DropSources, new PoeDbSchema()
                {
                    Table = "DropSources",
                    Columns =
                    [
                        "DropSourceId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "DisplayName TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.DropTypes, new PoeDbSchema()
                {
                    Table = "DropTypes",
                    Columns =
                    [
                        "DropTypeId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "DisplayName TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.GemTags, new PoeDbSchema()
                {
                    Table = "GemTags",
                    Columns =
                    [
                        "GemTagId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "DisplayName TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.ItemAspects, new PoeDbSchema()
                {
                    Table = "ItemAspects",
                    Columns =
                    [
                        "ItemAspectId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "DisplayName TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.ItemClasses, new PoeDbSchema()
                {
                    Table = "ItemClasses",
                    Columns =
                    [
                        "ItemClassId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "DisplayName TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.Leagues,  new PoeDbSchema()
                {
                    Table = "Leagues",
                    Columns =
                    [
                        "LeagueId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "DisplayName TEXT COLLATE NOCASE NOT NULL",
                        "ReleaseVersionMajor TEXT COLLATE NOCASE NOT NULL",
                        "ReleaseVersionMinor TEXT COLLATE NOCASE NOT NULL",
                        "ReleaseVersionPatch TEXT COLLATE NOCASE NOT NULL"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.SkillGems, new PoeDbSchema()
                {
                    Table = "SkillGems",
                    Columns =
                    [
                        "SkillGemId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "DisplayName TEXT COLLATE NOCASE NOT NULL",
                        "Description TEXT COLLATE NOCASE NOT NULL",
                        "PrimaryAttribute TEXT COLLATE NOCASE NOT NULL",
                        "DexterityPercent INTEGER NOT NULL",
                        "IntelligencePercent INTEGER NOT NULL",
                        "StrengthPercent INTEGER NOT NULL"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.SkillGems_GemTags, new PoeDbSchema()
                {
                    Table = "SkillGems_GemTags",
                    Columns =
                    [
                        "SkillGemId INTEGER REFERENCES SkillGems (SkillGemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "GemTagId INTEGER REFERENCES GemTags (GemTagId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "PRIMARY KEY (SkillGemId, GemTagId)"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.UniqueItems, new PoeDbSchema()
                {
                    Table = "UniqueItems",
                    Columns =
                    [
                        "UniqueItemId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL",
                        "Name TEXT COLLATE NOCASE NOT NULL",
                        "DisplayName TEXT COLLATE NOCASE NOT NULL",
                        "ItemClassId INTEGER NOT NULL",
                        "BaseItem TEXT COLLATE NOCASE NOT NULL",
                        "ReqLvl INTEGER NOT NULL",
                        "ReqDex INTEGER NOT NULL",
                        "ReqInt INTEGER NOT NULL",
                        "ReqStr INTEGER NOT NULL"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.UniqueItems_DropSources, new PoeDbSchema()
                {
                    Table = "UniqueItems_DropSources",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "DropSourceId INTEGER REFERENCES DropSources (DropSourceId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "PRIMARY KEY (UniqueItemId, DropSourceId)"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.UniqueItems_DropTypes, new PoeDbSchema()
                {
                    Table = "UniqueItems_DropTypes",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "DropTypeId INTEGER REFERENCES DropTypes (DropTypeId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "PRIMARY KEY (UniqueItemId, DropTypeId)"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.UniqueItems_ItemAspects, new PoeDbSchema()
                {
                    Table = "UniqueItems_ItemAspects",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "ItemAspectId INTEGER REFERENCES ItemAspects (ItemAspectId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "PRIMARY KEY (UniqueItemId, ItemAspectId)"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.UniqueItems_LeaguesIntroduced, new PoeDbSchema()
                {
                    Table = "UniqueItems_LeaguesIntroduced",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "LeagueId INTEGER REFERENCES Leagues (LeagueId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "PRIMARY KEY (UniqueItemId, LeagueId)"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.UniqueItems_FlavourTexts, new PoeDbSchema()
                {
                    Table = "UniqueItems_FlavourTexts",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "FlavourText TEXT COLLATE NOCASE NOT NULL",
                        "TextOrder INTEGER NOT NULL",
                        "PRIMARY KEY (UniqueItemId, FlavourText, TextOrder)"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.UniqueItems_ImplicitStatTexts, new PoeDbSchema()
                {
                    Table = "UniqueItems_ImplicitStatTexts",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "ImplicitStatText TEXT COLLATE NOCASE NOT NULL",
                        "TextOrder INTEGER NOT NULL",
                        "PRIMARY KEY (UniqueItemId, ImplicitStatText, TextOrder)"
                    ]
                }
            },
            {
                PoeDbSchemaTypes.UniqueItems_ExplicitStatTexts, new PoeDbSchema()
                {
                    Table = "UniqueItems_ExplicitStatTexts",
                    Columns =
                    [
                        "UniqueItemId INTEGER REFERENCES UniqueItems (UniqueItemId) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL",
                        "ExplicitStatText TEXT COLLATE NOCASE NOT NULL",
                        "TextOrder INTEGER NOT NULL",
                        "PRIMARY KEY (UniqueItemId, ExplicitStatText, TextOrder)"
                    ]
                }
            }
        };
    }
}