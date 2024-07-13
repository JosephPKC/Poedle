using PoeWikiApi.Models;
using PoeWikiData.Mappers.StaticData;
using PoeWikiData.Models.Leagues;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Models.StaticData.Enums;
using PoeWikiData.Models.UniqueItems;
using PoeWikiData.Utils;

namespace PoeWikiData.Mappers.UniqueItems
{
    internal static class UniqueItemDbMapper
    {
        public static UniqueItemDbModel Map(UniqueItemWikiModel pModel, LeagueDbLookUp pAllLeagues)
        {
            return new()
            {
                Id = pModel.Id,
                Name = pModel.Name,
                DisplayName = pModel.PageName,
                BaseItem = pModel.BaseItem,
                ItemClass = GetItemClass(pModel.ItemClass),
                ReqLvl = pModel.ReqLvl,
                ReqDex = pModel.ReqDex,
                ReqInt = pModel.ReqInt,
                ReqStr = pModel.ReqStr,
                LeaguesIntroduced = GetLeagues(pModel.ReleaseVersion, pAllLeagues),
                ItemAspects = GetItemAspects(pModel),
                DropSources = GetDropSources(pModel),
                DropTypes = GetDropTypes(pModel),
                FlavourText = GetFlavourTexts(pModel.FlavourText),
                ImplicitStatText = GetStatTexts(pModel.ImplicitStatText),
                ExplicitStatText = GetStatTexts(pModel.ExplicitStatText)
            };
        }

        private static StaticDataDbModel GetItemClass(string pItemClass)
        {
            return StaticDataMasterRef.ItemClasses.GetByName(StringUtils.NoSpaceDash(pItemClass)) ?? new();
        }

        private static IEnumerable<LeagueDbModel> GetLeagues(string pReleaseVersion, LeagueDbLookUp pAllLeagues)
        {
            IEnumerable<LeagueDbModel>? leagues = pAllLeagues.GetByVersion(new(pReleaseVersion));
            return leagues ?? [];
        }

