using Hurace.Core.Db.Queries;
using Hurace.Core.Db.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

#pragma warning disable CA1054 // Uri parameters should not be strings
namespace Hurace.Core.Db.Tests.UtilityTests
{
    public class SqlQueryGeneratorTests
    {
        public static IEnumerable<object[]> GetInsertSkiers
        {
            get
            {
                yield return new object[] { "Viktoria", "Rebensburg", new DateTime(1989, 10, 04), "https://nicetestimage", 0, 0, 0, false };
                yield return new object[] { "Tessa", "Worley", new DateTime(1989, 10, 04), "https://nicetestimage", 1, 1, 0, true };
            }
        }

        [Fact]
        public void GenerateBasicSelectQueryTest()
        {
            string expectedQueryString = "SELECT [Label], [Id] FROM [Hurace].[Sex]";
            var queryGenerator = new SqlQueryGenerator<Entities.Sex>();

            (var actualQueryString, var queryParameters) = queryGenerator.GenerateSelectQuery();

            Assert.Equal(expectedQueryString, actualQueryString);
            Assert.Empty(queryParameters);
        }

        [Fact]
        public void GenerateSelectQueryWithConditionsTest()
        {
            string expectedQueryFormat =
                "SELECT [FirstName], [LastName], [DateOfBirth], [ImageUrl], [CountryId], [SexId], [IsRemoved], [Id] " +
                "FROM [Hurace].[Skier] " +
                "WHERE ([Id] != @{0} AND ([FirstName] = @{1} OR [FirstName] = @{2}))";

            var queryGenerator = new SqlQueryGenerator<Entities.Skier>();

            var idExpectedValue = 15;
            var firstName1ExpectedValue = "Marcel";
            var firstName2ExpectedValue = "Viktoria";

            var conditions = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder().DeclareCondition("Id", QueryConditionType.NotEquals, idExpectedValue),
                    () => new QueryConditionBuilder()
                        .DeclareConditionNode(
                            QueryConditionNodeType.Or,
                            () => new QueryConditionBuilder().DeclareCondition("FirstName", QueryConditionType.Equals, firstName1ExpectedValue),
                            () => new QueryConditionBuilder().DeclareCondition("FirstName", QueryConditionType.Equals, firstName2ExpectedValue)))
                .Build();

            (var actualQuery, var queryParameters) = queryGenerator.GenerateSelectQuery(conditions);

            var idParameter =
                queryParameters.First(
                    qp => qp.ParameterName.Contains("Id0", StringComparison.OrdinalIgnoreCase));

            var firstName1Parameter =
                queryParameters.First(
                    qp => qp.ParameterName.Contains("FirstName0", StringComparison.OrdinalIgnoreCase));

            var firstName2Parameter =
                queryParameters.First(
                    qp => qp.ParameterName.Contains("FirstName1", StringComparison.OrdinalIgnoreCase));

            string expectedQuery = string.Format(
                expectedQueryFormat,
                idParameter.ParameterName,
                firstName1Parameter.ParameterName,
                firstName2Parameter.ParameterName);

            Assert.Equal(expectedQuery, actualQuery);
            Assert.Equal(idExpectedValue, idParameter.Value);
            Assert.Equal(firstName1ExpectedValue, firstName1Parameter.Value);
            Assert.Equal(firstName2ExpectedValue, firstName2Parameter.Value);
        }

        [Fact]
        public void GenerateSelectWithIdConditionTest()
        {
            string expectedQuery = "SELECT [Label], [Id] FROM [Hurace].[Sex] WHERE [Id] = @Id0";
            var queryGenerator = new SqlQueryGenerator<Entities.Sex>();

            (var generatedQuery, var queryParameters) = queryGenerator.GenerateSelectQuery(
                new QueryConditionBuilder()
                    .DeclareCondition("Id", QueryConditionType.Equals, 1)
                    .Build());

            Assert.Equal(expectedQuery, generatedQuery);
            Assert.Equal(1, queryParameters[0].Value);
        }

        [Fact]
        public void GenerateInsertQueryTest()
        {
            string expectedParameterName = "Name0";
            string expectedParameterValue = "AUS";
            string expectedQuery = $"INSERT INTO [Hurace].[Country] ([Name]) VALUES (@{expectedParameterName})";

            var queryGenerator = new SqlQueryGenerator<Entities.Country>();
            (var generatedQuery, var queryParameters) =
                queryGenerator.GenerateInsertQuery(
                new Entities.Country
                {
                    Name = expectedParameterValue
                });

            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Single(queryParameters);
            Assert.Equal(expectedParameterName, queryParameters[0].ParameterName);
            Assert.Equal(expectedParameterValue, queryParameters[0].Value);
        }

