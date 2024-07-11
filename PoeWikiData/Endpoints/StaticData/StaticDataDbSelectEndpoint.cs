using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiData.Models;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Utils;

namespace PoeWikiData.Endpoints.StaticData
{
    internal class StaticDataDbSelectEndpoint(PoeDbHandler pDb, CacheHandler<string, BaseModel> pCache, ConsoleLogger pLogger) : BaseDbSelectEndpoint(pDb, pCache, pLogger)
    {
        public StaticDataDbModelList SelectAllDropSources()
        {
            return _db.SelectStaticData(PoeDbSchemaList.SchemaList[PoeDbSchemaList.PoeDbTypes.DropSources].TableName);
        }

        public StaticDataDbModelList SelectAllDropTypes()
        {

            return _db.SelectStaticData(PoeDbSchemaList.SchemaList[PoeDbSchemaList.PoeDbTypes.DropTypes].TableName);
        }

        public StaticDataDbModelList SelectAllItemAspects()
        {
            return _db.SelectStaticData(PoeDbSchemaList.SchemaList[PoeDbSchemaList.PoeDbTypes.ItemAspects].TableName);
        }

        public StaticDataDbModelList SelectAllItemClasses()
        {
            return _db.SelectStaticData(PoeDbSchemaList.SchemaList[PoeDbSchemaList.PoeDbTypes.ItemClasses].TableName);
        }
    }
}
