using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    public abstract class QueryConditionBase : IQueryCondition
    {
        public (string, IEnumerable<QueryParameter>) Build()
        {
            var conditionStringBuilder = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            this.AppendTo(conditionStringBuilder, queryParameters);

            return (conditionStringBuilder.ToString(), queryParameters);
        }

        internal abstract void AppendTo(
            StringBuilder conditionStringBuilder,
            IList<QueryParameter> queryParameters);
    }
}
