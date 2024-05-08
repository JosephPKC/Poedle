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

        public static DbUnique MapUnique(PoeWikiUnique pUnique, List<DbLeague> pLeagues)
        {
            DbUnique unique = new()
            {
                ItemClass = GetItemClass(pUnique.ItemClass),
                BaseItem = pUnique.BaseItem,
                FlavourText = GetCleanedFlavourText(pUnique.FlavourText),
                LeaguesIntroduced = GetLeaguesIntroduced(pLeagues),
                ImplicitStatText = GetCleanedStatText(pUnique.ImplicitStatText),
                ExplicitStatText = GetCleanedStatText(pUnique.ExplicitStatText),
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
            string cleanedText = HTMLTagCleaner.ParseBasicHTMLTags(pFlavourText);
            // Clean up the lore embeds
            if (MiscStringUtils.ContainsIgnoreCase(cleanedText, "(lore)|"))
            {
                cleanedText = LoreEmbedRegex().Replace(cleanedText, "");
            }
            // Clean up <span class="tc -help">...</span>. Usually used for help text for incursion item flavour text.
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "tc -help");
            // Clean up <span class="glyph code"></span>. For harbinger item flavour text.
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "glyph [\\w\\d]+", "<glyph?>");

            return StringCleaner.SeparateStringLines(cleanedText);
        }

        private static List<string> GetCleanedStatText(string pStatText)
        {
            // Clean up [[word1|word2].
            string cleanedText = HTMLTagCleaner.ReplaceBracketGroupWithSecond(pStatText);
            // Clean up <hr style="width: 20%">
            cleanedText = cleanedText.Replace("<hr style=\"width: 20%\">", "");
            // Do generic parsing after as it is easier to find the above variants with the prefix '[['
            cleanedText = HTMLTagCleaner.ParseBasicHTMLTags(cleanedText);
            // Clean up <span class="item-stat-separator -unique">...</span>
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "item-stat-separator -unique", "\n");
            // Clean up <span class="hoverbox c-tooltip"></span>
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "hoverbox__activator c-tooltip__activator", "");
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "hoverbox__display c-tooltip__display", "");
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "hoverbox c-tooltip", "");
            // Clean up <span class="tc -default">...</span> and "tc -value", which is used to set a font for certain text.
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -default");
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -value");
            // Clean up <span class="tc -corrupted">...</span>. Usually for the Forbidden jewels.
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -corrupted");
            // Clean up <table class="...">...</table> with its header inner text. Usually, for the mod lines that can have multiple options.
            // Clean up multi mod lines (i.e. Aul's Uprising has a line that can be one of many different mods).
            cleanedText = HTMLTagCleaner.ReplaceTableClassWithHeader(cleanedText);
            // Clean up <span class="veiled -suffix"></span>. For veiled mods.
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "veiled -prefix");
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "veiled -suffix");
            // Clean up <abbr title="...">...</abbr> tags
            cleanedText = HTMLTagCleaner.ReplaceAbbrWithInnerText(cleanedText);
            // Clean up <veiled mod pool> and other similar meta tag lines.
            cleanedText = cleanedText.Replace("<veiled mod pool>", "");
            cleanedText = cleanedText.Replace("<two random mods of devotion>", "");
            // Clean up double bracket tags <<...>>
            cleanedText = HTMLTagCleaner.ParseDoubleBracketTags(cleanedText);
            // Clean up unecessary endlines found primarily in Precursor's Emblems
            cleanedText = cleanedText.Replace("\nor\n", "or");

            // Split the text into individual lines.
            List<string> cleanedTextLines = StringCleaner.SeparateStringLines(cleanedText);
            // Remove (Hidden) mod lines.
            return cleanedTextLines.FindAll(x => !x.Contains("(Hidden)"));
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
