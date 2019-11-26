using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    public class QueryConditionCombination : IQueryCondition
    {
        public enum Type
        {
            And,
            Or
        }

        public IQueryCondition FirstCondition { get; set; }
        public IQueryCondition SecondCondition { get; set; }
        public Type CombinationType { get; set; }

        public void Build(StringBuilder queryBuilder)
        {
            if (queryBuilder is null)
                throw new ArgumentNullException(nameof(queryBuilder));
            else if (FirstCondition == null)
                throw new InvalidOperationException(nameof(FirstCondition));
            else if (SecondCondition == null)
                throw new InvalidOperationException(nameof(SecondCondition));

            queryBuilder.Append("(");
            FirstCondition.Build(queryBuilder);

            switch (CombinationType)
            {
                case Type.And:
                    queryBuilder.Append(" AND ");
                    break;
                case Type.Or:
                    queryBuilder.Append(" OR ");
                    break;
                default:
                    throw new InvalidOperationException(nameof(CombinationType));
            }

            SecondCondition.Build(queryBuilder);
            queryBuilder.Append(")");
        }
    }
}