        [Theory]
        [MemberData(nameof(GetInsertSkiers))]
        public void GenerateSkierInsertQueryTest(
            string fn,
            string ln,
            DateTime dob,
            string url,
            int countryId,
            int sexId,
            int id,
            bool isRemoved)
        {
            string expectedQuery = "INSERT INTO [Hurace].[Skier] " +
                "([FirstName], [LastName], [DateOfBirth], [ImageUrl], [CountryId], [SexId], [IsRemoved]) VALUES " +
                "(@FirstName0, @LastName0, @DateOfBirth0, @ImageUrl0, @CountryId0, @SexId0, @IsRemoved0)";

            var queryGenerator = new SqlQueryGenerator<Entities.Skier>();
            (var generatedQuery, var queryParameters) = queryGenerator.GenerateInsertQuery(
                new Entities.Skier
                {
                    FirstName = fn,
                    LastName = ln,
                    DateOfBirth = dob,
                    ImageUrl = url,
                    CountryId = countryId,
                    SexId = sexId,
                    Id = id,
                    IsRemoved = isRemoved
                });

            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Equal(fn, queryParameters.FirstOrDefault(qp => qp.ParameterName == "FirstName0").Value);
            Assert.Equal(ln, queryParameters.FirstOrDefault(qp => qp.ParameterName == "LastName0").Value);
            Assert.Equal(dob.ToString("s"), queryParameters.FirstOrDefault(qp => qp.ParameterName == "DateOfBirth0").Value);
            Assert.Equal(url, queryParameters.FirstOrDefault(qp => qp.ParameterName == "ImageUrl0").Value);
            Assert.Equal(countryId, queryParameters.FirstOrDefault(qp => qp.ParameterName == "CountryId0").Value);
            Assert.Equal(sexId, queryParameters.FirstOrDefault(qp => qp.ParameterName == "SexId0").Value);

            Assert.Equal(
                isRemoved ? "TRUE" : "FALSE",
                queryParameters.FirstOrDefault(qp => qp.ParameterName == "IsRemoved0").Value);
        }

        [Fact]
        public static void GenerateInsertQueryWithInvalidParametersTest()
        {
            var queryGenerator = new SqlQueryGenerator<Entities.Sex>();
            Assert.Throws<ArgumentNullException>(() => queryGenerator.GenerateInsertQuery(null));
        }

        [Fact]
        public void GenerateUpdateQueryTest()
        {
            string expectedQuery = "UPDATE [Hurace].[StartPosition] " +
                "SET [StartListId] = @StartListId0, [SkierId] = @SkierId0, [Position] = @Position0 " +
                "WHERE [Id] = @Id0";

            var queryGenerator = new SqlQueryGenerator<Entities.StartPosition>();

            (var generatedQuery, var generatedParameters) =
                queryGenerator.GenerateUpdateQuery(
                    new Entities.StartPosition
                    {
                        StartListId = 35,
                        SkierId = 18,
                        Position = 25,
                        Id = 135
                    });

            Assert.Equal(expectedQuery, generatedQuery);
            Assert.Equal(35, generatedParameters[0].Value);
            Assert.Equal(18, generatedParameters[1].Value);
            Assert.Equal(25, generatedParameters[2].Value);
            Assert.Equal(135, generatedParameters[3].Value);
        }

        [Fact]
        public void GenerateUpdateQueryWithConditionsTest()
        {
            var updateCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder()
                        .DeclareConditionNode(
                            QueryConditionNodeType.And,
                            () => new QueryConditionBuilder()
                                .DeclareCondition("FirstName", QueryConditionType.NotEquals, "Marcel"),
                            () => new QueryConditionBuilder()
                                .DeclareCondition("DateOfBirth", QueryConditionType.LessThan, new DateTime(2005, 1, 1))),
                    () => new QueryConditionBuilder()
                        .DeclareConditionNode(
                            QueryConditionNodeType.Or,
                            () => new QueryConditionBuilder()
                                .DeclareCondition("LastName", QueryConditionType.Equals, "Halbmayr"),
                            () => new QueryConditionBuilder()
                                .DeclareCondition("LastName", QueryConditionType.Equals, "Fuchs")))
                .Build();

            var expectedUpdatedDateOfBirth = new DateTime(2001, 1, 1);
            var expectedUpdatedImageUrl = "https://robohash.org/1";
            var expectedUpdatedSexId = 0;

            var updatedObject = new
            {
                DateOfBirth = expectedUpdatedDateOfBirth,
                ImageUrl = expectedUpdatedImageUrl,
                SexId = expectedUpdatedSexId
            };

            string expectedUpdateQuery =
                "UPDATE [Hurace].[Skier] " +
                "SET [DateOfBirth] = @DateOfBirth0, [ImageUrl] = @ImageUrl0, [SexId] = @SexId0 " +
                "WHERE (([FirstName] != @FirstName0 AND [DateOfBirth] < @DateOfBirth1)" +
                " AND ([LastName] = @LastName0 OR [LastName] = @LastName1))";

            var queryGenerator = new SqlQueryGenerator<Entities.Skier>();

            (var actualUpdateQuery, var queryParameters) = queryGenerator.GenerateUpdateQuery(updatedObject, updateCondition);

            Assert.Equal(expectedUpdateQuery, actualUpdateQuery);
            Assert.Equal(7, queryParameters.Length);

