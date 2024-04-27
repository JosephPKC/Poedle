using System.Diagnostics;

using Flurl;

using DebugLogger;
using PoeWikiApi.Models;
using PoeWikiApi.Utils;

namespace PoeWikiApi
{
    internal class PoeWikiApi
    {
        private static readonly long _cacheSizeLimit = 1024;
        private static readonly string _uriBase = @"https://www.poewiki.net/w/index.php";
        private static readonly string _cargoTitle = "Special:CargoExport";

        private readonly HttpRetriever _http;
        private readonly CacheHandler<string, string> _cache;
        private readonly DebugLogger.DebugLogger _logger;

        #region "Cargo Model"
        private enum CargoModelTypes
        {
            NONE,
            UNIQUES,
            SKILLGEMS,
            PASSIVES
        }

        private struct CargoModel
        {
            public string Tables { get; set; }
            public string Fields { get; set; }
            public string Where { get; set; }
            public string Join { get; set; }
        }

        private static readonly Dictionary<CargoModelTypes, CargoModel> _cargoModelMap = new()
        {
            { CargoModelTypes.UNIQUES, new CargoModel() {
                Tables = "items",
                Fields = "_ID,name,class,base_item,influences,flavour_text,drop_monsters,acquisition_tags,release_version,removal_version,required_level,required_dexterity,required_intelligence,required_strength,is_corrupted,is_eater_of_worlds_item,is_searing_exarch_item,is_fractured,is_replica,is_synthesised,is_unmodifiable,is_veiled,stat_text",
                Where = "rarity_id=\"Unique\""
            }},
            { CargoModelTypes.SKILLGEMS, new CargoModel()
            {
                Tables = "skill_gems",
                Fields = "_ID,_pageName,gem_description,primary_attribute,dexterity_percent,intelligence_percent,strength_percent,is_awakened_support_gem,is_vaal_skill_gem,gem_tags",
                Where = "_pageName NOT LIKE \"Template:%\""
            }},
            { CargoModelTypes.PASSIVES, new CargoModel()
            {
                Tables = "passive_skills",
                Fields = "_ID,id,name,flavour_text,ascendancy_class,is_keystone,is_notable,stat_text",
                Where = "is_notable=true OR is_keystone=true"
            }}
        };
        #endregion

        public PoeWikiApi()
        {
            _http = new();
            _cache = new(_cacheSizeLimit);
            _logger = new(new DebugLogger.Writer.ConsoleWrapper());
        }

        public PoeWikiApi(DebugLogger.DebugLogger pLogger)
        {
            _http = new();
            _cache = new(_cacheSizeLimit);
            _logger = pLogger;
        }

        #region "Unique Items"
        public PoeWikiUnique? GetUniqueItemByName(string pName)
        {
            return GetFirstDataModel<PoeWikiUnique>(_cargoTitle, _cargoModelMap[CargoModelTypes.UNIQUES].Tables, _cargoModelMap[CargoModelTypes.UNIQUES].Fields, $"{_cargoModelMap[CargoModelTypes.UNIQUES].Where} AND name=\"{pName}\"");
        }

        public PoeWikiUnique? GetUniqueItemById(uint pId)
        {
            return GetFirstDataModel<PoeWikiUnique>(_cargoTitle, _cargoModelMap[CargoModelTypes.UNIQUES].Tables, _cargoModelMap[CargoModelTypes.UNIQUES].Fields, $"{_cargoModelMap[CargoModelTypes.UNIQUES].Where} AND _ID=\"{pId}\"");
        }

        public List<PoeWikiUnique> GetAllUniqueItems()
        {
            return GetListWithBatching<PoeWikiUnique>(_cargoTitle, _cargoModelMap[CargoModelTypes.UNIQUES].Tables, _cargoModelMap[CargoModelTypes.UNIQUES].Fields, _cargoModelMap[CargoModelTypes.UNIQUES].Where, 250, 0);
        }
        #endregion

        #region "Skill Gems"
        public PoeWikiSkillGem? GetSkillGemByName(string pName)
        {
            return GetFirstDataModel<PoeWikiSkillGem>(_cargoTitle, _cargoModelMap[CargoModelTypes.SKILLGEMS].Tables, _cargoModelMap[CargoModelTypes.SKILLGEMS].Fields, $"{_cargoModelMap[CargoModelTypes.SKILLGEMS].Where} AND _pageName=\"{pName}\"");
        }

