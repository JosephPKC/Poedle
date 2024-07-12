namespace PoeWikiData.Schema
{
    internal static class PoeDbSchemaManager
    {
        public static PoeDbSchema GetSchema(PoeDbSchemaTypes pSchemaType)
        {
            return PoeDbSchemaMasterRef.SchemaList[pSchemaType];
        }
    }
}
