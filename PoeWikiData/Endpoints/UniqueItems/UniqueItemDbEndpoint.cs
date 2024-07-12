using System.Data.SQLite;
using System.Reflection;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiApi.Models;
using PoeWikiData.Mappers.Leagues;
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
        public UniqueItemDbModel? Select(uint pId)
        {
            string where = $"UniqueItemId={SQLiteUtils.SQLiteString(pId.ToString())}";
            UniqueItemDbModel? model = SelectOne(PoeDbSchemaTypes.UniqueItems, UniqueItemSQLiteReader.Read, where);
            if (model == null) return null;

            AddLinkedData(model);
            return model;
        }

        public UniqueItemDbLookUp SelectAll()
        {
            IEnumerable<UniqueItemDbModel> allModels = SelectAll(PoeDbSchemaTypes.UniqueItems, UniqueItemSQLiteReader.Read);
            foreach (UniqueItemDbModel model in allModels)
            {
                AddLinkedData(model);
            }

            return new(allModels);
        }

        private void AddLinkedData(UniqueItemDbModel pModel)
        {
            // Get link data too
            // SELECT * FROM LINK TABLE WHERE UniqueItemId == {id}
            string where = $"UniqueItemId={SQLiteUtils.SQLiteString(pModel.Id.ToString())}";
            IEnumerable<LinkDbModel> links = SelectAll(PoeDbSchemaTypes.UniqueItems_DropSources, LinkDbReader.Read, where);
            UniqueItemDbLinker.AddDropSources(pModel, links);

            // Select texts, which have 3
            //SELECT * FROM TEXT LINK TABLE WHERE UniqueItemId == {id} ORDER BY Order ASC

            // SELECT * FROM LINK TABLE WHERE UniqueItemId == {id}
            where = $"UniqueItemId={SQLiteUtils.SQLiteString(pModel.Id.ToString())}";
            IEnumerable<TextLinkDbModel> textLinks = SelectAll(PoeDbSchemaTypes.UniqueItems_FlavourTexts, LinkDbReader.ReadText, where);
        }

        private IEnumerable<StaticDataDbModel> GetAllDropSources(SQLiteDataReader pReader, uint pModelId)
        {
            ICollection<StaticDataDbModel> allData = [];
            while (pReader.Read())
            {
                StaticDataDbModel data = new()
                {
                    Id = (uint)pReader.GetInt32(1),
                    Name = StaticDataMasterRef.DropSources.GetName((uint)pReader.GetInt32(1))
                };
                allData.Add(data);
            }
            return allData;
        }
        #endregion
    }
}
