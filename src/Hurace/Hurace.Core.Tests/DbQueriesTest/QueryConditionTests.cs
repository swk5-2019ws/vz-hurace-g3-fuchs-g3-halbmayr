using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

#pragma warning disable IDE0045 // Convert to conditional expression
namespace Hurace.Core.Tests.DbQueriesTest
{
    public class IQueryConditionTests
    {
        [Fact]
        public void QueryConditionBuildWithInvalidStringBuilder()
        {
            var condition = new QueryCondition()
            {
                ColumnToCheck = "Id",
                CompareValue = 1,
                ConditionType = QueryCondition.Type.Equals
            };

            Assert.Throws<ArgumentNullException>(() => condition.Build(null));
        }

        [Fact]
        public void QueryConditionCombinationBuildWithInvalidStringBuilder()
        {
            var condition = new QueryConditionCombination()
            {
                CombinationType = QueryConditionCombination.Type.And,
                FirstCondition = new QueryCondition()
                {
                    ColumnToCheck = "FirstName",
                    CompareValue = "Marcel",
                    ConditionType = QueryCondition.Type.Equals
                },
                SecondCondition = new QueryCondition()
                {
                    ColumnToCheck = "LastName",
                    CompareValue = "Hirscher",
                    ConditionType = QueryCondition.Type.Equals
                }
            };

            Assert.Throws<ArgumentNullException>(() => condition.Build(null));
        }

        [Fact]
        public void QueryConditionCombinationWithInvalidFirstCondition()
        {
            var condition = new QueryConditionCombination()
            {
                CombinationType = QueryConditionCombination.Type.And,
                FirstCondition = null,
                SecondCondition = new QueryCondition()
                {
                    ColumnToCheck = "LastName",
                    CompareValue = "Hirscher",
                    ConditionType = QueryCondition.Type.Equals
                }
            };

            Assert.Throws<InvalidOperationException>(() => condition.Build(new StringBuilder()));
        }

        [Fact]
        public void QueryConditionCombinationWithInvalidSecondCondition()
        {
            var condition = new QueryConditionCombination()
            {
                CombinationType = QueryConditionCombination.Type.And,
                FirstCondition = new QueryCondition()
                {
                    ColumnToCheck = "FirstName",
                    CompareValue = "Marcel",
                    ConditionType = QueryCondition.Type.Equals
                },
                SecondCondition = null
            };

            Assert.Throws<InvalidOperationException>(() => condition.Build(new StringBuilder()));
        }

