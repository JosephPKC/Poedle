using System.Text.RegularExpressions;

using Poedle.PoeDb.Models;
using Poedle.PoeWiki.Models;
using Poedle.Enums;
using Poedle.Utils.Lists;
using Poedle.Utils.Strings;

namespace Poedle.PoeDb.Mappers
{
    public static partial class UniqueMapper
    {
        [GeneratedRegex("^[\\w\\s]+\\(lore\\)\\|")]
        private static partial Regex LoreEmbedRegex();

        [GeneratedRegex("\\[\\[[\\w\\s]+\\|")]
        private static partial Regex KeywordAltEmbedRegex();

        public static DbUnique MapUnique(PoeWikiUnique pUnique, List<DbLeague> pLeagues)
        {
            DbUnique unique = new()
            {
                ItemClass = GetItemClass(pUnique.ItemClass),
                BaseItem = pUnique.BaseItem,
                FlavourText = GetCleanedFlavourText(pUnique.FlavourText),
                LeaguesIntroduced = GetLeaguesIntroduced(pLeagues),
                StatText = GetCleanedStatText(pUnique.StatText),
                ReqLvl = pUnique.ReqLvl,
                ReqDex = pUnique.ReqDex,
                ReqInt  = pUnique.ReqInt,
                ReqStr = pUnique.ReqStr,
                Qualities = GetQualities(pUnique),
                DropSources = GetDropSources(pUnique),
                DropSourcesSpecific = GetDropSourcesSpecific(pUnique)
            };

            BaseModelMapper.SetBasePoeFields(unique, pUnique);

            return unique;
        }

        private static ItemClassesEnum.ItemClasses GetItemClass(string pItemClass)
        {
            if (string.IsNullOrWhiteSpace(pItemClass))
            {
                return ItemClassesEnum.ItemClasses.NONE;
            }

            return ItemClassesEnum.StrToEnum(pItemClass);
        }

        private static List<LeaguesEnum.Leagues> GetLeaguesIntroduced(List<DbLeague> pLeagues)
        {
            if (pLeagues == null || pLeagues.Count == 0)
            {
                // Base Game
                return [LeaguesEnum.Leagues.BASE_GAME];
            }

            return pLeagues.Select(x => LeaguesEnum.StrToEnum(x.Name)).ToList();
        }

        private static List<string> GetCleanedFlavourText(string pFlavourText)
        {
            string cleanedText = HTMLTagCleaner.ParseHTMLTags(pFlavourText);
            // Clean up the lore embeds
            if (MiscStringUtils.ContainsIgnoreCase(cleanedText, "(lore)|"))
            {
                cleanedText = LoreEmbedRegex().Replace(cleanedText, "");
            }
            // Clean up the atziri and breach blessing upgrade special texts
            if (MiscStringUtils.ContainsIgnoreCase(cleanedText, "-help"))
            {
                cleanedText = HTMLTagCleaner.ReplaceSpanClassTags(cleanedText);
            }
            // Clean up the harbinger glyphs
            if (MiscStringUtils.ContainsIgnoreCase(cleanedText, "class=\"glyph"))
            {
                cleanedText = HTMLTagCleaner.ReplaceSpanClassTags(cleanedText, "<glyph?>");
            }

            return StringCleaner.SeparateStringLines(cleanedText);
        }

        private static List<string> GetCleanedStatText(string pStatText)
        {
            string cleanedText = pStatText;
            // Clean up the [[word|conjugate]] formats
            if (KeywordAltEmbedRegex().Match(cleanedText) != Match.Empty)
            {
                cleanedText = KeywordAltEmbedRegex().Replace(cleanedText, "");
            }
            // Do generic parsing after as it is easier to find the above variants with the prefix '[['
            cleanedText = HTMLTagCleaner.ParseHTMLTags(cleanedText);

            // Clean up the unique mod line
            if (MiscStringUtils.ContainsIgnoreCase(cleanedText, "-unique"))
            {
                cleanedText = HTMLTagCleaner.ReplaceSpanClassTags(cleanedText, "\n");
            }

            return StringCleaner.SeparateStringLines(cleanedText);
        }

