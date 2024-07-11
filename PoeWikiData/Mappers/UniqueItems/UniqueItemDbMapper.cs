using PoeWikiApi.Models;
using PoeWikiData.Models;
using PoeWikiData.Models.Enums;
using PoeWikiData.Models.Leagues;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Models.UniqueItems;
using PoeWikiData.Utils;

namespace PoeWikiData.Mappers.UniqueItems
{
    internal static class UniqueItemDbMapper
    {
        public static UniqueItemDbModel Map(UniqueItemWikiModel pModel, ReferenceDataModelGroup pRefData)
        {
            return new()
            {
                Id = pModel.Id,
                Name = pModel.Name,
                BaseItem = pModel.BaseItem,
                ItemClass = GetItemClass(pModel.ItemClass, pRefData.ItemClasses),
                ReqLvl = pModel.ReqLvl,
                ReqDex = pModel.ReqDex,
                ReqInt = pModel.ReqInt,
                ReqStr = pModel.ReqStr,
                LeaguesIntroduced = GetLeagues(pModel.ReleaseVersion, pRefData.Leagues),
                ItemAspects = GetItemAspects(pModel, pRefData.ItemAspects),
                DropSources = [],
                DropTypes = []
            };
        }

        private static StaticDataDbModel GetItemClass(string pItemClass, StaticDataDbModelList? pItemClasses)
        {
            if (pItemClasses == null) return new();

            return new()
            {
                Id = pItemClasses.GetId(pItemClass) ?? default,
                Name = pItemClass
            };
        }

        private static List<LeagueDbModel> GetLeagues(string pReleaseVersion, LeagueDbModelList? pAllLeagues)
        {
            if (pAllLeagues == null) return [];

            string[] versionSplit = pReleaseVersion.Split('.');
            if (versionSplit.Length < 3) return [];

            List<string> leagues = pAllLeagues.GetLeagues($"{versionSplit[0]}.{versionSplit[1]}.0");
            List<LeagueDbModel> leagueModels = [];
            foreach (string league in leagues)
            {
                LeagueDbModel? model = pAllLeagues.GetModelByName(league);
                if (model == null) continue;

                leagueModels.Add(model);
            }

            return leagueModels;
        }

        private static List<StaticDataDbModel> GetItemAspects(UniqueItemWikiModel pModel, StaticDataDbModelList? pItemAspects)
        {
            if (pItemAspects == null) return [];

            List<StaticDataDbModel> result = [];
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Abyssal, pItemAspects), UniqueTagsParser.IsAbyssal(pModel));
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Anointable, pItemAspects), UniqueTagsParser.IsAnointable(pModel));
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Corrupted, pItemAspects), pModel.IsCorrupted);
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.EaterOfWorlds, pItemAspects), pModel.IsEaterOfWorlds);
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Elder, pItemAspects), BaseUtils.ContainsIgnoreCase(pModel.Influences, "Elder"));
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Fractured, pItemAspects), pModel.IsFractured);
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.GrantsSkill, pItemAspects), UniqueTagsParser.IsGrantsSkills(pModel));
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Replica, pItemAspects), pModel.IsReplica);
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.SearingExarch, pItemAspects), pModel.IsSearingExarch);
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Shaper, pItemAspects), BaseUtils.ContainsIgnoreCase(pModel.Influences, "Shaper"));
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Synthesised, pItemAspects), pModel.IsSynthesised);
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.TriggersSkill, pItemAspects), UniqueTagsParser.IsTriggersSkill(pModel));
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Upgradeable, pItemAspects), UniqueTagsParser.IsUpgradeable(pModel));
            BaseUtils.ConditionalAddToList(result, GetItemAspectModel(ItemAspects.Veiled, pItemAspects), pModel.IsVeiled);
            return result;
        }

        private static StaticDataDbModel GetItemAspectModel(ItemAspects pAspect, StaticDataDbModelList pItemAspects)
        {
            if (!pItemAspects.HasId((uint)pAspect) || !pItemAspects.HasName(pAspect.ToString()))
            {
                throw new Exception($"Item Aspect Enum {pAspect} is invalid. Make sure the db table is updated.");
            }

            return new()
            {
                Id = pItemAspects.GetId(pAspect.ToString()) ?? 0,
                Name = pItemAspects.GetName((uint)pAspect)
            };
        }
    }
}