        [Theory]
        [InlineData("Name", QueryCondition.Type.Equals, "Marcel", "[Name] = 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.Equals, 15, "[Id] = 15")]
        [InlineData("Name", QueryCondition.Type.NotEquals, "Marcel", "[Name] != 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.NotEquals, 15, "[Id] != 15")]
        [InlineData("Name", QueryCondition.Type.GreaterThan, "Marcel", "[Name] > 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.GreaterThan, 15, "[Id] > 15")]
        [InlineData("Name", QueryCondition.Type.GreaterThanOrEquals, "Marcel", "[Name] >= 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.GreaterThanOrEquals, 15, "[Id] >= 15")]
        [InlineData("Name", QueryCondition.Type.Like, "Marcel", "[Name] LIKE 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.Like, 15, "[Id] LIKE 15")]
        [InlineData("Name", QueryCondition.Type.LessThan, "Marcel", "[Name] < 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.LessThan, 15, "[Id] < 15")]
        [InlineData("Name", QueryCondition.Type.LessThanOrEquals, "Marcel", "[Name] <= 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.LessThanOrEquals, 15, "[Id] <= 15")]
        public void QueryConditionWithValueTypesTests(
            string columnToCheck,
            QueryCondition.Type conditionType,
            object compareValue,
            string expectedConditionString)
        {
            var condition = new QueryCondition()
            {
                ColumnToCheck = columnToCheck,
                ConditionType = conditionType,
                CompareValue = compareValue
            };

            var actualQueryBuilder = new StringBuilder();
            condition.Build(actualQueryBuilder);

            Assert.Equal(expectedConditionString, actualQueryBuilder.ToString());
        }

        [Theory]
        [InlineData(QueryCondition.Type.Equals, "[DateOfBirth] = '2000-01-01T00:00:00'")]
        [InlineData(QueryCondition.Type.NotEquals, "[DateOfBirth] != '2000-01-01T00:00:00'")]
        [InlineData(QueryCondition.Type.GreaterThan, "[DateOfBirth] > '2000-01-01T00:00:00'")]
        [InlineData(QueryCondition.Type.GreaterThanOrEquals, "[DateOfBirth] >= '2000-01-01T00:00:00'")]
        [InlineData(QueryCondition.Type.LessThan, "[DateOfBirth] < '2000-01-01T00:00:00'")]
        [InlineData(QueryCondition.Type.LessThanOrEquals, "[DateOfBirth] <= '2000-01-01T00:00:00'")]
        public void QueryConditionWithDateTimeTests(QueryCondition.Type conditionType, string expectedConditionString)
        {
            var condition = new QueryCondition()
            {
                ColumnToCheck = "DateOfBirth",
                CompareValue = new DateTime(2000, 1, 1),
                ConditionType = conditionType
            };

            var actualQueryBuilder = new StringBuilder();
            condition.Build(actualQueryBuilder);

            Assert.Equal(expectedConditionString, actualQueryBuilder.ToString());
        }

        [Theory]
        [InlineData(QueryConditionCombination.Type.And, "({0} AND {1})")]
        [InlineData(QueryConditionCombination.Type.Or, "({0} OR {1})")]
        public void QueryConditionCombinationTests(
            QueryConditionCombination.Type combinationType,
            string expectedConditionStringFormat)
        {
            var firstCondition = new QueryCondition()
            {
                ColumnToCheck = "Id",
                CompareValue = 15,
                ConditionType = QueryCondition.Type.Equals
            };
            var secondCondition = new QueryCondition()
            {
                ColumnToCheck = "Name",
                CompareValue = "Marcel",
                ConditionType = QueryCondition.Type.Like
            };

            var condition = new QueryConditionCombination()
            {
                CombinationType = combinationType,
                FirstCondition = firstCondition,
                SecondCondition = secondCondition
            };

            var expectedFirstConditionStringBuilder = new StringBuilder();
            var expectedSecondConditionStringBuilder = new StringBuilder();

            firstCondition.Build(expectedFirstConditionStringBuilder);
            secondCondition.Build(expectedSecondConditionStringBuilder);

            var expectedConditionString = string.Format(
                expectedConditionStringFormat,
                expectedFirstConditionStringBuilder.ToString(),
                expectedSecondConditionStringBuilder.ToString());

            var actualQueryBuilder = new StringBuilder();
            condition.Build(actualQueryBuilder);

            Assert.Equal(expectedConditionString, actualQueryBuilder.ToString());
        }

        [Theory]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, true, "(({0} AND {1}) AND {2})")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, false, "({0} AND ({1} AND {2}))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, true, "(({0} OR {1}) AND {2})")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, false, "({0} AND ({1} OR {2}))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, true, "(({0} AND {1}) OR {2})")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, false, "({0} OR ({1} AND {2}))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, true, "(({0} OR {1}) OR {2})")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, false, "({0} OR ({1} OR {2}))")]
        public void UnbalancedInterlacedQueryConditionCombinationTests(
            QueryConditionCombination.Type combinationTypeOnFirstLayer,
            QueryConditionCombination.Type combinationTypeForInterlacedCondition,
            bool interlacedCombinationOnLeft,
            string expectedConditionStringFormat)
        {
            var simpleCondition = new QueryCondition()
            {
                ColumnToCheck = "Id",
                CompareValue = 15,
                ConditionType = QueryCondition.Type.NotEquals
            };

            var firstInterlacedCondition = new QueryCondition()
            {
                ColumnToCheck = "FirstName",
                CompareValue = "Marcel",
                ConditionType = QueryCondition.Type.Equals
            };
            var secondInterlacedCondition = new QueryCondition()
            {
                ColumnToCheck = "FirstName",
                CompareValue = "Viktoria",
                ConditionType = QueryCondition.Type.Equals
            };
            var interlacedCondition = new QueryConditionCombination()
            {
                CombinationType = combinationTypeForInterlacedCondition,
                FirstCondition = firstInterlacedCondition,
                SecondCondition = secondInterlacedCondition
            };

            var condition = new QueryConditionCombination()
            {
                CombinationType = combinationTypeOnFirstLayer,
                FirstCondition = interlacedCombinationOnLeft ? (IQueryCondition)interlacedCondition : simpleCondition,
                SecondCondition = interlacedCombinationOnLeft ? (IQueryCondition)simpleCondition : interlacedCondition
            };

            var expectedSimpleConditionStringBuilder = new StringBuilder();
            var expectedFirstInterlacedConditionStringBuilder = new StringBuilder();
            var expectedSecondInterlacedConditionStringBuilder = new StringBuilder();

            simpleCondition.Build(expectedSimpleConditionStringBuilder);
            firstInterlacedCondition.Build(expectedFirstInterlacedConditionStringBuilder);
            secondInterlacedCondition.Build(expectedSecondInterlacedConditionStringBuilder);

            string expectedConditionString;

            if (interlacedCombinationOnLeft)
                expectedConditionString = string.Format(
                    expectedConditionStringFormat,
                    expectedFirstInterlacedConditionStringBuilder.ToString(),
                    expectedSecondInterlacedConditionStringBuilder.ToString(),
                    expectedSimpleConditionStringBuilder.ToString());
            else
                expectedConditionString = string.Format(
                    expectedConditionStringFormat,
                    expectedSimpleConditionStringBuilder.ToString(),
                    expectedFirstInterlacedConditionStringBuilder.ToString(),
                    expectedSecondInterlacedConditionStringBuilder.ToString());

            var actualConditionStringBuilder = new StringBuilder();
            condition.Build(actualConditionStringBuilder);

            Assert.Equal(expectedConditionString, actualConditionStringBuilder.ToString());
        }

        [Theory]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And,
                    QueryConditionCombination.Type.And, "(({0} AND {1}) AND ({2} AND {3}))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And,
                    QueryConditionCombination.Type.Or, "(({0} AND {1}) AND ({2} OR {3}))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or,
                    QueryConditionCombination.Type.And, "(({0} OR {1}) AND ({2} AND {3}))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or,
                    QueryConditionCombination.Type.Or, "(({0} OR {1}) AND ({2} OR {3}))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And,
                    QueryConditionCombination.Type.And, "(({0} AND {1}) OR ({2} AND {3}))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And,
                    QueryConditionCombination.Type.Or, "(({0} AND {1}) OR ({2} OR {3}))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or,
                    QueryConditionCombination.Type.And, "(({0} OR {1}) OR ({2} AND {3}))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or,
                    QueryConditionCombination.Type.Or, "(({0} OR {1}) OR ({2} OR {3}))")]
        public void BalancedInterlacedConditionCombinationTests(
            QueryConditionCombination.Type combinationTypeOnFirstLayer,
            QueryConditionCombination.Type combinationInFirstInterlacedCondition,
            QueryConditionCombination.Type combinationInSecondInterlacedCondition,
            string expectedConditionStringFormat)
        {
            var firstConditionOfFirstInterlacedCondition = new QueryCondition()
            {
                ColumnToCheck = "FirstName",
                CompareValue = "Marcel",
                ConditionType = QueryCondition.Type.Equals
            };
            var secondConditionOfFirstInterlacedCondition = new QueryCondition()
            {
                ColumnToCheck = "FirstName",
                CompareValue = "Viktoria",
                ConditionType = QueryCondition.Type.Equals
            };
            var firstInterlacedCondition = new QueryConditionCombination()
            {
                CombinationType = combinationInFirstInterlacedCondition,
                FirstCondition = firstConditionOfFirstInterlacedCondition,
                SecondCondition = secondConditionOfFirstInterlacedCondition
            };

            var firstConditionOfSecondInterlacedCondition = new QueryCondition()
            {
                ColumnToCheck = "LastName",
                CompareValue = "Hirscher",
                ConditionType = QueryCondition.Type.Equals
            };
            var secondConditionOfSecondInterlacedCondition = new QueryCondition()
            {
                ColumnToCheck = "LastName",
                CompareValue = "Mathis",
                ConditionType = QueryCondition.Type.NotEquals
            };
            var secondInterlacedCondition = new QueryConditionCombination()
            {
                CombinationType = combinationInSecondInterlacedCondition,
                FirstCondition = firstConditionOfSecondInterlacedCondition,
                SecondCondition = secondConditionOfSecondInterlacedCondition
            };

            var condition = new QueryConditionCombination()
            {
                CombinationType = combinationTypeOnFirstLayer,
                FirstCondition = firstInterlacedCondition,
                SecondCondition = secondInterlacedCondition
            };

            var expectedConditionStringBuilderOfFF = new StringBuilder();
            var expectedConditionStringBuilderOfSF = new StringBuilder();
            var expectedConditionStringBuilderOfFS = new StringBuilder();
            var expectedConditionStringBuilderOfSS = new StringBuilder();

            firstConditionOfFirstInterlacedCondition.Build(expectedConditionStringBuilderOfFF);
            secondConditionOfFirstInterlacedCondition.Build(expectedConditionStringBuilderOfSF);
            firstConditionOfSecondInterlacedCondition.Build(expectedConditionStringBuilderOfFS);
            secondConditionOfSecondInterlacedCondition.Build(expectedConditionStringBuilderOfSS);

            var expectedConditionString =
                string.Format(
                    expectedConditionStringFormat,
                    expectedConditionStringBuilderOfFF,
                    expectedConditionStringBuilderOfSF,
                    expectedConditionStringBuilderOfFS,
                    expectedConditionStringBuilderOfSS);

            var actualConditionStringBuilder = new StringBuilder();
            condition.Build(actualConditionStringBuilder);

            Assert.Equal(expectedConditionString, actualConditionStringBuilder.ToString());
        }
    }
}
