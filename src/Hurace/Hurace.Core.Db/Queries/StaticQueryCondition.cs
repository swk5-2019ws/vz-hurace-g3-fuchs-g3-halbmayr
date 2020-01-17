using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    internal class StaticQueryCondition : IQueryCondition
    {
        private readonly bool alwaysTrue;

        internal StaticQueryCondition(bool alwaysTrue)
        {
            this.alwaysTrue = alwaysTrue;
        }

        public void AppendTo(StringBuilder queryStringBuilder, IList<QueryParameter> queryParameters)
        {
            queryStringBuilder.Append($"1 = {(alwaysTrue ? 1 : 0)}");
        }
    }
}
