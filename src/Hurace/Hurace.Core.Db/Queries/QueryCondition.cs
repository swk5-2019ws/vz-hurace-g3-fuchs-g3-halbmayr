using Hurace.Core.Db.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable IDE0046 // Convert to conditional expression
namespace Hurace.Core.Db.Queries
{
    public sealed class QueryCondition : IQueryCondition
    {
        /// <summary>
        /// Describes how a <see cref="QueryCondition"/> compares a column to a concrete value
        /// </summary>
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

        public void AppendTo(StringBuilder queryStringBuilder, IList<QueryParameter> queryParameters)
        {
            if (queryStringBuilder is null)
                throw new ArgumentNullException(nameof(queryStringBuilder));
            else if (queryParameters is null)
                throw new ArgumentNullException(nameof(queryParameters));

            queryStringBuilder.Append($"[{ColumnToCheck}] ");

            switch (ConditionType)
            {
                case Type.Equals:
                    queryStringBuilder.Append("=");
                    break;
                case Type.NotEquals:
                    queryStringBuilder.Append("!=");
                    break;
                case Type.GreaterThan:
                    queryStringBuilder.Append(">");
                    break;
                case Type.GreaterThanOrEquals:
                    queryStringBuilder.Append(">=");
                    break;
                case Type.Like:
                    queryStringBuilder.Append("LIKE");
                    break;
                case Type.LessThan:
                    queryStringBuilder.Append("<");
                    break;
                case Type.LessThanOrEquals:
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
