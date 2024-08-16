using System.Data.SQLite;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiApi.Models;
using PoeWikiData.Mappers.Links;
using PoeWikiData.Mappers.UniqueItems;
using PoeWikiData.Models;
using PoeWikiData.Models.Leagues;
using PoeWikiData.Models.Links;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Models.UniqueItems;
using PoeWikiData.Schema;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Endpoints.UniqueItems
{
    internal class UniqueItemDbEndpoint(SQLiteConnection pSQLite, CacheHandler<string, IEnumerable<BaseDbModel>> pCache, ConsoleLogger pLog) : BaseDbEndpoint(pSQLite, pCache, pLog)
    {
        #region "Update"
        public void Update(PoeWikiManager pApi, LeagueDbLookUp pAllLeagues)
        {
            UniqueItemDbModel? getDbModelFromWikiData(UniqueItemWikiModel pModel) => UniqueItemDbMapper.Map(pModel, pAllLeagues);

            IEnumerable<PoeDbSchemaTypes> linkSchema =
            [
                PoeDbSchemaTypes.UniqueItems_DropSources,
                PoeDbSchemaTypes.UniqueItems_DropTypes,
                PoeDbSchemaTypes.UniqueItems_ItemAspects,
                PoeDbSchemaTypes.UniqueItems_LeaguesIntroduced,
                PoeDbSchemaTypes.UniqueItems_FlavourTexts,
                PoeDbSchemaTypes.UniqueItems_ImplicitStatTexts,
                PoeDbSchemaTypes.UniqueItems_ExplicitStatTexts
            ];

            FullUpdate("UNIQUE ITEMS", PoeDbSchemaTypes.UniqueItems, linkSchema, null, pApi.UniqueItems.GetAll(), getDbModelFromWikiData, UniqueItemSQLiteMapper.Map, UpdateAllUniqueItemLinks);
        }

        private void UpdateAllUniqueItemLinks(UniqueItemDbModel pModel)
        {
            UpdateTableLinks("Drop Sources", PoeDbSchemaTypes.UniqueItems_DropSources, pModel, pModel.DropSources, UniqueItemSQLiteMapper.MapLink);
            UpdateTableLinks("Drop Types", PoeDbSchemaTypes.UniqueItems_DropTypes, pModel, pModel.DropTypes, UniqueItemSQLiteMapper.MapLink);
            UpdateTableLinks("Item Aspects", PoeDbSchemaTypes.UniqueItems_ItemAspects, pModel, pModel.ItemAspects, UniqueItemSQLiteMapper.MapLink);
            UpdateTableLinks("Leagues Introduced", PoeDbSchemaTypes.UniqueItems_LeaguesIntroduced, pModel, pModel.LeaguesIntroduced, UniqueItemSQLiteMapper.MapLink);
            UpdateTableLinksWithOrder("Flavour Texts", PoeDbSchemaTypes.UniqueItems_FlavourTexts, pModel, pModel.FlavourText, UniqueItemSQLiteMapper.MapLink);
            UpdateTableLinksWithOrder("Implicit Stat Texts", PoeDbSchemaTypes.UniqueItems_ImplicitStatTexts, pModel, pModel.ImplicitStatText, UniqueItemSQLiteMapper.MapLink);
            UpdateTableLinksWithOrder("Explicit Stat Texts", PoeDbSchemaTypes.UniqueItems_ExplicitStatTexts, pModel, pModel.ExplicitStatText, UniqueItemSQLiteMapper.MapLink);
        }
        #endregion

        #region "Select"
        public UniqueItemDbModel? Select(uint pId, LeagueDbLookUp pAllLeagues)
        {
            string where = $"UniqueItemId={SQLiteUtils.SQLiteString(pId.ToString())}";
            UniqueItemDbModel? model = SelectOne(PoeDbSchemaTypes.UniqueItems, UniqueItemSQLiteReader.Read, where);
            if (model == null) return null;

            AddAllLinkedData(model, pAllLeagues);
            return model;
        }

        public UniqueItemDbLookUp SelectAll(LeagueDbLookUp pAllLeagues)
        {
            IEnumerable<UniqueItemDbModel> allModels = SelectAll(PoeDbSchemaTypes.UniqueItems, UniqueItemSQLiteReader.Read);
            foreach (UniqueItemDbModel model in allModels)
            {
                AddAllLinkedData(model, pAllLeagues);
            }

            return new(allModels);
        }

        private void AddAllLinkedData(UniqueItemDbModel pModel, LeagueDbLookUp pAllLeagues)
        {
            IEnumerable<StaticDataDbModel> GetDropSources(IEnumerable<LinkDbModel> pLinks) => UniqueItemDbLinker.GetStaticData(pLinks, StaticDataMasterRef.DropSources);
            pModel.DropSources = SelectLinks(PoeDbSchemaTypes.UniqueItems_DropSources, LinkDbReader.Read, GetDropSources, "UniqueItemId", pModel.Id);

            IEnumerable<StaticDataDbModel> GetDropTypes(IEnumerable<LinkDbModel> pLinks) => UniqueItemDbLinker.GetStaticData(pLinks, StaticDataMasterRef.DropTypes);
            pModel.DropTypes = SelectLinks(PoeDbSchemaTypes.UniqueItems_DropTypes, LinkDbReader.Read, GetDropTypes, "UniqueItemId", pModel.Id);

            IEnumerable<StaticDataDbModel> GetItemAspects(IEnumerable<LinkDbModel> pLinks) => UniqueItemDbLinker.GetStaticData(pLinks, StaticDataMasterRef.ItemAspects);
            pModel.ItemAspects = SelectLinks(PoeDbSchemaTypes.UniqueItems_ItemAspects, LinkDbReader.Read, GetItemAspects, "UniqueItemId", pModel.Id);

            IEnumerable<LeagueDbModel> GetLeaguesIntroduced(IEnumerable<LinkDbModel> pLinks) => UniqueItemDbLinker.GetStaticData(pLinks, pAllLeagues);
            pModel.LeaguesIntroduced = SelectLinks(PoeDbSchemaTypes.UniqueItems_LeaguesIntroduced, LinkDbReader.Read, GetLeaguesIntroduced, "UniqueItemId", pModel.Id);

            IEnumerable<string> GetFlavourTexts(IEnumerable<TextLinkDbModel> pLinks) => UniqueItemDbLinker.GetTexts(pLinks);
            pModel.FlavourText = SelectLinks(PoeDbSchemaTypes.UniqueItems_FlavourTexts, LinkDbReader.ReadText, GetFlavourTexts, "UniqueItemId", pModel.Id, "TextOrder", true);

            IEnumerable<string> GetImplicitStatTexts(IEnumerable<TextLinkDbModel> pLinks) => UniqueItemDbLinker.GetTexts(pLinks);
            pModel.ImplicitStatText = SelectLinks(PoeDbSchemaTypes.UniqueItems_ImplicitStatTexts, LinkDbReader.ReadText, GetImplicitStatTexts, "UniqueItemId", pModel.Id, "TextOrder", true);

            IEnumerable<string> GetExplicitStatTexts(IEnumerable<TextLinkDbModel> pLinks) => UniqueItemDbLinker.GetTexts(pLinks);
            pModel.ExplicitStatText = SelectLinks(PoeDbSchemaTypes.UniqueItems_ExplicitStatTexts, LinkDbReader.ReadText, GetExplicitStatTexts, "UniqueItemId", pModel.Id, "TextOrder", true);
        }
        #endregion
    }
}