        public PoeWikiSkillGem? GetSkillGemById(string pId)
        {
            return GetFirstDataModel<PoeWikiSkillGem>(_cargoTitle, _cargoModelMap[CargoModelTypes.SKILLGEMS].Tables, _cargoModelMap[CargoModelTypes.SKILLGEMS].Fields, $"{_cargoModelMap[CargoModelTypes.SKILLGEMS].Where} AND _ID=\"{pId}\"");
        }

        public List<PoeWikiSkillGem> GetAllSkillGems()
        {
            return GetListWithBatching<PoeWikiSkillGem>(_cargoTitle, _cargoModelMap[CargoModelTypes.SKILLGEMS].Tables, _cargoModelMap[CargoModelTypes.SKILLGEMS].Fields, _cargoModelMap[CargoModelTypes.SKILLGEMS].Where, 500, 0);
        }
        #endregion

        #region "Passive Skills"
        public PoeWikiPassive? GetPassiveSkillByName(string pName)
        {
            return GetFirstDataModel<PoeWikiPassive>(_cargoTitle, _cargoModelMap[CargoModelTypes.PASSIVES].Tables, _cargoModelMap[CargoModelTypes.PASSIVES].Fields, $"{_cargoModelMap[CargoModelTypes.PASSIVES].Where} AND name=\"{pName}\"");
        }

        public PoeWikiPassive? GetPassiveSkillById(string pId)
        {
            return GetFirstDataModel<PoeWikiPassive>(_cargoTitle, _cargoModelMap[CargoModelTypes.PASSIVES].Tables, _cargoModelMap[CargoModelTypes.PASSIVES].Fields, $"{_cargoModelMap[CargoModelTypes.PASSIVES].Where} AND _ID=\"{pId}\"");
        }

        public List<PoeWikiPassive> GetAllPassiveSkills()
        {
            return GetListWithBatching<PoeWikiPassive>(_cargoTitle, _cargoModelMap[CargoModelTypes.PASSIVES].Tables, _cargoModelMap[CargoModelTypes.PASSIVES].Fields, _cargoModelMap[CargoModelTypes.PASSIVES].Where, 500, 0);
 
        }
        #endregion

        private T? GetFirstDataModel<T>(string pTitle, string pTables, string pFields, string pWhere)
        {
            return JsonParser.ParseJsonList<T>(GetFromCacheOrApi(BuildQueryString(pTitle, pTables, pFields, pWhere, 1, 0))).FirstOrDefault();
        }

        private List<T> GetListWithBatching<T>(string pTitle, string pTables, string pFields, string pWhere, int pLimit, int pOffsetStart)
        {
            List<T> returnList = [];
            for (int offset = pOffsetStart, count = pLimit; count >= pLimit; offset += pLimit)
            {
                List<T> batchList = JsonParser.ParseJsonList<T>(GetFromCacheOrApi(BuildQueryString(pTitle, pTables, pFields, pWhere, pLimit, offset)));
                count = batchList.Count;
                returnList.AddRange(batchList);
            }

            return returnList;
        }

        private string GetFromCacheOrApi(string pUri)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            _logger.Log($"GETTING: {pUri}.");

            string? result = _cache.Get(pUri);
            if (result == null)
            {
                result = _http.Get(pUri);
                _logger.Log("Found: api.");
                _cache.Set(pUri, result);
            }
            else
            {
                _logger.Log("Found: cache.");
            }

            stopwatch.Stop();
            _logger.Log($"Done. Time elapsed: {stopwatch.ElapsedMilliseconds / 1000.0}.");
            return result;
        }

        private static string BuildQueryString(string pTitle, string pTables, string pFields, string pWhere, int pLimit, int pOffset)
        {
            return _uriBase.AppendQueryParam("title", pTitle).AppendQueryParam("tables", pTables).AppendQueryParam("fields", pFields).AppendQueryParam("where", pWhere).AppendQueryParam("limit", pLimit).AppendQueryParam("offset", pOffset).AppendQueryParam("format", "json");
        }
    }
}
 