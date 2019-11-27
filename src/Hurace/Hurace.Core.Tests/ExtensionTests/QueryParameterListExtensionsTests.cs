using Hurace.Core.Db.Extensions;
using Hurace.Core.Db.Queries;
using Hurace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Hurace.Core.Tests.ExtensionTests
{
    public class QueryParameterListExtensionsTests
    {
        [Theory]
        [InlineData(QueryParameterType.InsertValueParameter)]
        [InlineData(QueryParameterType.WhereConditionParameter)]
        public void AddQueryParameterOnNullListTest(QueryParameterType parameterType)
        {
            IList<QueryParameter> unsetQueryParameterList = null;
            Assert.Throws<ArgumentNullException>(
                () => unsetQueryParameterList.AddQueryParameter("", new object(), parameterType));
        }

        [Theory]
        [InlineData(QueryParameterType.InsertValueParameter)]
        [InlineData(QueryParameterType.WhereConditionParameter)]
        public void AddQueryParameterWithNullValueTest(QueryParameterType parameterType)
        {
            var emptyQueryParameterList = new List<QueryParameter>();
            Assert.Throws<ArgumentNullException>(
                () => emptyQueryParameterList.AddQueryParameter("", null, parameterType));
        }

        [Theory]
        [InlineData(QueryParameterType.InsertValueParameter, 15)]
        [InlineData(QueryParameterType.WhereConditionParameter, 15)]
        [InlineData(QueryParameterType.InsertValueParameter, "Marcel")]
        [InlineData(QueryParameterType.WhereConditionParameter, "Marcel")]
        public void AddSingleParameterTest(QueryParameterType parameterType, object expectedColumnValue)
        {
            string columnName = "Testcolumn";
            var queryParameterList = new List<QueryParameter>();

            var returnedParameter = queryParameterList.AddQueryParameter(columnName, expectedColumnValue, parameterType);

            Assert.Single(queryParameterList);

            var parameter = queryParameterList.First();

            Assert.Equal(returnedParameter, parameter);
            Assert.Equal(expectedColumnValue, parameter.Value);

            var expectedParameterNamePrefix = parameterType switch
            {
                QueryParameterType.InsertValueParameter => QueryParameterListExtensions.InsertValueParameterPrefix,
                QueryParameterType.WhereConditionParameter => QueryParameterListExtensions.WhereConditionParameterPrefix,
                _ => throw new InvalidOperationException(),
            };

            var sepChar = QueryParameterListExtensions.ParameterSegmentationChar;
            var expectedParameterName = $"{expectedParameterNamePrefix}{sepChar}{columnName}{sepChar}0";

            Assert.Equal(expectedParameterName, parameter.ParameterName);
        }

        [Fact]
        public void AddMultipleParametersOfDifferentColumNameTest()
        {
            static QueryParameterType DecideParameterType(int index)
            {
                return index % 2 == 0 ? QueryParameterType.InsertValueParameter : QueryParameterType.WhereConditionParameter;
            }

            static string GenerateExpectedParameterName(int index, string columnName)
            {
                var parameterType = DecideParameterType(index);

                var expectedParameterNamePrefix = parameterType switch
                {
                    QueryParameterType.InsertValueParameter => QueryParameterListExtensions.InsertValueParameterPrefix,
                    QueryParameterType.WhereConditionParameter => QueryParameterListExtensions.WhereConditionParameterPrefix,
                    _ => throw new InvalidOperationException(),
                };

                var sepChar = QueryParameterListExtensions.ParameterSegmentationChar;
                return $"{expectedParameterNamePrefix}{sepChar}{columnName}{sepChar}0";
            }

            var queryParameters = new List<QueryParameter>();

            var raceProperties = typeof(Race).GetProperties();
            for (int i = 0; i < raceProperties.Length; i++)
            {
                var currentPropertyInfo = raceProperties[i];
                var parameterValue = new { Index = i };

                var actualParameter = queryParameters.AddQueryParameter(
                    currentPropertyInfo.Name,
                    parameterValue,
                    DecideParameterType(i));

                Assert.Contains(actualParameter, queryParameters);
                Assert.Equal(
                    GenerateExpectedParameterName(i, currentPropertyInfo.Name),
                    actualParameter.ParameterName);
                Assert.Equal(parameterValue, actualParameter.Value);
            }
        }

        [Fact]
        public void AddMultipleParametersOfSameColumnNameTest()
        {
            static QueryParameterType DecideParameterType(int index)
            {
                return index % 2 == 0 ? QueryParameterType.InsertValueParameter : QueryParameterType.WhereConditionParameter;
            }

            static string GenerateExpectedParameterName(int index, string columnName)
            {
                var parameterType = DecideParameterType(index);

                var expectedParameterNamePrefix = parameterType switch
                {
                    QueryParameterType.InsertValueParameter => QueryParameterListExtensions.InsertValueParameterPrefix,
                    QueryParameterType.WhereConditionParameter => QueryParameterListExtensions.WhereConditionParameterPrefix,
                    _ => throw new InvalidOperationException(),
                };

                var sepChar = QueryParameterListExtensions.ParameterSegmentationChar;
                return $"{expectedParameterNamePrefix}{sepChar}{columnName}{sepChar}{index / 2}";
            }

            string columnName = "Id";
            var queryParameters = new List<QueryParameter>();

            for (int i = 0; i < 1000; i++)
            {
                var parameterValue = new { Index = i };

                var actualQueryParameter = queryParameters.AddQueryParameter(
                    columnName,
                    parameterValue,
                    DecideParameterType(i));

                Assert.Contains(actualQueryParameter, queryParameters);
                Assert.Equal(
                    GenerateExpectedParameterName(i, columnName),
                    actualQueryParameter.ParameterName);
                Assert.Equal(parameterValue, actualQueryParameter.Value);
            }
        }
    }
}
