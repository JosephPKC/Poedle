using System.Text;

namespace PoeWikiData.Utils.SQLite
{
    internal class SQLiteColumnBuilder
    {
        private string _name = "";
        private string _type = "";

        private bool _isPrimaryKey = false;
        private bool _isAutoIncrement = false;
        private bool _isUnique = false;
        private bool _isNotNull = false;
        private CollateTypes _collateType = CollateTypes.None;

        private string _foreignTable = "";
        private string _foreignColumn = "";
        private ForeignKeyUpdateTypes _onDeleteType = ForeignKeyUpdateTypes.None;
        private ForeignKeyUpdateTypes _onUpdateType = ForeignKeyUpdateTypes.None;

        public enum CollateTypes
        {
            None,
            NoCase
        }

        public enum ForeignKeyUpdateTypes
        {
            None,
            Cascade
        }

        public SQLiteColumnBuilder Name(string pColumnName, string pType)
        {
            _name = pColumnName;
            _type = pType;
            return this;
        }

        public SQLiteColumnBuilder PrimaryKey(bool pIsPrimaryKey = true, bool pIsAutoIncrement = false)
        {
            _isPrimaryKey = pIsPrimaryKey;
            _isAutoIncrement = pIsAutoIncrement;
            return this;
        }

        public SQLiteColumnBuilder Unique(bool pIsUnique = true)
        {
            _isUnique = pIsUnique;
            return this;
        }

        public SQLiteColumnBuilder NotNull(bool pIsNotNull = true)
        {
            _isNotNull = pIsNotNull;
            return this;
        }

        public SQLiteColumnBuilder Collate(CollateTypes pCollateType)
        {
            _collateType = pCollateType;
            return this;
        }

        public SQLiteColumnBuilder ForeignKey(string pForeignTable, string pForeignColumn, ForeignKeyUpdateTypes pOnDelete = ForeignKeyUpdateTypes.None, ForeignKeyUpdateTypes pOnUpdate = ForeignKeyUpdateTypes.None)
        {
            _foreignTable = pForeignTable; 
            _foreignColumn = pForeignColumn;
            _onDeleteType = pOnDelete;
            _onUpdateType = pOnUpdate;
            return this;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                throw new Exception($"{_name} needs a value.");
            }

            if (string.IsNullOrWhiteSpace(_type))
            {
                throw new Exception($"{_type} needs a value.");
            }

            StringBuilder columnBuilder = new();
            columnBuilder.Append($"{_name} {_type.ToUpper()}");

            // PRIMARY KEY
            if (_isPrimaryKey)
            {
                columnBuilder.Append(" PRIMARY KEY");
                if (_isAutoIncrement)
                {
                    columnBuilder.Append(" AUTOINCREMENT");
                }
            }

            // FOREIGN KEY
            if (!string.IsNullOrWhiteSpace(_foreignTable) && !string.IsNullOrWhiteSpace(_foreignColumn))
            {
                columnBuilder.Append($" FOREIGN KEY {_foreignTable} ({_foreignColumn})");
                if (_onDeleteType != ForeignKeyUpdateTypes.None)
                {
                    columnBuilder.Append($" ON DELETE {_onDeleteType.ToString().ToUpper()}");
                }

                if (_onUpdateType != ForeignKeyUpdateTypes.None)
                {
                    columnBuilder.Append($" ON UPDATE {_onUpdateType.ToString().ToUpper()}");
                }
            }

            // OTHER FLAGS
            if (_isUnique)
            {
                columnBuilder.Append(" UNIQUE");
            }

            if (_collateType != CollateTypes.None)
            {
                columnBuilder.Append($" COLLATE {_collateType.ToString().ToUpper()}");
            }

            if (_isNotNull)
            {
                columnBuilder.Append(" NOT NULL");
            }

            return columnBuilder.ToString();
        }
    }
}