            var updateDateOfBirthParam = queryParameters.First(qp => qp.ParameterName == "DateOfBirth0");
            var updatedImageUrlParam = queryParameters.First(qp => qp.ParameterName == "ImageUrl0");
            var updateSexIdParam = queryParameters.First(qp => qp.ParameterName == "SexId0");

            Assert.Equal(expectedUpdatedDateOfBirth.ToString("s"), updateDateOfBirthParam.Value);
            Assert.Equal(expectedUpdatedImageUrl, updatedImageUrlParam.Value);
            Assert.Equal(expectedUpdatedSexId, updateSexIdParam.Value);

            var conditionFirstName = queryParameters.First(qp => qp.ParameterName == "FirstName0");
            var conditionDateOfBirth = queryParameters.First(qp => qp.ParameterName == "DateOfBirth1");
            var conditionLastName1 = queryParameters.First(qp => qp.ParameterName == "LastName0");
            var conditionLastName2 = queryParameters.First(qp => qp.ParameterName == "LastName1");

            Assert.Equal("Marcel", conditionFirstName.Value);
            Assert.Equal("2005-01-01T00:00:00", conditionDateOfBirth.Value);
            Assert.Equal("Halbmayr", conditionLastName1.Value);
            Assert.Equal("Fuchs", conditionLastName2.Value);
        }

        [Fact]
        public static void GenerateUpdateQueryWithInvalidParameterTest1()
        {
            var queryGenerator = new SqlQueryGenerator<Entities.Sex>();
            Assert.Throws<ArgumentNullException>(() => queryGenerator.GenerateUpdateQuery(null));
        }

        [Fact]
        public static void GenerateUpdateQueryWithInvalidParameterTest2()
        {
            var queryGenerator = new SqlQueryGenerator<Entities.Country>();
            Assert.Throws<ArgumentOutOfRangeException>(
                () => queryGenerator.GenerateUpdateQuery(
                    new Entities.Country
                    {
                        Name = "AUS",
                        Id = -3
                    }));
        }

        [Fact]
        public static void GenerateUpdateQueryWithInvalidParameterTest3()
        {
            var queryGenerator = new SqlQueryGenerator<Entities.Skier>();
            Assert.Throws<ArgumentNullException>(() => queryGenerator.GenerateUpdateQuery(null, null));
        }

        [Fact]
        public static void GenerateUpdateQueryWithInvalidParameterTest4()
        {
            var queryGenerator = new SqlQueryGenerator<Entities.Skier>();
            Assert.Throws<ArgumentNullException>(() => queryGenerator.GenerateUpdateQuery(new { }, null));
        }

        [Fact]
        public void GenerateDeleteByIdQueryTest()
        {
            string expectedQuery = "DELETE FROM [Hurace].[StartPosition] WHERE [Id] = @Id0";

            int idToDelete = 135;

            var queryGenerator = new SqlQueryGenerator<Entities.StartPosition>();
            (var generatedQuery, var queryParameters) = queryGenerator.GenerateDeleteQuery(idToDelete);

            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Single(queryParameters);
            Assert.Equal(idToDelete, queryParameters.First().Value);
        }

        [Fact]
        public void GenerateDeleteWithConditionsParameterTest()
        {
            var condition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder().DeclareCondition("FirstName", QueryConditionType.NotEquals, "Stevenie"),
                    () => new QueryConditionBuilder()
                        .DeclareConditionNode(
                            QueryConditionNodeType.Or,
                            () => new QueryConditionBuilder()
                                .DeclareCondition("LastName", QueryConditionType.NotEquals, "Zeitlhofinger"),
                            () => new QueryConditionBuilder()
                                .DeclareCondition("LastName", QueryConditionType.Like, "Halbmayr")))
                .Build();

            string expectedQuery = "DELETE FROM [Hurace].[Skier] " +
                "WHERE ([FirstName] != @FirstName0 AND ([LastName] != @LastName0 OR [LastName] LIKE @LastName1))";

            var queryGenerator = new SqlQueryGenerator<Entities.Skier>();

            (var actualQuery, var actualQueryParameters) = queryGenerator.GenerateDeleteQuery(condition);

            Assert.Equal(expectedQuery, actualQuery);
            Assert.Equal(3, actualQueryParameters.Length);

            var firstNameParameter = actualQueryParameters.First(qp => qp.ParameterName == "FirstName0");
            var lastNameParameter1 = actualQueryParameters.First(qp => qp.ParameterName == "LastName0");
            var lastNameParameter2 = actualQueryParameters.First(qp => qp.ParameterName == "LastName1");

            Assert.Equal("Stevenie", firstNameParameter.Value);
            Assert.Equal("Zeitlhofinger", lastNameParameter1.Value);
            Assert.Equal("Halbmayr", lastNameParameter2.Value);
        }

        [Fact]
        public void GenerateGetLastIndentyQueryTest()
        {
            string expected = "SELECT IDENT_CURRENT('[Hurace].[Skier]')";
            string actual = new SqlQueryGenerator<Entities.Skier>().GenerateGetLastIdentityQuery();

            Assert.Equal(expected, actual);
        }
    }
}
