using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hurace.Core.Db.Extensions
{
    public static class IListOfQueryParameterExtensions
    {
        public const string InsertValueParameterPrefix = "IV";
        public const string WhereConditionParameterPrefix = "WC";
        public const char ParameterSegmentationChar = '_';

        public static QueryParameter AddQueryParameter(
            this IList<QueryParameter> queryParameters,
            string columName,
            object value,
            QueryParameterType parameterType)
        {
            if (queryParameters is null)
                throw new ArgumentNullException(nameof(queryParameters));
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            var parameterPrefix = parameterType switch
            {
                QueryParameterType.InsertValueParameter => InsertValueParameterPrefix,
                QueryParameterType.WhereConditionParameter => WhereConditionParameterPrefix,
                _ => throw new InvalidOperationException($"ParameterType {parameterType} is not recognized"),
            };

            string genericParameterName = $"{parameterPrefix}{ParameterSegmentationChar}{columName}";
            
            int countOfSimilarParameters =
                queryParameters.Count(
                    qp => qp.ParameterName.StartsWith(genericParameterName, StringComparison.OrdinalIgnoreCase));

            string uniqueParameterName = $"{genericParameterName}{ParameterSegmentationChar}{countOfSimilarParameters}";

            var newQueryParameter = new QueryParameter(uniqueParameterName, value);
            queryParameters.Add(newQueryParameter);
            return newQueryParameter;
        }
    }
}
