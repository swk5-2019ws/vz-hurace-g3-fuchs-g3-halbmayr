using Hurace.Core.Db.Extensions;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Hurace.Core.Db.Tests.QueryConditionTests
{
    public class QueryConditionTests
    {
        [Fact]
        public void BuildQueryConditionBuilderWithoutConditionsTest()
        {
            Assert.Throws<InvalidOperationException>(
                () => new QueryConditionBuilder()
                    .Build());
        }

        [Fact]
        public void QueryConditionWithInvalidValueTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new QueryConditionBuilder()
                    .DeclareCondition("Id", QueryConditionType.Equals, null)
                    .Build());
        }
        [Fact]
        public void QueryConditionWithColumnNameTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new QueryConditionBuilder()
                    .DeclareCondition(null, QueryConditionType.Equals, 1)
                    .Build());
        }

        [Theory]
        [InlineData("Id", QueryConditionType.Equals, 15, "[Id] = @{0}")]
        [InlineData("Id", QueryConditionType.NotEquals, 15, "[Id] != @{0}")]
        [InlineData("Id", QueryConditionType.GreaterThan, 15, "[Id] > @{0}")]
        [InlineData("Id", QueryConditionType.GreaterThanOrEquals, 15, "[Id] >= @{0}")]
        [InlineData("Id", QueryConditionType.Like, 15, "[Id] LIKE @{0}")]
        [InlineData("Id", QueryConditionType.LessThan, 15, "[Id] < @{0}")]
        [InlineData("Id", QueryConditionType.LessThanOrEquals, 15, "[Id] <= @{0}")]
        [InlineData("Name", QueryConditionType.Equals, "Marcel", "[Name] = @{0}")]
        [InlineData("Name", QueryConditionType.NotEquals, "Marcel", "[Name] != @{0}")]
        [InlineData("Name", QueryConditionType.GreaterThan, "Marcel", "[Name] > @{0}")]
        [InlineData("Name", QueryConditionType.GreaterThanOrEquals, "Marcel", "[Name] >= @{0}")]
        [InlineData("Name", QueryConditionType.Like, "Marcel", "[Name] LIKE @{0}")]
        [InlineData("Name", QueryConditionType.LessThan, "Marcel", "[Name] < @{0}")]
        [InlineData("Name", QueryConditionType.LessThanOrEquals, "Marcel", "[Name] <= @{0}")]
        public void QueryConditionWithDifferentTypesTests(
            string columnToCheck,
            QueryConditionType conditionType,
            object expectedValue,
            string expectedConditionStringFormat)
        {
            var condition = new QueryConditionBuilder()
                .DeclareCondition(columnToCheck, conditionType, expectedValue)
                .Build();

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
        [InlineData(QueryConditionType.Equals, "[DateOfBirth] = @{0}")]
        [InlineData(QueryConditionType.NotEquals, "[DateOfBirth] != @{0}")]
        [InlineData(QueryConditionType.GreaterThan, "[DateOfBirth] > @{0}")]
        [InlineData(QueryConditionType.GreaterThanOrEquals, "[DateOfBirth] >= @{0}")]
        [InlineData(QueryConditionType.LessThan, "[DateOfBirth] < @{0}")]
        [InlineData(QueryConditionType.LessThanOrEquals, "[DateOfBirth] <= @{0}")]
        public void QueryConditionWithDateTimeTests(
            QueryConditionType conditionType,
            string expectedConditionStringFormat)
        {
            var columnToCheck = "DateOfBirth";
            var expectedValue = new DateTime(2000, 1, 1);

            var condition = new QueryConditionBuilder()
                .DeclareCondition(columnToCheck, conditionType, expectedValue)
                .Build();

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
