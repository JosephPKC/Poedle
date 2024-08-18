using System.Data.SQLite;
using System.Reflection;
using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiData.Mappers.Links;
using PoeWikiData.Mappers.SkillGems;
using PoeWikiData.Models;
using PoeWikiData.Models.Links;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Models.UniqueItems;
using PoeWikiData.Schema;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Endpoints.UniqueItems
{
    internal class SkillGemDbEndpoint(SQLiteConnection pSQLite, CacheHandler<string, IEnumerable<BaseDbModel>> pCache, ConsoleLogger pLog) : BaseDbEndpoint(pSQLite, pCache, pLog)
    {
        #region "Update"
        public void Update(PoeWikiManager pApi)
        {
            IEnumerable<PoeDbSchemaTypes> linkSchema =
            [
                PoeDbSchemaTypes.SkillGems_GemTags
            ];

            FullUpdate("SKILL GEMS", PoeDbSchemaTypes.SkillGems, linkSchema, null, pApi.SkillGems.GetAll(), SkillGemDbMapper.Map, SkillGemSQLiteMapper.Map, UpdateAllLinks);
        }

        private void UpdateAllLinks(SkillGemDbModel pModel)
        {
            UpdateTableLinks("Gem Tags", PoeDbSchemaTypes.SkillGems_GemTags, pModel, pModel.GemTags, LinkSQLiteMapper.MapLink);
        }
        #endregion

        #region "Select"
        public SkillGemDbModel? Select(uint pId)
        {
            string where = $"UniqueItemId={SQLiteUtils.SQLiteString(pId.ToString())}";
            SkillGemDbModel? model = SelectOne(PoeDbSchemaTypes.SkillGems, SkillGemSQLiteMapper.Read, where);
            if (model == null) return null;

            AddAllLinkedData(model);
            return model;
        }

        public SkillGemDbLookup SelectAll()
        {
            IEnumerable<SkillGemDbModel> allModels = SelectAll(PoeDbSchemaTypes.SkillGems, SkillGemSQLiteMapper.Read);
            foreach (SkillGemDbModel model in allModels)
            {
                AddAllLinkedData(model);
            }

            return new(allModels);
        }

        protected void AddAllLinkedData(SkillGemDbModel pModel)
        {
            static IEnumerable<StaticDataDbModel> GetGemTags(IEnumerable<LinkDbModel> pLinks) => DbLinker.GetStaticData(pLinks, StaticDataMasterRef.GemTags);
            pModel.GemTags = SelectLinks(PoeDbSchemaTypes.SkillGems_GemTags, LinkDbReader.Read, GetGemTags, "SkillGemId", pModel.Id);
        }

        #endregion
    }
}
