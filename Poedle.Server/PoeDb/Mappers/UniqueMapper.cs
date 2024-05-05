using System.Text.RegularExpressions;

using Poedle.PoeDb.Models;
using Poedle.PoeWiki.Models;
using Poedle.Enums;
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
                DropSources = GetDropSources(pUnique)
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
            if (cleanedText.Contains("(lore)|"))
            {
                cleanedText = LoreEmbedRegex().Replace(cleanedText, "");
            }
            // Clean up the atziri and breach blessing upgrade special texts
            if (cleanedText.Contains("-help"))
            {
                cleanedText = HTMLTagCleaner.ReplaceSpanClassTags(cleanedText);
            }
            // Clean up the harbinger glyphs
            if (cleanedText.Contains("class=\"glyph"))
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
            if (cleanedText.Contains("-unique"))
            {
                cleanedText = HTMLTagCleaner.ReplaceSpanClassTags(cleanedText, "\n");
            }

            return StringCleaner.SeparateStringLines(cleanedText);
        }

        private static List<QualitiesEnum.Qualities> GetQualities(PoeWikiUnique pUnique)
        {
            List<QualitiesEnum.Qualities> qualities = [];

            ConditionalAddToList(qualities, QualitiesEnum.Qualities.CORRUPTED, pUnique.IsCorrupted);
            ConditionalAddToList(qualities, QualitiesEnum.Qualities.FRACTURED, pUnique.IsFractured);
            ConditionalAddToList(qualities, QualitiesEnum.Qualities.REPLICA, pUnique.IsReplica);
            ConditionalAddToList(qualities, QualitiesEnum.Qualities.SYNTHESISED, pUnique.IsSynthesised);
            ConditionalAddToList(qualities, QualitiesEnum.Qualities.VEILED, pUnique.IsVeiled);
            ConditionalAddToList(qualities, QualitiesEnum.Qualities.EATER_OF_WORLDS, pUnique.IsEaterOfWorlds);
            ConditionalAddToList(qualities, QualitiesEnum.Qualities.SEARING_EXARCH, pUnique.IsSearingExarch);
            ConditionalAddToList(qualities, QualitiesEnum.Qualities.ELDER, pUnique.Influences.Contains("elder"));
            ConditionalAddToList(qualities, QualitiesEnum.Qualities.SHAPER, pUnique.Influences.Contains("shaper"));

            return qualities;
        }

        private static List<DropSourcesEnum.DropSources> GetDropSources(PoeWikiUnique pUnique)
        {
            List<DropSourcesEnum.DropSources> drops = [];

            ConditionalAddToList(drops, DropSourcesEnum.DropSources.BOSS_DROP, pUnique.DropMonsters.Count > 0);

            return drops;
        }

        private static void ConditionalAddToList<T>(List<T> pList, T pValue, bool pCondition)
        {
            if (pCondition)
            {
                pList.Add(pValue);
            }
        }


    }
}
