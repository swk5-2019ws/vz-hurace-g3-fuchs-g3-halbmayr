using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Assert.Throws<InvalidOperationException>(() => condition.AppendTo(new StringBuilder(), new List<QueryParameter>()));
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

            Assert.Throws<InvalidOperationException>(() => condition.AppendTo(new StringBuilder(), new List<QueryParameter>()));
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

            var expectedFirstConditionStringBuilder = new StringBuilder();
            var expectedSecondConditionStringBuilder = new StringBuilder();
            var expectedQueryParameters = new List<QueryParameter>();

            firstCondition.AppendTo(expectedFirstConditionStringBuilder, expectedQueryParameters);
            secondCondition.AppendTo(expectedSecondConditionStringBuilder, expectedQueryParameters);

            var expectedConditionString = string.Format(
                expectedConditionStringFormat,
                expectedFirstConditionStringBuilder.ToString(),
                expectedSecondConditionStringBuilder.ToString());

            var actualConditionStringBuilder = new StringBuilder();
            var actualQueryParameters = new List<QueryParameter>();

            condition.AppendTo(actualConditionStringBuilder, actualQueryParameters);

            Assert.Equal(expectedConditionString, actualConditionStringBuilder.ToString());
            Assert.Equal(expectedQueryParameters.Count, actualQueryParameters.Count);

            var idParameter = actualQueryParameters.First(qp => qp.ParameterName == "Id0");
            var nameParameter = actualQueryParameters.First(qp => qp.ParameterName == "Name0");

            Assert.Equal(15, idParameter.Value);
            Assert.Equal("Marcel", nameParameter.Value);
        }

        [Theory]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, true,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) AND [Id] != @Id0)")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, false,
                    "([Id] != @Id0 AND ([FirstName] = @FirstName0 AND [FirstName] = @FirstName1))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, true,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) AND [Id] != @Id0)")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, false,
                    "([Id] != @Id0 AND ([FirstName] = @FirstName0 OR [FirstName] = @FirstName1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, true,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) OR [Id] != @Id0)")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, false,
                    "([Id] != @Id0 OR ([FirstName] = @FirstName0 AND [FirstName] = @FirstName1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, true,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) OR [Id] != @Id0)")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, false,
                    "([Id] != @Id0 OR ([FirstName] = @FirstName0 OR [FirstName] = @FirstName1))")]
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
                FirstCondition = interlacedCombinationOnLeft ? (IQueryCondition)interlacedCondition : simpleCondition,
                SecondCondition = interlacedCombinationOnLeft ? (IQueryCondition)simpleCondition : interlacedCondition
            };

            var actualConditionStringBuilder = new StringBuilder();
            var actualQueryParameters = new List<QueryParameter>();

            condition.AppendTo(actualConditionStringBuilder, actualQueryParameters);

            Assert.Equal(expectedConditionString, actualConditionStringBuilder.ToString());
            Assert.Equal(3, actualQueryParameters.Count);

            var idParameter = actualQueryParameters.First(qp => qp.ParameterName == "Id0");
            var firstNameParameter1 = actualQueryParameters.First(qp => qp.ParameterName == "FirstName0");
            var firstNameParameter2 = actualQueryParameters.First(qp => qp.ParameterName == "FirstName1");

            Assert.Equal(15, idParameter.Value);
            Assert.Equal("Marcel", firstNameParameter1.Value);
            Assert.Equal("Viktoria", firstNameParameter2.Value);
        }

        [Theory]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, QueryConditionCombination.Type.And,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) " +
                    "AND ([LastName] = @LastName0 AND [LastName] != @LastName1))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) " +
                    "AND ([LastName] = @LastName0 OR [LastName] != @LastName1))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) " +
                    "AND ([LastName] = @LastName0 AND [LastName] != @LastName1))")]
        [InlineData(QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) " +
                    "AND ([LastName] = @LastName0 OR [LastName] != @LastName1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, QueryConditionCombination.Type.And,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) " +
                    "OR ([LastName] = @LastName0 AND [LastName] != @LastName1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And, QueryConditionCombination.Type.Or,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) " +
                    "OR ([LastName] = @LastName0 OR [LastName] != @LastName1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, QueryConditionCombination.Type.And,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) " +
                    "OR ([LastName] = @LastName0 AND [LastName] != @LastName1))")]
        [InlineData(QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or, QueryConditionCombination.Type.Or,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) " +
                    "OR ([LastName] = @LastName0 OR [LastName] != @LastName1))")]
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

            var actualConditionStringBuilder = new StringBuilder();
            var actualQueryParameters = new List<QueryParameter>();

            condition.AppendTo(actualConditionStringBuilder, actualQueryParameters);

            Assert.Equal(expectedConditionString, actualConditionStringBuilder.ToString());
            Assert.Equal(4, actualQueryParameters.Count);

            var firstNameParameter1 = actualQueryParameters.First(qp => qp.ParameterName == "FirstName0");
            var firstNameParameter2 = actualQueryParameters.First(qp => qp.ParameterName == "FirstName1");
            var lastNameParameter1 = actualQueryParameters.First(qp => qp.ParameterName == "LastName0");
            var lastNameParameter2 = actualQueryParameters.First(qp => qp.ParameterName == "LastName1");

            Assert.Equal("Marcel", firstNameParameter1.Value);
            Assert.Equal("Viktoria", firstNameParameter2.Value);
            Assert.Equal("Hirscher", lastNameParameter1.Value);
            Assert.Equal("Mathis", lastNameParameter2.Value);
        }
    }
}
