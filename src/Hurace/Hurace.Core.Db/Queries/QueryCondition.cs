using Hurace.Core.Db.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable IDE0046 // Convert to conditional expression
namespace Hurace.Core.Db.Queries
{
    internal sealed class QueryCondition : IQueryCondition
    {
        public string ColumnToCheck { get; set; }
        public QueryConditionType ConditionType { get; set; }
        public object CompareValue { get; set; }

        public void AppendTo(StringBuilder queryStringBuilder, IList<QueryParameter> queryParameters)
        {
            if (queryStringBuilder is null)
                throw new ArgumentNullException(nameof(queryStringBuilder));
            else if (queryParameters is null)
                throw new ArgumentNullException(nameof(queryParameters));

            queryStringBuilder.Append($"[{ColumnToCheck}] ");

            switch (ConditionType)
            {
                case QueryConditionType.Equals:
                    queryStringBuilder.Append("=");
                    break;
                case QueryConditionType.NotEquals:
                    queryStringBuilder.Append("!=");
                    break;
                case QueryConditionType.GreaterThan:
                    queryStringBuilder.Append(">");
                    break;
                case QueryConditionType.GreaterThanOrEquals:
                    queryStringBuilder.Append(">=");
                    break;
                case QueryConditionType.Like:
                    queryStringBuilder.Append("LIKE");
                    break;
                case QueryConditionType.LessThan:
                    queryStringBuilder.Append("<");
                    break;
                case QueryConditionType.LessThanOrEquals:
                    queryStringBuilder.Append("<=");
                    break;
                default:
                    throw new InvalidOperationException(nameof(ConditionType));
            }

            queryStringBuilder.Append(' ');

            var newQueryParameter =
                queryParameters.AddQueryParameter(
                    ColumnToCheck,
                    CompareValue);

            queryStringBuilder.Append($"@{newQueryParameter.ParameterName}");
        }
    }
}
