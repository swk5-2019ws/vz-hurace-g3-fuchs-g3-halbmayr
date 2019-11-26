using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable IDE0046 // Convert to conditional expression
namespace Hurace.Core.Db.Queries
{
    public class QueryCondition : IQueryCondition
    {
        public enum Type
        {
            Equals,
            NotEquals,
            LessThan,
            LessThanOrEquals,
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
                case Type.LessThan:
                    queryBuilder.Append("<");
                    break;
                case Type.LessThanOrEquals:
                    queryBuilder.Append("<=");
                    break;
                default:
                    throw new InvalidOperationException(nameof(ConditionType));
            }

            queryBuilder.Append(' ');

            var useApostrophesForValue = this.WriteValueInApostrophes();
            if (useApostrophesForValue)
                queryBuilder.Append("'");

            queryBuilder.Append(GetStringRepresentationOfCompareValue());

            if (useApostrophesForValue)
                queryBuilder.Append("'");
        }

        private bool WriteValueInApostrophes()
        {
            return this.CompareValue is string
                || this.CompareValue is char
                || this.CompareValue is DateTime;
        }

        private string GetStringRepresentationOfCompareValue()
        {
            if (CompareValue is DateTime compareValueDateTime)
                return compareValueDateTime.ToString("s");
            else if (CompareValue is bool compareValueBool)
                return compareValueBool ? "1" : "0";
            else
                return CompareValue.ToString();
        }
    }
}
