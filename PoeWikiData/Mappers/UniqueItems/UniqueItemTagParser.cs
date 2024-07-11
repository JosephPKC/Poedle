using PoeWikiApi.Models;
using PoeWikiData.Utils;

namespace PoeWikiData.Mappers.UniqueItems
{
    public static class UniqueTagsParser
    {
        /// <summary>
        /// Item is abyssal if it has abyssal sockets naturally.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsAbyssal(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.ImplicitStatText, "Abyssal [[Item socket|Sockets]]") || BaseUtils.ContainsIgnoreCase(pUnique.ExplicitStatText, "Abyssal [[Item socket|Sockets]]");
        }

        /// <summary>
        /// Item is anointable if it can be anointed.
        /// This includes all amulets and rings, and certain Blight items.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsAnointable(UniqueItemWikiModel pUnique)
        {
            return IsBlightDrop(pUnique) || BaseUtils.EqualsIgnoreCase(pUnique.ItemClass, "Amulet") || BaseUtils.EqualsIgnoreCase(pUnique.ItemClass, "Ring");
        }

        /// <summary>
        /// Item is a boss drop, if it drops from a boss.
        /// Nearly all boss drops are stated in its DropMonsters.
        /// Some drop from generic Atlas bosses, and thus check DropText.
        /// Certain Delirium items are effectively boss drops in that you need to kill the boss wave in the Simulacrum.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBossDrop(UniqueItemWikiModel pUnique)
        {
            // NOTE: Simulacrum Boss drops are technically not boss drops. But, they require killing the boss and its wave.
            // NOTE: Synthesised Maps drop from any Atlas boss.
            return pUnique.DropMonsters.Count > 0 || IsSimulacrumDrop(pUnique) || IsAtlasBossDrop(pUnique);
        }

