using Hurace.Core.Db.Extensions;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Hurace.Core.Tests.ExtensionTests
{
    public class IListOfQueryParameterExtensionsTests
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

            queryParameterList.AddQueryParameter(columnName, expectedColumnValue, parameterType);

            Assert.Single(queryParameterList);

            var parameter = queryParameterList.First();

            Assert.Equal(expectedColumnValue, parameter.Value);

            var expectedParameterName = parameterType switch
            {
                QueryParameterType.InsertValueParameter => IListOfQueryParameterExtensions.InsertValueParameterPrefix,
                QueryParameterType.WhereConditionParameter => IListOfQueryParameterExtensions.WhereConditionParameterPrefix,
                _ => throw new InvalidOperationException(),
            };

            var sepChar = IListOfQueryParameterExtensions.ParameterSegmentationChar;
            expectedParameterName += $"{sepChar}{columnName}{sepChar}0";

            Assert.Equal(expectedParameterName, parameter.ParameterName);
        }

        [Theory]
        [InlineData(QueryParameterType.InsertValueParameter)]
        [InlineData(QueryParameterType.WhereConditionParameter)]
        public void AddMultipleParametersOfSameTypeTest(QueryParameterType parameterType)
        {
            Assert.False(true);
        }

        [Fact]
        public void AddMultipleParametersOfDifferentTypesTest()
        {
            Assert.False(true);
        }

        [Fact]
        public void AddMultipleParametersOfSameColumnNameTest()
        {
            Assert.False(true);
        }
    }
}