        private static IEnumerable<StaticDataDbModel> GetItemAspects(UniqueItemWikiModel pModel)
        {
            ICollection<StaticDataDbModel> result = [];
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Abyssal, StaticDataMasterRef.ItemAspects), UniqueItemTagsParser.IsAbyssal(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Anointable, StaticDataMasterRef.ItemAspects), UniqueItemTagsParser.IsAnointable(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Corrupted, StaticDataMasterRef.ItemAspects), pModel.IsCorrupted);
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.EaterOfWorlds, StaticDataMasterRef.ItemAspects), pModel.IsEaterOfWorlds);
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Elder, StaticDataMasterRef.ItemAspects), StringUtils.ContainsIgnoreCase(pModel.Influences, "Elder"));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Fractured, StaticDataMasterRef.ItemAspects), pModel.IsFractured);
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.GrantsSkill, StaticDataMasterRef.ItemAspects), UniqueItemTagsParser.IsGrantsSkills(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Replica, StaticDataMasterRef.ItemAspects), pModel.IsReplica);
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.SearingExarch, StaticDataMasterRef.ItemAspects), pModel.IsSearingExarch);
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Shaper, StaticDataMasterRef.ItemAspects), StringUtils.ContainsIgnoreCase(pModel.Influences, "Shaper"));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Synthesised, StaticDataMasterRef.ItemAspects), pModel.IsSynthesised);
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.TriggersSkill, StaticDataMasterRef.ItemAspects), UniqueItemTagsParser.IsTriggersSkill(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Upgradeable, StaticDataMasterRef.ItemAspects), UniqueItemTagsParser.IsUpgradeable(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(ItemAspects.Veiled, StaticDataMasterRef.ItemAspects), pModel.IsVeiled);
            return result;
        }

        private static IEnumerable<StaticDataDbModel> GetDropTypes(UniqueItemWikiModel pModel)
        {
            ICollection<StaticDataDbModel> result = [];
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropTypes.BossDrop, StaticDataMasterRef.DropTypes), UniqueItemTagsParser.IsBossDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropTypes.Special, StaticDataMasterRef.DropTypes), UniqueItemTagsParser.IsSpecialDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropTypes.SpecialDropArea, StaticDataMasterRef.DropTypes), UniqueItemTagsParser.IsSpecialAreaDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropTypes.SpecialEncounter, StaticDataMasterRef.DropTypes), UniqueItemTagsParser.IsSpecialEncounterDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropTypes.Upgrade, StaticDataMasterRef.DropTypes), UniqueItemTagsParser.IsUpgrade(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropTypes.VendorRecipe, StaticDataMasterRef.DropTypes), UniqueItemTagsParser.IsVendorRecipe(pModel));
            return result;
        }

        private static IEnumerable<StaticDataDbModel> GetDropSources(UniqueItemWikiModel pModel)
        {
            ICollection<StaticDataDbModel> result = [];
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.AbyssalBoss, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsAbyssalBossDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.AbyssalTroveStygianSpire, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsAbyssalTroveDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.AtlasMapBoss, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsAtlasBossDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.TheBeachhead, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsBeachheadDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.BlightedMap, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsBlightedMapDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.BlightRavagedMap, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsBlightRavagedMapDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.BreachBreachstone, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsBreachMonsterDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.CorruptionAltar, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsCorruptionAltarUpgrade(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.CurioDisplay, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsCurioDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.DeliriumBoss, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsDeliriumBossDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.AzuriteMineBoss, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsDelveDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.DomainOfTimelessConflict, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsDomainDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.ExpeditionLogbookBoss, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsExpeditionBossDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.ExpeditionZone, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsExpeditionZoneDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.FlawlessBreachstone, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsFlawlessBreachstoneDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.HarbingerCraft, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsHarbingerItemDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.HeistBoss, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsHeistBossDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.TempleOfAtzoatlRoom, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsIncursionRoomDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.VaalOmnitect, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsIncurionBossDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.Labyrinth, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsLabDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.LegionGeneral, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsLegionGeneralDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.MastermindsLair, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsMastermindDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.RitualAltar, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsRitualDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.Sanctum, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsSanctumWithoutRelicDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.SanctumWithRelic, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsSanctumWithRelicDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.Simulacrum, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsSimulacrumDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.SmugglersCache, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsHeistCacheDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.SyndicateSafehouse, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsSafehouseDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.Tier17MapBoss, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsTier17BossDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.Trialmaster, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsTrialMasterDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.UltimatumTrial, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsUltimatumTrialDrop(pModel));
            ListUtils.ConditionalAddToList(result, StaticDataDbMapper.GetStaticDataFromEnum(DropSources.Warband, StaticDataMasterRef.DropSources), UniqueItemTagsParser.IsWarbandsBossDrop(pModel));
            return result;
        }

        private static IEnumerable<string> GetFlavourTexts(string pFlavourText)
        {
            string cleanedText = HtmlTextCleaner.ParseBasicHTMLTags(pFlavourText);
            // Clean up the lore embeds
            cleanedText = HtmlTextCleaner.CleanTextEmbed(cleanedText, "lore");
            // Clean up <span class="tc -help">...</span>. Usually used for help text for incursion item flavour text.
            cleanedText = HtmlTextCleaner.ReplaceSpanClass(cleanedText, "tc -help");
            // Clean up <span class="glyph code"></span>. For harbinger item flavour text.
            cleanedText = HtmlTextCleaner.ReplaceSpanClass(cleanedText, "glyph [\\w\\d]+", "<glyph?>");

            IEnumerable<string> cleanedTexts = StringUtils.SeparateStringLines(cleanedText);
            return StringUtils.RemoveBlankLines(cleanedTexts);
        }

        private static List<string> GetStatTexts(string pStatText)
        {
            // Clean up [[word1|word2].
            string cleanedText = HtmlTextCleaner.ReplaceBracketGroupWithSecond(pStatText);
            // Clean up <hr style="width: 20%">
            cleanedText = cleanedText.Replace("<hr style=\"width: 20%\">", "");
            // Do generic parsing after as it is easier to find the above variants with the prefix '[['
            cleanedText = HtmlTextCleaner.ParseBasicHTMLTags(cleanedText);
            // Clean up <span class="item-stat-separator -unique">...</span>
            cleanedText = HtmlTextCleaner.ReplaceSpanClass(cleanedText, "item-stat-separator -unique", "\n");
            // Clean up <span class="hoverbox c-tooltip"></span>
            cleanedText = HtmlTextCleaner.ReplaceSpanClass(cleanedText, "hoverbox__activator c-tooltip__activator", "");
            cleanedText = HtmlTextCleaner.ReplaceSpanClass(cleanedText, "hoverbox__display c-tooltip__display", "");
            cleanedText = HtmlTextCleaner.ReplaceSpanClass(cleanedText, "hoverbox c-tooltip", "");
            // Clean up <span class="tc -default">...</span> and "tc -value", which is used to set a font for certain text.
            cleanedText = HtmlTextCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -default");
            cleanedText = HtmlTextCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -value");
            // Clean up <span class="tc -corrupted">...</span>. Usually for the Forbidden jewels.
            cleanedText = HtmlTextCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -corrupted");
            // Clean up <table class="...">...</table> with its header inner text. Usually, for the mod lines that can have multiple options.
            // Clean up multi mod lines (i.e. Aul's Uprising has a line that can be one of many different mods).
            cleanedText = HtmlTextCleaner.ReplaceTableClassWithHeader(cleanedText);
            // Clean up <span class="veiled -suffix"></span>. For veiled mods.
            cleanedText = HtmlTextCleaner.ReplaceSpanClassWithInnerText(cleanedText, "veiled -prefix");
            cleanedText = HtmlTextCleaner.ReplaceSpanClassWithInnerText(cleanedText, "veiled -suffix");
            // Clean up <abbr title="...">...</abbr> tags
            cleanedText = HtmlTextCleaner.ReplaceAbbrWithInnerText(cleanedText);
            // Clean up <veiled mod pool> and other similar meta tag lines.
            cleanedText = cleanedText.Replace("<veiled mod pool>", "");
            cleanedText = cleanedText.Replace("<two random mods of devotion>", "");
            // Clean up double bracket tags <<...>>
            cleanedText = HtmlTextCleaner.ParseDoubleBracketTags(cleanedText);
            // Clean up unecessary endlines found primarily in Precursor's Emblems
            cleanedText = cleanedText.Replace("\nor\n", "or");

            // Split the text into individual lines.
            List<string> cleanedTextLines = (List<string>)StringUtils.SeparateStringLines(cleanedText);
            cleanedTextLines = (List<string>)StringUtils.RemoveBlankLines(cleanedTextLines);
            // Remove (Hidden) mod lines.
            return cleanedTextLines.FindAll(x => !x.Contains("(Hidden)"));
        }
    }
}
