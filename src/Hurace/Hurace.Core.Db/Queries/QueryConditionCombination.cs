using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    public sealed class QueryConditionCombination : IQueryCondition
    {
        public enum Type
        {
            And,
            Or
        }

        public IQueryCondition FirstCondition { get; set; }
        public IQueryCondition SecondCondition { get; set; }
        public Type CombinationType { get; set; }

        public void AppendTo(StringBuilder queryStringBuilder, IList<QueryParameter> queryParameters)
        {
            if (queryStringBuilder is null)
                throw new ArgumentNullException(nameof(queryStringBuilder));
            else if (queryParameters is null)
                throw new ArgumentNullException(nameof(queryParameters));
            else if (FirstCondition == null)
                throw new InvalidOperationException(nameof(FirstCondition));
            else if (SecondCondition == null)
                throw new InvalidOperationException(nameof(SecondCondition));

            queryStringBuilder.Append("(");
            FirstCondition.AppendTo(queryStringBuilder, queryParameters);

            switch (CombinationType)
            {
                case Type.And:
                    queryStringBuilder.Append(" AND ");
                    break;
                case Type.Or:
                    queryStringBuilder.Append(" OR ");
                    break;
                default:
                    throw new InvalidOperationException(nameof(CombinationType));
            }

            SecondCondition.AppendTo(queryStringBuilder, queryParameters);
            queryStringBuilder.Append(")");
        }
    }
}