        /// <summary>
        /// Item grants skills if there is a stat line that grants a skill.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsGrantsSkills(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.ImplicitStatText, "Grants [[Level]]") || BaseUtils.ContainsIgnoreCase(pUnique.ExplicitStatText, "Grants [[Level]]");
        }

        /// <summary>
        /// Misc special drops for any other non-normal drop.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsSpecialDrop(UniqueItemWikiModel pUnique)
        {
            // Angler's Plait, Song of the Sirens
            uint[] specialUniques = [132754, 133934];
            return specialUniques.Contains(pUnique.Id);
        }

        /// <summary>
        /// Drops in a special zone or area, outside of normal maps or atlas zones.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsSpecialAreaDrop(UniqueItemWikiModel pUnique)
        {
            if (IsAbyssDrop(pUnique)) return true;
            if (IsBetrayalDrop(pUnique)) return true;
            if (IsBlightDrop(pUnique)) return true;
            if (IsBreachDrop(pUnique)) return true;
            if (IsDeliriumDrop(pUnique)) return true;
            if (IsDelveDrop(pUnique)) return true;
            if (IsExpeditionDrop(pUnique)) return true;
            if (IsIncursionDrop(pUnique)) return true;
            if (IsHarbingerDrop(pUnique)) return true;
            if (IsHeistDrop(pUnique)) return true;
            if (IsLabDrop(pUnique)) return true;
            if (IsLegionDrop(pUnique)) return true;
            if (IsSanctumDrop(pUnique)) return true;

            return false;
        }

        /// <summary>
        /// Drops in normal maps or zones, but requires a special encounter.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsSpecialEncounterDrop(UniqueItemWikiModel pUnique)
        {
            if (IsAbyssalTroveDrop(pUnique)) return true;
            if (IsBreachMonsterDrop(pUnique)) return true;
            if (IsDeliriumBossDrop(pUnique)) return true;
            if (IsExpeditionZoneDrop(pUnique)) return true;
            if (IsHeistCacheDrop(pUnique)) return true;
            if (IsLegionGeneralDrop(pUnique)) return true;
            if (IsRitualDrop(pUnique)) return true;
            if (IsUltimatumTrialDrop(pUnique)) return true;
            if (IsWarbandsBossDrop(pUnique)) return true;

            return false;
        }

        /// <summary>
        /// Item triggers a skill if there is a stat line that specifies a triggered skill.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsTriggersSkill(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.ExplicitStatText, "Trigger [[Level]]") || BaseUtils.ContainsIgnoreCase(pUnique.ExplicitStatText, "Triggers [[Level]]");
        }


        /// <summary>
        /// Upgrades are items that can be created by upgrading a base item of some kind.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsUpgrade(UniqueItemWikiModel pUnique)
        {
            if (IsIncursionUpgrade(pUnique)) return true;
            if (IsBreachUpgrade(pUnique)) return true;
            if (IsHarbingerUpgrade(pUnique)) return true;

            return false;
        }

        /// <summary>
        /// Upgradeables are items that can be upgraded using some item.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsUpgradeable(UniqueItemWikiModel pUnique)
        {
            if (IsIncursionUpgradeable(pUnique)) return true;
            if (IsBreachUpgradeable(pUnique)) return true;
            if (IsHarbingerUpgradeable(pUnique)) return true;

            return false;
        }

        /// <summary>
        /// Vendor Recipes are items that can be crafted using a special vendor recipe or inventory recipe (not upgrading).
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsVendorRecipe(UniqueItemWikiModel pUnique)
        {
            // The Anima Stone, Arborix, Cameria's Avarice, Combat Focus (all 3), Duskdawn, The Goddess Scorned, Hyrri's Bite, Kingmaker, Loreweave, Magna Eclipsis, Precursor's Emblem (all 7), The Retch, Star of Wraeclast, The Taming, The Vinktar Square (all 5)
            // NOTE: The Adorned is technically not a vendor recipe, but it requires placing pieces in inventory similar to a vendor recipe.
            uint[] vendorUniques = [134063, 132766, 132920, 141935, 141934, 141933, 133128, 134166, 64350, 136997, 133493, 133501, 133657, 133658, 133659, 133660, 133661, 133662, 133663, 141166, 141854, 134282, 134412, 134413, 134414, 134415, 134416, 146303];
            // Harbinger base items are also vendor recipe, similar to Adorned
            return vendorUniques.Contains(pUnique.Id) || IsHarbingerUpgradeable(pUnique);
        }

        #region "Abyss"
        /// <summary>
        /// Drops from Abyssal Troves, Stygian Spires, or from Abyssal Depths bosses.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsAbyssDrop(UniqueItemWikiModel pUnique)
        {
            return IsDelveAbyssBossDrop(pUnique) || IsAbyssalTroveDrop(pUnique) || IsAbyssalBossDrop(pUnique);
        }

        /// <summary>
        /// Drops from Abyssal Troves or Stygian Spires from Abyss encounters.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsAbyssalTroveDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Abyssal Troves");
        }

        /// <summary>
        /// Drops from Abyssal bosses.
        /// This INCLUDES the delve Abyssal boss.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsAbyssalBossDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "ReligiousTemplar");
        }
        #endregion

        #region "Atlas"
        /// <summary>
        /// Drops from generic atlas map bosses.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsAtlasBossDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Map Bosses");
        }
        #endregion

        #region "Betrayal"
        /// <summary>
        /// Drops from Syndicate Safehouse leaders or the Mastermind.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBetrayalDrop(UniqueItemWikiModel pUnique)
        {
            return IsSafehouseDrop(pUnique) || IsMastermindDrop(pUnique);
        }

        /// <summary>
        /// Drops from the Mastermind, Catarina.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsMastermindDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "BetrayalCatarina1");
        }

        /// <summary>
        /// Drops from the Safehouse leaders, but NOT the Mastermind.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsSafehouseDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Safehouse");
        }
        #endregion

        #region "Blight"
        /// <summary>
        /// Drops from Blighted maps or Blight-Ravaged maps.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBlightDrop(UniqueItemWikiModel pUnique)
        {
            return IsBlightedMapDrop(pUnique) || IsBlightRavagedMapDrop(pUnique);
        }

        /// <summary>
        /// Drops from Blighted maps.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBlightedMapDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Blighted Map");
        }

        /// <summary>
        /// Drops from Blight-Ravaged maps.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBlightRavagedMapDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Blight-ravaged map");
        }
        #endregion

        #region "Breach"
        /// <summary>
        /// Drops in Breaches, Breachstones, and Flawless Breachstones.
        /// Includes all base and upgraded items + Uul Netol's Vow.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBreachDrop(UniqueItemWikiModel pUnique)
        {
            // Upgraded breach items can drop in Flawless Breachstones

            return IsBreachMonsterDrop(pUnique) || IsFlawlessBreachstoneDrop(pUnique);
        }

        /// <summary>
        /// Drops from Breach monsters during Breach encounters.
        /// Includes all Breach upgradeable items.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBreachMonsterDrop(UniqueItemWikiModel pUnique)
        {
            return IsBreachUpgradeable(pUnique);
        }

        /// <summary>
        /// Drops from Breach monsters during Breach encounters.
        /// Includes all Breach upgraded items.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsFlawlessBreachstoneDrop(UniqueItemWikiModel pUnique)
        {
            // Uul-Netol's Vow is neither upgradeable or upgraded.
            return IsBreachUpgrade(pUnique) || pUnique.Id == 134395;
        }

        /// <summary>
        /// Can be crafted with a Breach blessing.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBreachUpgrade(UniqueItemWikiModel pUnique)
        {
            // Items mention Flawless Breachstone.
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Flawless Breachstone");
        }

        /// <summary>
        /// Can be upgraded with a Breach blessing.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBreachUpgradeable(UniqueItemWikiModel pUnique)
        {
            // Items mention Breach Monsters.
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Breach Monsters");
        }
        #endregion

        #region "Delirium"
        /// <summary>
        /// Drops during Delirium encounters or in Simulacrum.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsDeliriumDrop(UniqueItemWikiModel pUnique)
        {
            return IsDeliriumBossDrop(pUnique) || IsSimulacrumDrop(pUnique);
        }

        /// <summary>
        /// Drops from a Delirium boss during a Delirium encounter.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsDeliriumBossDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "LeagueAffliction");
        }

        /// <summary>
        /// Drops from defeating a boss wave in the Simulacrum.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsSimulacrumDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Simulacrum");
        }
        #endregion

        #region "Delve"
        /// <summary>
        /// Drops in the Azurite Mine.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsDelveDrop(UniqueItemWikiModel pUnique)
        {
            // These uniques have a different tag for boss drop.
            return IsDelveAbyssBossDrop(pUnique) || IsDelveNonAbyssBossDrop(pUnique);
        }

        /// <summary>
        /// Drops from Azurite Mine bosses, excep the Abyss boss.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsDelveNonAbyssBossDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "LeagueDelve");
        }

        /// <summary>
        /// Drops from the Abyss boss in the Azurite Mine.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsDelveAbyssBossDrop(UniqueItemWikiModel pUnique)
        {
            // Ahkeli's Valley, Putembo's Meadow, Uzaza's Mountain drop from the Abyss boss in delve.
            uint[] delveUniques = [132721, 133677, 134397];
            return delveUniques.Contains(pUnique.Id);
        }
        #endregion

        #region "Expedition"
        /// <summary>
        /// Drops from Expedition zones, logbook zones, and Expedition bosses.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsExpeditionDrop(UniqueItemWikiModel pUnique)
        {
            return IsExpeditionZoneDrop(pUnique) || IsExpeditionBossDrop(pUnique);
        }

        /// <summary>
        /// Drops in any Expedition zone.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsExpeditionZoneDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Expedition monsters");
        }

        /// <summary>
        /// Drops in any Expedition zone.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsExpeditionBossDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "LeagueExpedition");
        }
        #endregion

        #region "Harbinger"
        /// <summary>
        /// Drops from Harbinger Portals in the Beachhead, crafted using Harbinger pieces found in the Beachhead, or upgraded using Harbinger scrolls.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsHarbingerDrop(UniqueItemWikiModel pUnique)
        {
            return IsHarbingerItemDrop(pUnique) || IsBeachheadDrop(pUnique);
        }

        /// <summary>
        /// Drops from the Beachhead
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsBeachheadDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "HarbingerPortal");
        }

        /// <summary>
        /// Harbinger item crafted from Harbinger item pieces.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsHarbingerItemDrop(UniqueItemWikiModel pUnique)
        {
            return IsHarbingerUpgrade(pUnique) || IsHarbingerUpgradeable(pUnique);
        }

        /// <summary>
        /// Upgraded from a Harbinger item using a Harbinger Scroll.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsHarbingerUpgrade(UniqueItemWikiModel pUnique)
        {
            // The Shattered Divinity, The Tempest's Liberation, The Torrent's Reclamation, The Surging Thoughts, The Yielding Mortality, The Immortal Will
            uint[] harbingerUniques = [134261, 134285, 134288, 134278, 134326, 134187];
            return harbingerUniques.Contains(pUnique.Id);
        }

        /// <summary>
        /// Created using Harbinger Pieces.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsHarbingerUpgradeable(UniqueItemWikiModel pUnique)
        {
            // The Flow Untethered, The Fracturing Spinner, The Tempest's Binding, The Rippling Thoughts, The Enmity Divine, The Unshattered Will
            uint[] harbingerUniques = [146301, 146305, 146306, 146307, 146308, 146302];
            return harbingerUniques.Contains(pUnique.Id);
        }
        #endregion

        #region "Heist"
        /// <summary>
        /// Drops from Grand Heists, Smuggler's Caches, Contracts, and Heist bosses.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsHeistDrop(UniqueItemWikiModel pUnique)
        {
            return IsHeistCacheDrop(pUnique) || IsCurioDrop(pUnique) || IsHeistBossDrop(pUnique);
        }

        /// <summary>
        /// Drops from Smuggler's Caches.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsHeistCacheDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Smuggler's Cache");
        }

        /// <summary>
        /// Drops from Curio Displays in Grand Heists.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsCurioDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Curio Display");
        }


        /// <summary>
        /// Drops from Grand Heist Bosses from Unique contracts.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsHeistBossDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "LeagueHeist");
        }
        #endregion

        #region "Incurion"
        /// <summary>
        /// Drops or is upgraded in the Temple of Atzoatl.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsIncursionDrop(UniqueItemWikiModel pUnique)
        {
            return IsCorruptionAltarUpgrade(pUnique) || IsIncurionBossDrop(pUnique) || IsIncursionRoomDrop(pUnique);
        }

        /// <summary>
        /// Upgrades from double corrupting on the Corruption Altar.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsCorruptionAltarUpgrade(UniqueItemWikiModel pUnique)
        {
            // Shadowstitch is a special upgrade that require double corrupting a Sacrificial Garb
            return pUnique.Id == 133888;
        }

        /// <summary>
        /// Drops from the Vaal Omnitect in the Temple of Atzoatl.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsIncurionBossDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "VaalSaucerBoss");
        }

        /// <summary>
        /// Drops from Temple of Atzoatl rooms.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsIncursionRoomDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Incursion Room");
        }

        /// <summary>
        /// Upgraded in the Temple of Atzoalt with a Vial.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsIncursionUpgrade(UniqueItemWikiModel pUnique)
        {
            // All have to be manually added.
            // Apep's Supremacy, Coward's Legacy, Slavedriver's Hand, Fate of the Vaal, Mask of the Stitched Demon, Omeyocan,
            // Transcendent Flesh, Trascendent Spirit, Trascendent Mind, Zerphi's Heart, Soul Ripper
            uint[] incursionUniques = [137879, 146263, 133922, 133204, 133532, 133610, 134357, 134358, 134359, 134520, 133938];
            return incursionUniques.Contains(pUnique.Id) || IsCorruptionAltarUpgrade(pUnique);
        }

        /// <summary>
        /// Drops in the Temple of Atzoatl.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsIncursionUpgradeable(UniqueItemWikiModel pUnique)
        {
            return IsIncursionRoomDrop(pUnique) || IsIncurionsNonUpgradeableBossDrop(pUnique);
        }

        /// <summary>
        /// Drops from the Vaal Omnitect boss, but is not upgradeable.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsIncurionsNonUpgradeableBossDrop(UniqueItemWikiModel pUnique)
        {
            if (!IsIncurionBossDrop(pUnique)) return false;
            // Ambition, String of Servitude are boss drops but aren't upgradeable
            return pUnique.Id != 140833 && pUnique.Id != 141939;
        }


        #endregion

        #region "Labyrinth"
        /// <summary>
        /// Drops in any Labyrinth.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsLabDrop(UniqueItemWikiModel pUnique)
        {
            if (BaseUtils.ContainsIgnoreCase(pUnique.DropText, "labyrinth")) return true;
            // Death's Door needs to be added manually
            return pUnique.Id == 133059;
        }
        #endregion

        #region "Legion"
        /// <summary>
        /// Drops from Legion encounters or from the Domain of Timeless Conflict.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsLegionDrop(UniqueItemWikiModel pUnique)
        {
            return IsLegionGeneralDrop(pUnique) || IsDomainDrop(pUnique);
        }

        /// <summary>
        /// Drops from Legion generals.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsLegionGeneralDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "LeagionLeague");
        }

        /// <summary>
        /// Drops from the Domain of Timeless Conflict.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsDomainDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "Domain of Timeless Conflict");
        }
        #endregion

        #region "Ritual"
        /// <summary>
        /// Found in Ritual Altars.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsRitualDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "[[Ritual]]s");
        }
        #endregion

        #region "Sanctum"
        /// <summary>
        /// Drops Lycia in Sanctum.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsSanctumDrop(UniqueItemWikiModel pUnique)
        {
            return IsSanctumWithoutRelicDrop(pUnique) || IsSanctumWithRelicDrop(pUnique);
        }

        /// <summary>
        /// Drops from Lycia in Sanctum without a unique relic.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsSanctumWithoutRelicDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "[[Lycia, Herald of the Scourge]] in a [[Sanctum]]");
        }

        /// <summary>
        /// Drops from Lycia in Sanctum while affected by a unique relic.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsSanctumWithRelicDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "is applied to a [[Sanctum]]");
        }
        #endregion

        #region "Tier 17"
        /// <summary>
        /// Drops from a Tier 17 map boss.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsTier17BossDrop(UniqueItemWikiModel pUnique)
        {
            string[] t17Maps = ["Abomination Map", "Citadel Map", "Fortress Map", "Sanctuary Map", "Ziggurat Map"];
            return BaseUtils.ContainsAnyIgnoreCase(pUnique.DropText, [.. t17Maps]);
        }
        #endregion

        #region "Ultimatum"
        /// <summary>
        /// Drops from Ultimatum Trials or the Trialmaster.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsUltimatumDrop(UniqueItemWikiModel pUnique)
        {
            return IsUltimatumTrialDrop(pUnique) || IsTrialMasterDrop(pUnique);
        }

        /// <summary>
        /// Dropped by Ultimatum Trials.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsUltimatumTrialDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsIgnoreCase(pUnique.DropText, "[[Ultimatum]] encounters");
        }

        /// <summary>
        /// Dropped by the Trialmaster.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsTrialMasterDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "LeagueUltimatum");
        }
        #endregion

        #region "Warbands"
        /// <summary>
        /// Drops from Warbands boss.
        /// </summary>
        /// <param name="pUnique"></param>
        /// <returns></returns>
        public static bool IsWarbandsBossDrop(UniqueItemWikiModel pUnique)
        {
            return BaseUtils.ContainsSubIgnoreCase(pUnique.DropMonsters, "Wb\\/Wb");
        }
        #endregion

    }
}
