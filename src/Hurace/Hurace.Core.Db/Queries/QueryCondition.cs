using Hurace.Core.Db.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable IDE0046 // Convert to conditional expression
namespace Hurace.Core.Db.Queries
{
    public sealed class QueryCondition : QueryConditionBase
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

        internal override void AppendTo(StringBuilder conditionStringBuilder, IList<QueryParameter> queryParameters)
        {
            if (conditionStringBuilder is null)
                throw new ArgumentNullException(nameof(conditionStringBuilder));
            else if (queryParameters is null)
                throw new ArgumentNullException(nameof(queryParameters));

            conditionStringBuilder.Append($"[{ColumnToCheck}] ");

            switch (ConditionType)
            {
                case Type.Equals:
                    conditionStringBuilder.Append("=");
                    break;
                case Type.NotEquals:
                    conditionStringBuilder.Append("!=");
                    break;
                case Type.GreaterThan:
                    conditionStringBuilder.Append(">");
                    break;
                case Type.GreaterThanOrEquals:
                    conditionStringBuilder.Append(">=");
                    break;
                case Type.Like:
                    conditionStringBuilder.Append("LIKE");
                    break;
                case Type.LessThan:
                    conditionStringBuilder.Append("<");
                    break;
                case Type.LessThanOrEquals:
                    conditionStringBuilder.Append("<=");
                    break;
                default:
                    throw new InvalidOperationException(nameof(ConditionType));
            }

            conditionStringBuilder.Append(' ');

            var newQueryParameter =
                queryParameters.AddQueryParameter(
                    ColumnToCheck,
                    CompareValue,
                    QueryParameterType.WhereConditionParameter);

            conditionStringBuilder.Append($"@{newQueryParameter.ParameterName}");
        }
    }
}
