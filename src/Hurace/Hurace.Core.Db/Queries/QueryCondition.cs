using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    public class QueryCondition : IQueryCondition
    {
        public enum Type
        {
            Equals,
            NotEquals,
            LowerThan,
            LowerThanOrEquals,
            GreaterThan,
            GreaterThanOrEquals,
            Like
        }

        public string ColumnToCheck { get; set; }
        public Type ConditionType { get; set; }
        public object CompareValue { get; set; }

        public void Build(StringBuilder queryBuilder)
        {
            if (queryBuilder is null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            queryBuilder.Append($"[{ColumnToCheck}] ");

            switch (ConditionType)
            {
                case Type.Equals:
                    queryBuilder.Append("=");
                    break;
                case Type.NotEquals:
                    queryBuilder.Append("!=");
                    break;
                case Type.GreaterThan:
                    queryBuilder.Append(">");
                    break;
                case Type.GreaterThanOrEquals:
                    queryBuilder.Append(">=");
                    break;
                case Type.Like:
                    queryBuilder.Append("LIKE");
                    break;
                case Type.LowerThan:
                    queryBuilder.Append("<");
                    break;
                case Type.LowerThanOrEquals:
                    queryBuilder.Append("<=");
                    break;
                default:
                    throw new InvalidOperationException(nameof(ConditionType));
            }

            queryBuilder.Append(' ');

            var useApostrophesForValue = this.WriteValueInApostrophes();
            if (useApostrophesForValue)
                queryBuilder.Append("'");

            queryBuilder.Append(CompareValue);

            if (useApostrophesForValue)
                queryBuilder.Append("'");
        }

        private bool WriteValueInApostrophes()
        {
            return this.CompareValue is string
                || this.CompareValue is char;
        }
    }
}
