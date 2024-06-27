namespace PoeWikiData.Endpoints.DbQueryParams
{
    internal static class CreateTableQueries
    {
        public enum ColumnDataTypes
        {
            None,
            Integer,
            Text
        }

        public class CreateTableColumn
        {
            public string Name { get; set; } = "";
            public ColumnDataTypes DataType { get; set; } = ColumnDataTypes.None;
            public PrimaryKeyAttribute? PrimaryKey { get; set; } = null;
            public ForeignKeyAttribute? ForeignKey { get; set; } = null;
            public bool IsUnique { get; set; } = false;
            public bool IsNotNull { get; set; } = false;
            public CollationNames Collate { get; set; } = CollationNames.None;
        }

        public class PrimaryKeyAttribute
        {
            public bool IsAutoIncrement { get; set; } = false;
        }

        public class ForeignKeyAttribute
        {
            public string ForeignTableName { get; set; } = "";
            public string ForeignColumnName { get; set; } = "";
            public ForeignKeyReactions OnUpdate { get; set; } = ForeignKeyReactions.None;
            public ForeignKeyReactions OnDelete { get; set; } = ForeignKeyReactions.None;
        }

        public enum ForeignKeyReactions
        {
            None,
            NoAction,
            SetNull,
            SetDefault,
            Cascade,
            Restrict
        }

        public enum CollationNames
        {
            None,
            RTrim,
            NoCase,
            Binary
        }
    }
}
