using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurace.Core.Db.Extensions
{
    public static class QueryParameterListExtensions
    {
        /// <summary>
        /// Creates a new <see cref="QueryParameter"/>, adds it to the list and returns it.
        /// Its recommended to use this method because parametername-conflicts are resolved here.
        /// </summary>
        /// <param name="queryParameters">QueryParameter store</param>
        /// <param name="parameterAppliedColumn">information to which column the new <see cref="QueryParameter"/> gets applied</param>
        /// <param name="parameterValue">the value of the new <see cref="QueryParameter"/></param>
        /// <returns>the newly created <see cref="QueryParameter"/></returns>
        public static QueryParameter AddQueryParameter(
            this IList<QueryParameter> queryParameters,
            string parameterAppliedColumn,
            object parameterValue)
        {
            if (queryParameters is null)
                throw new ArgumentNullException(nameof(queryParameters));
            if (parameterValue is null)
                throw new ArgumentNullException(nameof(parameterValue));

            int countOfSimilarParameters =
                queryParameters.Count(
                    qp => qp.ParameterName.StartsWith(parameterAppliedColumn, StringComparison.OrdinalIgnoreCase));

            string uniqueParameterName = $"{parameterAppliedColumn}{countOfSimilarParameters}";

            var newQueryParameter = new QueryParameter(uniqueParameterName, parameterValue);
            queryParameters.Add(newQueryParameter);
            return newQueryParameter;
        }
    }
}
