using System.Text;

namespace PoeWikiData.Utils.SQL
{
    internal class SQLiteQueryBuilder
    {
        private bool? _isDistinct = false;
        private uint? _top = null;
        private string _fields = string.Empty;
        private string _table = string.Empty;
        private string _where = string.Empty;
        private string _groupBy = string.Empty;
        private string _orderBy = string.Empty;
        private bool? _isAsc = false;

        public SQLiteQueryBuilder() { }

        public SQLiteQueryBuilder Select(string? pFields, bool pIsAppend = false)
        {
            _fields = StringUtils.SetOrAppend(_fields, pFields, pIsAppend);
            return this;
        }

        public SQLiteQueryBuilder Distinct(bool? pIsDistinct = true)
        {
            _isDistinct = pIsDistinct;
            return this;
        }

        public SQLiteQueryBuilder Top(uint? pTop)
        {
            _top = pTop;
            return this;
        }

        public SQLiteQueryBuilder From(string? pTable, bool pIsAppend = false)
        {
            _table = StringUtils.SetOrAppend(_table, pTable, pIsAppend);
            return this;
        }

        public SQLiteQueryBuilder Where(string? pConditions, bool pIsAppend = false)
        {
            _where = StringUtils.SetOrAppend(_where, pConditions, pIsAppend);
            return this;
        }

        public SQLiteQueryBuilder GroupBy(string? pGroups, bool pIsAppend = false)
        {
            _groupBy = StringUtils.SetOrAppend(_groupBy, pGroups, pIsAppend);
            return this;
        }

        public SQLiteQueryBuilder OrderBy(string? pOrder, bool pIsAppend = false)
        {
            _orderBy = StringUtils.SetOrAppend(_orderBy, pOrder, pIsAppend);
            return this;
        }

        public SQLiteQueryBuilder Asc(bool? pIsAsc)
        {
            _isAsc = pIsAsc;
            return this;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(_table))
            {
                throw new Exception($"{_table} needs a value.");
            }

            StringBuilder queryBuilder = new();

            // SELECT FIELDS FROM TABLE
            queryBuilder.Append("SELECT ");
            if (_isDistinct != null && _isDistinct.Value)
            {
                queryBuilder.Append($"DISTINCT ");
            }

            if (_top != null && _top > 0)
            {
                queryBuilder.Append($"TOP {_top} ");
            }

            queryBuilder.Append(string.IsNullOrWhiteSpace(_fields) ? "* " : $"{_fields} ");
            queryBuilder.Append($"FROM {_table} ");

            // WHERE X GROUP BY Y ORDER BY Z
            if (!string.IsNullOrWhiteSpace(_where))
            {
                queryBuilder.Append($"WHERE {_where} ");
            }

            if (!string.IsNullOrWhiteSpace(_groupBy))
            {
                queryBuilder.Append($"GROUP BY {_groupBy} ");
            }

            if (!string.IsNullOrWhiteSpace(_orderBy))
            {
                queryBuilder.Append($"ORDER BY {_orderBy} ");
                if (_isAsc != null)
                {
                    queryBuilder.Append(_isAsc.Value ? "ASC" : "DESC");
                }
            }

            return queryBuilder.ToString();
        }
    }
}
