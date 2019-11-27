using Hurace.Core.Db.Extensions;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Hurace.Core.Tests.DbQueriesTests
{
    public class QueryConditionTests
    {
        [Fact]
        public void QueryConditionWithInvalidValueTest()
        {
            var condition = new QueryCondition()
            {
                ColumnToCheck = "Id",
                CompareValue = null,
                ConditionType = QueryCondition.Type.Equals
            };

            Assert.Throws<ArgumentNullException>(() => condition.AppendTo(new StringBuilder(), new List<QueryParameter>()));
        }

        [Theory]
        [InlineData("Id", QueryCondition.Type.Equals, 15, "[Id] = @{0}")]
        [InlineData("Id", QueryCondition.Type.NotEquals, 15, "[Id] != @{0}")]
        [InlineData("Id", QueryCondition.Type.GreaterThan, 15, "[Id] > @{0}")]
        [InlineData("Id", QueryCondition.Type.GreaterThanOrEquals, 15, "[Id] >= @{0}")]
        [InlineData("Id", QueryCondition.Type.Like, 15, "[Id] LIKE @{0}")]
        [InlineData("Id", QueryCondition.Type.LessThan, 15, "[Id] < @{0}")]
        [InlineData("Id", QueryCondition.Type.LessThanOrEquals, 15, "[Id] <= @{0}")]
        [InlineData("Name", QueryCondition.Type.Equals, "Marcel", "[Name] = @{0}")]
        [InlineData("Name", QueryCondition.Type.NotEquals, "Marcel", "[Name] != @{0}")]
        [InlineData("Name", QueryCondition.Type.GreaterThan, "Marcel", "[Name] > @{0}")]
        [InlineData("Name", QueryCondition.Type.GreaterThanOrEquals, "Marcel", "[Name] >= @{0}")]
        [InlineData("Name", QueryCondition.Type.Like, "Marcel", "[Name] LIKE @{0}")]
        [InlineData("Name", QueryCondition.Type.LessThan, "Marcel", "[Name] < @{0}")]
        [InlineData("Name", QueryCondition.Type.LessThanOrEquals, "Marcel", "[Name] <= @{0}")]
        public void QueryConditionWithDifferentTypesTests(
            string columnToCheck,
            QueryCondition.Type conditionType,
            object expectedValue,
            string expectedConditionStringFormat)
        {
            var condition = new QueryCondition()
            {
                ColumnToCheck = columnToCheck,
                ConditionType = conditionType,
                CompareValue = expectedValue
            };

            var actualConditionStringBuilder = new StringBuilder();
            var actualQueryParameters = new List<QueryParameter>();

            condition.AppendTo(actualConditionStringBuilder, actualQueryParameters);

            string expectedQueryParameterName = $"{columnToCheck}0";

            string expectedConditionString = string.Format(expectedConditionStringFormat, expectedQueryParameterName);

            Assert.Equal(expectedConditionString, actualConditionStringBuilder.ToString());
            Assert.Single(actualQueryParameters);
            var actualQueryParam = actualQueryParameters.First();
            Assert.Equal(expectedQueryParameterName, actualQueryParam.ParameterName);
            Assert.Equal(expectedValue, actualQueryParam.Value);
        }

        [Theory]
        [InlineData(QueryCondition.Type.Equals, "[DateOfBirth] = @{0}")]
        [InlineData(QueryCondition.Type.NotEquals, "[DateOfBirth] != @{0}")]
        [InlineData(QueryCondition.Type.GreaterThan, "[DateOfBirth] > @{0}")]
        [InlineData(QueryCondition.Type.GreaterThanOrEquals, "[DateOfBirth] >= @{0}")]
        [InlineData(QueryCondition.Type.LessThan, "[DateOfBirth] < @{0}")]
        [InlineData(QueryCondition.Type.LessThanOrEquals, "[DateOfBirth] <= @{0}")]
        public void QueryConditionWithDateTimeTests(
            QueryCondition.Type conditionType,
            string expectedConditionStringFormat)
        {
            var columnToCheck = "DateOfBirth";
            var expectedValue = new DateTime(2000, 1, 1);

            var condition = new QueryCondition()
            {
                ColumnToCheck = columnToCheck,
                ConditionType = conditionType,
                CompareValue = expectedValue
            };

            var actualConditionStringBuilder = new StringBuilder();
            var actualQueryParameters = new List<QueryParameter>();

            condition.AppendTo(actualConditionStringBuilder, actualQueryParameters);

            string expectedQueryParameterName = $"{columnToCheck}0";

            string expectedConditionString = string.Format(expectedConditionStringFormat, expectedQueryParameterName);

            Assert.Equal(expectedConditionString, actualConditionStringBuilder.ToString());
            Assert.Single(actualQueryParameters);
            var actualQueryParam = actualQueryParameters.First();
            Assert.Equal(expectedQueryParameterName, actualQueryParam.ParameterName);
            Assert.Equal(expectedValue.ToString("s"), actualQueryParam.Value);
        }
    }
}
