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
        [Fact]
        public void AddQueryParameterOnNullListTest()
        {
            IList<QueryParameter> unsetQueryParameterList = null;
            Assert.Throws<ArgumentNullException>(
                () => unsetQueryParameterList.AddQueryParameter("", new object()));
        }

        [Fact]
        public void AddQueryParameterWithNullValueTest()
        {
            var emptyQueryParameterList = new List<QueryParameter>();
            Assert.Throws<ArgumentNullException>(
                () => emptyQueryParameterList.AddQueryParameter("", null));
        }

        [Theory]
        [InlineData(15)]
        [InlineData("Marcel")]
        public void AddSingleParameterTest(object expectedColumnValue)
        {
            string columnName = "Testcolumn";
            var queryParameterList = new List<QueryParameter>();

            var returnedParameter = queryParameterList.AddQueryParameter(columnName, expectedColumnValue);

            Assert.Single(queryParameterList);

            var parameter = queryParameterList.First();

            Assert.Equal(returnedParameter, parameter);
            Assert.Equal(expectedColumnValue, parameter.Value);

            var expectedParameterName = $"{columnName}0";

            Assert.Equal(expectedParameterName, parameter.ParameterName);
        }

        [Fact]
        public void AddMultipleParametersOfDifferentColumNameTest()
        {
            var queryParameters = new List<QueryParameter>();

            var raceProperties = typeof(Race).GetProperties();
            for (int i = 0; i < raceProperties.Length; i++)
            {
                var currentProperty = raceProperties[i];
                var parameterValue = new { Index = i };

                var actualParameter = queryParameters.AddQueryParameter(
                    currentProperty.Name,
                    parameterValue);

                Assert.Contains(actualParameter, queryParameters);
                Assert.Equal(
                    $"{currentProperty.Name}0",
                    actualParameter.ParameterName);
                Assert.Equal(parameterValue, actualParameter.Value);
            }
        }

        [Fact]
        public void AddMultipleParametersOfSameColumnNameTest()
        {
            string columnName = "Id";
            var queryParameters = new List<QueryParameter>();

            for (int i = 0; i < 1000; i++)
            {
                var parameterValue = new { Index = i };

                var actualQueryParameter = queryParameters.AddQueryParameter(
                    columnName,
                    parameterValue);

                Assert.Contains(actualQueryParameter, queryParameters);
                Assert.Equal(
                    $"{columnName}{i}",
                    actualQueryParameter.ParameterName);
                Assert.Equal(parameterValue, actualQueryParameter.Value);
            }
        }
    }
}
