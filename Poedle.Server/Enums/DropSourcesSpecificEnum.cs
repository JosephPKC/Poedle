namespace Poedle.Enums
{
    public static class DropSourcesSpecificEnum
    {
        public enum DropSourcesSpecific
        {
            NONE,
            ABYSSAL_BOSS,
            ABYSSAL_TROVE,
            ATLAS_MAP_BOSS,
            BEACHHEAD,
            BLIGHTED,
            BLIGHT_RAVAGED,
            BREACH,
            CORRUPTION,
            CURIO_DISPLAY,
            DELIRIUM_BOSS,
            DELVE_BOSS,
            DOMAIN_TIMLESS,
            EXPEDITION_BOSS,
            EXPEDITION_ZONE,
            FLAWLESS_BREACHSTONE,
            HARBINGER,
            HEIST_BOSS,
            INCURSION,
            INCURION_BOSS,
            LABYRINTH,
            LEGION_GENERAL,
            MASTERMIND,
            RITUAL,
            SANCTUM,
            SANCTUM_WITH_RELIC,
            SIMULACRUM,
            SMUGGLERS_CACHE,
            SYNDICATE_SAFEHOUSE,
            TIER_17_BOSS,
            TRIALMASTER,
            ULTIMATUM_TRIAL,
            WARBAND
        }

        private readonly static Dictionary<DropSourcesSpecific, string> _enumToStrExceptions = new()
        {
            { DropSourcesSpecific.ABYSSAL_BOSS, "Abyssal Boss" },
            { DropSourcesSpecific.ABYSSAL_TROVE, "Abyssal Trove/Stygian Spire" },
            { DropSourcesSpecific.ATLAS_MAP_BOSS, "Atlas Map Boss" },
            { DropSourcesSpecific.BEACHHEAD, "The Beachhead" },
            { DropSourcesSpecific.BLIGHTED, "Blighted Map" },
            { DropSourcesSpecific.BLIGHT_RAVAGED, "Blight-Ravaged Map" },
            { DropSourcesSpecific.BREACH, "Breach / Breachstone" },
            { DropSourcesSpecific.CORRUPTION, "Corruption Altar" },
            { DropSourcesSpecific.CURIO_DISPLAY, "Curio Display" },
            { DropSourcesSpecific.DELIRIUM_BOSS, "Delirium Boss" },
            { DropSourcesSpecific.DELVE_BOSS, "Azurite Mine Boss" },
            { DropSourcesSpecific.DOMAIN_TIMLESS, "Domain of Timeless Conflict" },
            { DropSourcesSpecific.EXPEDITION_BOSS, "Expedition Logbook Boss" },
            { DropSourcesSpecific.EXPEDITION_ZONE, "Expedition Zone" },
            { DropSourcesSpecific.FLAWLESS_BREACHSTONE, "Flawless Breachstone" },
            { DropSourcesSpecific.HARBINGER, "Harbinger Craft" },
            { DropSourcesSpecific.HEIST_BOSS, "Heist Boss" },
            { DropSourcesSpecific.INCURSION, "Temple of Atzoatl Room" },
            { DropSourcesSpecific.INCURION_BOSS, "Vaal Omnitect" },
            { DropSourcesSpecific.LABYRINTH, "Labyrinth" },
            { DropSourcesSpecific.LEGION_GENERAL, "Legion General" },
            { DropSourcesSpecific.MASTERMIND, "Mastermind's Lair" },
            { DropSourcesSpecific.RITUAL, "Ritual Altar" },
            { DropSourcesSpecific.SANCTUM, "Sanctum" },
            { DropSourcesSpecific.SANCTUM_WITH_RELIC, "Sanctum With Relic" },
            { DropSourcesSpecific.SIMULACRUM, "Simulacrum" },
            { DropSourcesSpecific.SMUGGLERS_CACHE, "Smuggler's Cache" },
            { DropSourcesSpecific.SYNDICATE_SAFEHOUSE, "Syndicate Safehouse" },
            { DropSourcesSpecific.TIER_17_BOSS, "Tier 17 Boss" },
            { DropSourcesSpecific.TRIALMASTER, "Trialmaster" },
            { DropSourcesSpecific.ULTIMATUM_TRIAL, "Ultimatum Trial" },
            { DropSourcesSpecific.WARBAND, "Warband" }
        };

        public static string EnumToStr(DropSourcesSpecific pEnum)
        {
            return EnumUtil.GetNameByValue(pEnum, _enumToStrExceptions);
        }

    }
}
