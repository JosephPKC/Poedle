using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiApi.Models;
using PoeWikiData.Endpoints.Leagues;
using PoeWikiData.Endpoints.StaticData;
using PoeWikiData.Mappers.UniqueItems;
using PoeWikiData.Models;
using PoeWikiData.Models.UniqueItems;
using PoeWikiData.Utils;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Endpoints.UniqueItems
{
    internal class UniqueItemDbUpdateEndpoint(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger) : BaseDbUpdateEndpoint(pDb, pCache, pLogger)
    {
        public void Update(PoeWikiManager pApi, ReferenceDataModelGroup pRefData)
        {
            static List<UniqueItemWikiModel> getWikiDataFromApi(PoeWikiManager pApi) => pApi.UniqueItems.GetAll();
            UniqueItemDbModel? getDbModelFromWikiData(UniqueItemWikiModel pModel) => UniqueItemDbMapper.Map(pModel, pRefData);
            SQLiteValues? getSQLValuesFromDbModel(UniqueItemDbModel pModel) => UniqueItemSQLiteMapper.Map(pModel);
            void doPostProcessing(UniqueItemDbModel pModel) => UpdateAllUniqueItemLinks(pModel);

            PoeDbSchema schema = PoeDbSchemaList.SchemaList[PoeDbSchemaList.PoeDbTypes.UniqueItems];
            FullyUpdateTable("UNIQUE ITEMS", schema, null, pApi, getWikiDataFromApi, getDbModelFromWikiData, getSQLValuesFromDbModel, doPostProcessing);
        }


        private void UpdateAllUniqueItemLinks(UniqueItemDbModel pModel)
        {
            // Drop Sources, Drop Types, Item Aspects, League Introduced
            var DropSourcesList = new List<string>();

        }
    }
}
