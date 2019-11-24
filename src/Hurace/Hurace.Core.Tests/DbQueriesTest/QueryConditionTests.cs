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
        [InlineData("Name", QueryCondition.Type.LowerThan, "Marcel", "[Name] < 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.LowerThan, 15, "[Id] < 15")]
        [InlineData("Name", QueryCondition.Type.LowerThanOrEquals, "Marcel", "[Name] <= 'Marcel'")]
        [InlineData("Id", QueryCondition.Type.LowerThanOrEquals, 15, "[Id] <= 15")]
        public void QueryConditionTests(
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
        [InlineData(QueryConditionCombination.Type.AND, "({0} AND {1})")]
        [InlineData(QueryConditionCombination.Type.OR, "({0} OR {1})")]
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
        [InlineData(QueryConditionCombination.Type.AND, QueryConditionCombination.Type.AND, true, "(({0} AND {1}) AND {2})")]
        [InlineData(QueryConditionCombination.Type.AND, QueryConditionCombination.Type.AND, false, "({0} AND ({1} AND {2}))")]
        [InlineData(QueryConditionCombination.Type.AND, QueryConditionCombination.Type.OR, true, "(({0} OR {1}) AND {2})")]
        [InlineData(QueryConditionCombination.Type.AND, QueryConditionCombination.Type.OR, false, "({0} AND ({1} OR {2}))")]
        [InlineData(QueryConditionCombination.Type.OR, QueryConditionCombination.Type.AND, true, "(({0} AND {1}) OR {2})")]
        [InlineData(QueryConditionCombination.Type.OR, QueryConditionCombination.Type.AND, false, "({0} OR ({1} AND {2}))")]
        [InlineData(QueryConditionCombination.Type.OR, QueryConditionCombination.Type.OR, true, "(({0} OR {1}) OR {2})")]
        [InlineData(QueryConditionCombination.Type.OR, QueryConditionCombination.Type.OR, false, "({0} OR ({1} OR {2}))")]
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
        [InlineData(QueryConditionCombination.Type.AND, QueryConditionCombination.Type.AND,
                    QueryConditionCombination.Type.AND, "(({0} AND {1}) AND ({2} AND {3}))")]
        [InlineData(QueryConditionCombination.Type.AND, QueryConditionCombination.Type.AND,
                    QueryConditionCombination.Type.OR, "(({0} AND {1}) AND ({2} OR {3}))")]
        [InlineData(QueryConditionCombination.Type.AND, QueryConditionCombination.Type.OR,
                    QueryConditionCombination.Type.AND, "(({0} OR {1}) AND ({2} AND {3}))")]
        [InlineData(QueryConditionCombination.Type.AND, QueryConditionCombination.Type.OR,
                    QueryConditionCombination.Type.OR, "(({0} OR {1}) AND ({2} OR {3}))")]
        [InlineData(QueryConditionCombination.Type.OR, QueryConditionCombination.Type.AND,
                    QueryConditionCombination.Type.AND, "(({0} AND {1}) OR ({2} AND {3}))")]
        [InlineData(QueryConditionCombination.Type.OR, QueryConditionCombination.Type.AND,
                    QueryConditionCombination.Type.OR, "(({0} AND {1}) OR ({2} OR {3}))")]
        [InlineData(QueryConditionCombination.Type.OR, QueryConditionCombination.Type.OR,
                    QueryConditionCombination.Type.AND, "(({0} OR {1}) OR ({2} AND {3}))")]
        [InlineData(QueryConditionCombination.Type.OR, QueryConditionCombination.Type.OR,
                    QueryConditionCombination.Type.OR, "(({0} OR {1}) OR ({2} OR {3}))")]
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
