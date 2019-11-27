using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hurace.Core.Db.Extensions
{
    public static class QueryParameterListExtensions
    {
        public static QueryParameter AddQueryParameter(
            this IList<QueryParameter> queryParameters,
            string columName,
            object value)
        {
            if (queryParameters is null)
                throw new ArgumentNullException(nameof(queryParameters));
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            int countOfSimilarParameters =
                queryParameters.Count(
                    qp => qp.ParameterName.StartsWith(columName, StringComparison.OrdinalIgnoreCase));

            string uniqueParameterName = $"{columName}{countOfSimilarParameters}";

            var newQueryParameter = new QueryParameter(uniqueParameterName, value);
            queryParameters.Add(newQueryParameter);
            return newQueryParameter;
        }
    }
}
