using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Db.Queries
{
    public interface IQueryCondition
    {
        /// <summary>
        /// Append the queryCondition encoded as string to a <see cref="StringBuilder"/>-instance and
        /// add newly created parameters to a <see cref="IList{QueryParameter}"/>-instance.
        /// </summary>
        /// <param name="queryStringBuilder">query to add the condition to</param>
        /// <param name="queryParameters">query parameter store</param>
        void AppendTo(StringBuilder queryStringBuilder, IList<QueryParameter> queryParameters);
    }
}
