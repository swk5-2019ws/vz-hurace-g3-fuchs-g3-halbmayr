using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

#pragma warning disable IDE0045 // Convert to conditional expression
namespace Hurace.Core.Tests.DbQueriesTests
{
    public class QueryConditionCombinationTests
    {
        [Fact]
        public void QueryConditionCombinationWithInvalidFirstConditionTest()
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

            Assert.Throws<InvalidOperationException>(() => (_, _) = condition.Build());
        }

        [Fact]
        public void QueryConditionCombinationWithInvalidSecondConditionTest()
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

            Assert.Throws<InvalidOperationException>(() => (_, _) = condition.Build());
        }

        [Theory]
        [InlineData(QueryConditionCombination.Type.And, "({0} AND {1})")]
        [InlineData(QueryConditionCombination.Type.Or, "({0} OR {1})")]
        public void BasicQueryConditionCombinationTests(
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

            (var exptectedFirstConditionString, _) = firstCondition.Build();
            (var expectedSecondConditionString, _) = secondCondition.Build();

            var expectedConditionString = string.Format(
                expectedConditionStringFormat,
                exptectedFirstConditionString,
                expectedSecondConditionString);

            (var actualConditionString, _) = condition.Build();

            Assert.Equal(expectedConditionString, actualConditionString);
        }

        [Theory]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, true,
                    "(([FirstName] = @WC_FirstName_0 AND [FirstName] = @WC_FirstName_1) AND [Id] != @WC_Id_0)")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, false,
                    "([Id] != @WC_Id_0 AND ([FirstName] = @WC_FirstName_0 AND [FirstName] = @WC_FirstName_1))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, true,
                    "(([FirstName] = @WC_FirstName_0 OR [FirstName] = @WC_FirstName_1) AND [Id] != @WC_Id_0)")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, false,
                    "([Id] != @WC_Id_0 AND ([FirstName] = @WC_FirstName_0 OR [FirstName] = @WC_FirstName_1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, true,
                    "(([FirstName] = @WC_FirstName_0 AND [FirstName] = @WC_FirstName_1) OR [Id] != @WC_Id_0)")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, false,
                    "([Id] != @WC_Id_0 OR ([FirstName] = @WC_FirstName_0 AND [FirstName] = @WC_FirstName_1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, true,
                    "(([FirstName] = @WC_FirstName_0 OR [FirstName] = @WC_FirstName_1) OR [Id] != @WC_Id_0)")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, false,
                    "([Id] != @WC_Id_0 OR ([FirstName] = @WC_FirstName_0 OR [FirstName] = @WC_FirstName_1))")]
        public void UnbalancedInterlacedQueryConditionCombinationTests(
            QueryConditionCombination.Type combinationTypeOnFirstLayer,
            QueryConditionCombination.Type combinationTypeForInterlacedCondition,
            bool interlacedCombinationOnLeft,
            string expectedConditionString)
        {
            var simpleCondition = new QueryCondition()
            {
                ColumnToCheck = "Id",
                CompareValue = 15,
                ConditionType = QueryCondition.Type.NotEquals
            };

            var interlacedCondition = new QueryConditionCombination()
            {
                CombinationType = combinationTypeForInterlacedCondition,
                FirstCondition = new QueryCondition()
                {
                    ColumnToCheck = "FirstName",
                    CompareValue = "Marcel",
                    ConditionType = QueryCondition.Type.Equals
                },
                SecondCondition = new QueryCondition()
                {
                    ColumnToCheck = "FirstName",
                    CompareValue = "Viktoria",
                    ConditionType = QueryCondition.Type.Equals
                }
            };

            var condition = new QueryConditionCombination()
            {
                CombinationType = combinationTypeOnFirstLayer,
                FirstCondition = interlacedCombinationOnLeft ? (QueryConditionBase)interlacedCondition : simpleCondition,
                SecondCondition = interlacedCombinationOnLeft ? (QueryConditionBase)simpleCondition : interlacedCondition
            };

            (var actualConditionString, _) = condition.Build();

            Assert.Equal(expectedConditionString, actualConditionString);
        }

        [Theory]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, QueryConditionCombination.Type.And,
                    "(([FirstName] = @WC_FirstName_0 AND [FirstName] = @WC_FirstName_1) " +
                    "AND ([LastName] = @WC_LastName_0 AND [LastName] != @WC_LastName_1))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or,
                    "(([FirstName] = @WC_FirstName_0 AND [FirstName] = @WC_FirstName_1) " +
                    "AND ([LastName] = @WC_LastName_0 OR [LastName] != @WC_LastName_1))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And,
                    "(([FirstName] = @WC_FirstName_0 OR [FirstName] = @WC_FirstName_1) " +
                    "AND ([LastName] = @WC_LastName_0 AND [LastName] != @WC_LastName_1))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or,
                    "(([FirstName] = @WC_FirstName_0 OR [FirstName] = @WC_FirstName_1) " +
                    "AND ([LastName] = @WC_LastName_0 OR [LastName] != @WC_LastName_1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, QueryConditionCombination.Type.And,
                    "(([FirstName] = @WC_FirstName_0 AND [FirstName] = @WC_FirstName_1) " +
                    "OR ([LastName] = @WC_LastName_0 AND [LastName] != @WC_LastName_1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or,
                    "(([FirstName] = @WC_FirstName_0 AND [FirstName] = @WC_FirstName_1) " +
                    "OR ([LastName] = @WC_LastName_0 OR [LastName] != @WC_LastName_1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And,
                    "(([FirstName] = @WC_FirstName_0 OR [FirstName] = @WC_FirstName_1) " +
                    "OR ([LastName] = @WC_LastName_0 AND [LastName] != @WC_LastName_1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or,
                    "(([FirstName] = @WC_FirstName_0 OR [FirstName] = @WC_FirstName_1) " +
                    "OR ([LastName] = @WC_LastName_0 OR [LastName] != @WC_LastName_1))")]
        public void BalancedInterlacedConditionCombinationTests(
            QueryConditionCombination.Type combinationTypeOnFirstLayer,
            QueryConditionCombination.Type combinationInFirstInterlacedCondition,
            QueryConditionCombination.Type combinationInSecondInterlacedCondition,
            string expectedConditionString)
        {
            var condition = new QueryConditionCombination()
            {
                CombinationType = combinationTypeOnFirstLayer,
                FirstCondition = new QueryConditionCombination()
                {
                    CombinationType = combinationInFirstInterlacedCondition,
                    FirstCondition = new QueryCondition()
                    {
                        ColumnToCheck = "FirstName",
                        CompareValue = "Marcel",
                        ConditionType = QueryCondition.Type.Equals
                    },
                    SecondCondition = new QueryCondition()
                    {
                        ColumnToCheck = "FirstName",
                        CompareValue = "Viktoria",
                        ConditionType = QueryCondition.Type.Equals
                    }
                },
                SecondCondition = new QueryConditionCombination()
                {
                    CombinationType = combinationInSecondInterlacedCondition,
                    FirstCondition = new QueryCondition()
                    {
                        ColumnToCheck = "LastName",
                        CompareValue = "Hirscher",
                        ConditionType = QueryCondition.Type.Equals
                    },
                    SecondCondition = new QueryCondition()
                    {
                        ColumnToCheck = "LastName",
                        CompareValue = "Mathis",
                        ConditionType = QueryCondition.Type.NotEquals
                    }
                }
            };

            (var actualConditionString, _) = condition.Build();

            Assert.Equal(expectedConditionString, actualConditionString);
        }
    }
}
