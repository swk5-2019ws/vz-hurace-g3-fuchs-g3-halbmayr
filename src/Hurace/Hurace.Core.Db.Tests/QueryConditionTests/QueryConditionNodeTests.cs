using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

#pragma warning disable IDE0045 // Convert to conditional expression
namespace Hurace.Core.Db.Tests.QueryConditionTests
{
    public class QueryConditionNodeTests
    {
        [Fact]
        public void QueryConditionCombinationWithInvalidFirstConditionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new QueryConditionBuilder()
                    .DeclareConditionNode(
                        QueryConditionNodeType.And,
                        null,
                        () => new QueryConditionBuilder().DeclareCondition("LastName", QueryConditionType.Equals, "Hirscher")));
        }

        [Fact]
        public void QueryConditionCombinationWithInvalidSecondConditionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new QueryConditionBuilder()
                    .DeclareConditionNode(
                        QueryConditionNodeType.And,
                        () => new QueryConditionBuilder().DeclareCondition("FirstName", QueryConditionType.Equals, "Marcel"),
                        null));
        }

        [Theory]
        [InlineData(QueryConditionNodeType.And, "({0} AND {1})")]
        [InlineData(QueryConditionNodeType.Or, "({0} OR {1})")]
        public void BasicQueryConditionCombinationTests(
            QueryConditionNodeType combinationType,
            string expectedConditionStringFormat)
        {
            var firstConditionBuilder = new QueryConditionBuilder()
                .DeclareCondition("Id", QueryConditionType.Equals, 15);
            var secondConditionBuilder = new QueryConditionBuilder()
                .DeclareCondition("Name", QueryConditionType.Like, "Marcel");

            var condition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    combinationType,
                    () => firstConditionBuilder,
                    () => secondConditionBuilder)
                .Build();

            var expectedFirstConditionStringBuilder = new StringBuilder();
            var expectedSecondConditionStringBuilder = new StringBuilder();
            var expectedQueryParameters = new List<QueryParameter>();

            firstConditionBuilder.Build()
                .AppendTo(expectedFirstConditionStringBuilder, expectedQueryParameters);
            secondConditionBuilder.Build()
                .AppendTo(expectedSecondConditionStringBuilder, expectedQueryParameters);

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
        [InlineData(QueryConditionNodeType.And, QueryConditionNodeType.And, true,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) AND [Id] != @Id0)")]
        [InlineData(QueryConditionNodeType.And, QueryConditionNodeType.And, false,
                    "([Id] != @Id0 AND ([FirstName] = @FirstName0 AND [FirstName] = @FirstName1))")]
        [InlineData(QueryConditionNodeType.And, QueryConditionNodeType.Or, true,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) AND [Id] != @Id0)")]
        [InlineData(QueryConditionNodeType.And, QueryConditionNodeType.Or, false,
                    "([Id] != @Id0 AND ([FirstName] = @FirstName0 OR [FirstName] = @FirstName1))")]
        [InlineData(QueryConditionNodeType.Or, QueryConditionNodeType.And, true,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) OR [Id] != @Id0)")]
        [InlineData(QueryConditionNodeType.Or, QueryConditionNodeType.And, false,
                    "([Id] != @Id0 OR ([FirstName] = @FirstName0 AND [FirstName] = @FirstName1))")]
        [InlineData(QueryConditionNodeType.Or, QueryConditionNodeType.Or, true,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) OR [Id] != @Id0)")]
        [InlineData(QueryConditionNodeType.Or, QueryConditionNodeType.Or, false,
                    "([Id] != @Id0 OR ([FirstName] = @FirstName0 OR [FirstName] = @FirstName1))")]
        public void UnbalancedInterlacedQueryConditionCombinationTests(
            QueryConditionNodeType combinationTypeOnFirstLayer,
            QueryConditionNodeType combinationTypeForInterlacedCondition,
            bool interlacedCombinationOnLeft,
            string expectedConditionString)
        {
            var simpleConditionBuilder = new QueryConditionBuilder()
                .DeclareCondition("Id", QueryConditionType.NotEquals, 15);

            var interlacedConditionBuilder = new QueryConditionBuilder()
                .DeclareConditionNode(
                    combinationTypeForInterlacedCondition,
                    () => new QueryConditionBuilder().DeclareCondition("FirstName", QueryConditionType.Equals, "Marcel"),
                    () => new QueryConditionBuilder().DeclareCondition("FirstName", QueryConditionType.Equals, "Viktoria"));

            var condition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    combinationTypeOnFirstLayer,
                    () => interlacedCombinationOnLeft ? interlacedConditionBuilder : simpleConditionBuilder,
                    () => interlacedCombinationOnLeft ? simpleConditionBuilder : interlacedConditionBuilder)
                .Build();

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
        [InlineData(QueryConditionNodeType.And, QueryConditionNodeType.And, QueryConditionNodeType.And,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) " +
                    "AND ([LastName] = @LastName0 AND [LastName] != @LastName1))")]
        [InlineData(QueryConditionNodeType.And, QueryConditionNodeType.And, QueryConditionNodeType.Or,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) " +
                    "AND ([LastName] = @LastName0 OR [LastName] != @LastName1))")]
        [InlineData(QueryConditionNodeType.And, QueryConditionNodeType.Or, QueryConditionNodeType.And,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) " +
                    "AND ([LastName] = @LastName0 AND [LastName] != @LastName1))")]
        [InlineData(QueryConditionNodeType.And, QueryConditionNodeType.Or, QueryConditionNodeType.Or,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) " +
                    "AND ([LastName] = @LastName0 OR [LastName] != @LastName1))")]
        [InlineData(QueryConditionNodeType.Or, QueryConditionNodeType.And, QueryConditionNodeType.And,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) " +
                    "OR ([LastName] = @LastName0 AND [LastName] != @LastName1))")]
        [InlineData(QueryConditionNodeType.Or, QueryConditionNodeType.And, QueryConditionNodeType.Or,
                    "(([FirstName] = @FirstName0 AND [FirstName] = @FirstName1) " +
                    "OR ([LastName] = @LastName0 OR [LastName] != @LastName1))")]
        [InlineData(QueryConditionNodeType.Or, QueryConditionNodeType.Or, QueryConditionNodeType.And,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) " +
                    "OR ([LastName] = @LastName0 AND [LastName] != @LastName1))")]
        [InlineData(QueryConditionNodeType.Or, QueryConditionNodeType.Or, QueryConditionNodeType.Or,
                    "(([FirstName] = @FirstName0 OR [FirstName] = @FirstName1) " +
                    "OR ([LastName] = @LastName0 OR [LastName] != @LastName1))")]
        public void BalancedInterlacedConditionCombinationTests(
            QueryConditionNodeType combinationTypeOnFirstLayer,
            QueryConditionNodeType combinationInFirstInterlacedCondition,
            QueryConditionNodeType combinationInSecondInterlacedCondition,
            string expectedConditionString)
        {
            var condition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    combinationTypeOnFirstLayer,
                    () => new QueryConditionBuilder()
                        .DeclareConditionNode(
                            combinationInFirstInterlacedCondition,
                            () => new QueryConditionBuilder().DeclareCondition("FirstName", QueryConditionType.Equals, "Marcel"),
                            () => new QueryConditionBuilder().DeclareCondition("FirstName", QueryConditionType.Equals, "Viktoria")),
                    () => new QueryConditionBuilder()
                        .DeclareConditionNode(
                            combinationInSecondInterlacedCondition,
                            () => new QueryConditionBuilder().DeclareCondition("LastName", QueryConditionType.Equals, "Hirscher"),
                            () => new QueryConditionBuilder().DeclareCondition("LastName", QueryConditionType.NotEquals, "Mathis")))
                .Build();

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
