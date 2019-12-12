using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    internal sealed class QueryConditionNode : IQueryCondition
    {
        public IQueryCondition FirstCondition { get; set; }
        public IQueryCondition SecondCondition { get; set; }
        public QueryConditionNodeType NodeType { get; set; }

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

            switch (NodeType)
            {
                case QueryConditionNodeType.And:
                    queryStringBuilder.Append(" AND ");
                    break;
                case QueryConditionNodeType.Or:
                    queryStringBuilder.Append(" OR ");
                    break;
                default:
                    throw new InvalidOperationException(nameof(NodeType));
            }

            SecondCondition.AppendTo(queryStringBuilder, queryParameters);
            queryStringBuilder.Append(")");
        }
    }
}