        /// <summary>
        /// Gets all of the special qualities for the item.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        private static List<QualitiesEnum.Qualities> GetQualities(PoeWikiUnique pUnique)
        {
            List<QualitiesEnum.Qualities> qualities = [];

            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.ABYSSAL, UniqueTagsParser.IsAbyssal(pUnique));
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.ANOINTABLE, UniqueTagsParser.IsAnointable(pUnique));
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.CORRUPTED, pUnique.IsCorrupted);
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.EATER_OF_WORLDS, pUnique.IsEaterOfWorlds);
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.ELDER, MiscStringUtils.ContainsIgnoreCase(pUnique.Influences, "Elder"));
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.FRACTURED, pUnique.IsFractured);
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.GRANTS_SKILL, UniqueTagsParser.IsGrantsSkills(pUnique));
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.REPLICA, pUnique.IsReplica);
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.SEARING_EXARCH, pUnique.IsSearingExarch);
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.SHAPER, MiscStringUtils.ContainsIgnoreCase(pUnique.Influences, "Shaper"));
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.SYNTHESISED, pUnique.IsSynthesised);
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.TRIGGERS_SKILL, UniqueTagsParser.IsTriggersSkill(pUnique));
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.UPGRADEABLE, UniqueTagsParser.IsUpgradeable(pUnique));
            MiscListUtils.ConditionalAddToList(qualities, QualitiesEnum.Qualities.VEILED, pUnique.IsVeiled);

            return qualities;
        }

        /// <summary>
        /// Gets all of the generic drop sources for the item.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        private static List<DropSourcesEnum.DropSources> GetDropSources(PoeWikiUnique pUnique)
        {
            List<DropSourcesEnum.DropSources> drops = [];

            MiscListUtils.ConditionalAddToList(drops, DropSourcesEnum.DropSources.BOSS_DROP, UniqueTagsParser.IsBossDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesEnum.DropSources.SPECIAL, UniqueTagsParser.IsSpecialDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesEnum.DropSources.SPECIAL_DROP_AREA, UniqueTagsParser.IsSpecialAreaDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesEnum.DropSources.SPECIAL_ENCOUNTER, UniqueTagsParser.IsSpecialEncounterDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesEnum.DropSources.UPGRADE, UniqueTagsParser.IsUpgrade(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesEnum.DropSources.VENDOR_RECIPE, UniqueTagsParser.IsVendorRecipe(pUnique));

            return drops;
        }

        /// <summary>
        ///  Gets all of the specific drop sources for the item.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        private static List<DropSourcesSpecificEnum.DropSourcesSpecific> GetDropSourcesSpecific(PoeWikiUnique pUnique)
        {
            List<DropSourcesSpecificEnum.DropSourcesSpecific> drops = [];

            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.ABYSSAL_BOSS, UniqueTagsParser.IsAbyssalBossDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.ABYSSAL_TROVE, UniqueTagsParser.IsAbyssalTroveDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.ATLAS_MAP_BOSS, UniqueTagsParser.IsAtlasBossDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.BEACHHEAD, UniqueTagsParser.IsBeachheadDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.BLIGHTED, UniqueTagsParser.IsBlightedMapDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.BLIGHT_RAVAGED, UniqueTagsParser.IsBlightRavagedMapDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.BREACH, UniqueTagsParser.IsBreachMonsterDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.CORRUPTION, UniqueTagsParser.IsCorruptionAltarUpgrade(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.CURIO_DISPLAY, UniqueTagsParser.IsCurioDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.DELIRIUM_BOSS, UniqueTagsParser.IsDeliriumBossDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.DELVE_BOSS, UniqueTagsParser.IsDelveDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.DOMAIN_TIMLESS, UniqueTagsParser.IsDomainDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.EXPEDITION_BOSS, UniqueTagsParser.IsExpeditionBossDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.EXPEDITION_ZONE, UniqueTagsParser.IsExpeditionZoneDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.FLAWLESS_BREACHSTONE, UniqueTagsParser.IsFlawlessBreachstoneDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.HARBINGER, UniqueTagsParser.IsHarbingerItemDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.HEIST_BOSS, UniqueTagsParser.IsHeistBossDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.INCURSION, UniqueTagsParser.IsIncursionRoomDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.INCURION_BOSS, UniqueTagsParser.IsIncurionBossDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.LABYRINTH, UniqueTagsParser.IsLabDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.LEGION_GENERAL, UniqueTagsParser.IsLegionGeneralDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.MASTERMIND, UniqueTagsParser.IsMastermindDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.RITUAL, UniqueTagsParser.IsRitualDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.SANCTUM, UniqueTagsParser.IsSanctumWithoutRelicDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.SANCTUM_WITH_RELIC, UniqueTagsParser.IsSanctumWithRelicDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.SIMULACRUM, UniqueTagsParser.IsSimulacrumDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.SMUGGLERS_CACHE, UniqueTagsParser.IsHeistCacheDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.SYNDICATE_SAFEHOUSE, UniqueTagsParser.IsSafehouseDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.TIER_17_BOSS, UniqueTagsParser.IsTier17BossDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.TRIALMASTER, UniqueTagsParser.IsTrialMasterDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.ULTIMATUM_TRIAL, UniqueTagsParser.IsUltimatumTrialDrop(pUnique));
            MiscListUtils.ConditionalAddToList(drops, DropSourcesSpecificEnum.DropSourcesSpecific.WARBAND, UniqueTagsParser.IsWarbandsBossDrop(pUnique));

            return drops;
        }
    }
}
