using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    public sealed class QueryConditionCombination : QueryConditionBase
    {
        public enum Type
        {
            And,
            Or
        }

        public QueryConditionBase FirstCondition { get; set; }
        public QueryConditionBase SecondCondition { get; set; }
        public Type CombinationType { get; set; }

        internal override void AppendTo(StringBuilder conditionStringBuilder, IList<QueryParameter> queryParameters)
        {
            if (conditionStringBuilder is null)
                throw new ArgumentNullException(nameof(conditionStringBuilder));
            else if (queryParameters is null)
                throw new ArgumentNullException(nameof(queryParameters));
            else if (FirstCondition == null)
                throw new InvalidOperationException(nameof(FirstCondition));
            else if (SecondCondition == null)
                throw new InvalidOperationException(nameof(SecondCondition));

            conditionStringBuilder.Append("(");
            FirstCondition.AppendTo(conditionStringBuilder, queryParameters);

            switch (CombinationType)
            {
                case Type.And:
                    conditionStringBuilder.Append(" AND ");
                    break;
                case Type.Or:
                    conditionStringBuilder.Append(" OR ");
                    break;
                default:
                    throw new InvalidOperationException(nameof(CombinationType));
            }

            SecondCondition.AppendTo(conditionStringBuilder, queryParameters);
            conditionStringBuilder.Append(")");
        }
    }
}
