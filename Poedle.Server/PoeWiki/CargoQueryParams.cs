namespace Poedle.PoeWiki
{
    public static class CargoQueryParams
    {
        public enum CargoParamTypes
        {
            NONE,
            LEAGUES,
            UNIQUES,
            SKILLGEMS,
            PASSIVES        
        }

        public struct CargoParams
        {
            public string Tables { get; set; }
            public string Fields { get; set; }
            public string Where { get; set; }
            public string Join { get; set; }
        }

        public static readonly Dictionary<CargoParamTypes, CargoParams> CargoParamsMap = new()
        {
            { CargoParamTypes.LEAGUES, new CargoParams() {
                Tables = "events",
                Fields = "name,_pageName,release_version",
                Where = "type=\"Challenge league\""
            }},
            { CargoParamTypes.UNIQUES, new CargoParams() {
                Tables = "items",
                Fields = "name,_pageName,class,base_item,influences,flavour_text,drop_monsters,release_version,stat_text,required_level,required_dexterity,required_intelligence,required_strength,is_corrupted,is_fractured,is_replica,is_synthesised,is_veiled,is_eater_of_worlds_item,is_searing_exarch_item",
                Where = "rarity_id=\"Unique\" AND removal_version IS NULL AND _pageName NOT LIKE \"%testcase%\""
            }},
            { CargoParamTypes.SKILLGEMS, new CargoParams() {
                Tables = "skill_gems",
                Fields = "_pageName,gem_description,primary_attribute,dexterity_percent,intelligence_percent,strength_percent,is_awakened_support_gem,is_vaal_skill_gem,gem_tags",
                Where = "_pageName NOT LIKE \"Template:%\""
            }},
            { CargoParamTypes.PASSIVES, new CargoParams() {
                Tables = "passive_skills",
                Fields = "id,name,_pageName,flavour_text,ascendancy_class,is_keystone,is_notable,stat_text",
                Where = "is_notable=true OR is_keystone=true"
            }}
        };
    }
}
