using Hurace.Core.Db.Queries;
using Hurace.Core.Db.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

#pragma warning disable CA1054 // Uri parameters should not be strings
namespace Hurace.Core.Tests.DbUtilityTests
{
    public class SimpleSqlQueryGeneratorTests
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
        public void SimpleSelectQueryTest()
        {
            string expectedQuery = "SELECT [Label], [Id] FROM [Hurace].[Sex]";
            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Sex>();
            string generatedQuery = queryGenerator.GenerateGetAllConditionalQuery();
            Assert.Equal(expectedQuery, generatedQuery);
        }

        [Fact]
        public void GenerateSelectQueryWithConditionsTest()
        {
            string expectedQuery =
                "SELECT [FirstName], [LastName], [DateOfBirth], [ImageUrl], [CountryId], [SexId], [IsRemoved], [Id] " +
                "FROM [Hurace].[Skier] " +
                "WHERE ([Id] != 15 AND ([FirstName] = 'Marcel' OR [FirstName] = 'Viktoria'))";

            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Skier>();

            var conditions = new QueryConditionCombination()
            {
                CombinationType = QueryConditionCombination.Type.And,
                FirstCondition = new QueryCondition()
                {
                    ColumnToCheck = "Id",
                    CompareValue = 15,
                    ConditionType = QueryCondition.Type.NotEquals
                },
                SecondCondition = new QueryConditionCombination()
                {
                    CombinationType = QueryConditionCombination.Type.Or,
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
                }
            };

            string actualQuery = queryGenerator.GenerateGetAllConditionalQuery(conditions);

            Assert.Equal(expectedQuery, actualQuery);
        }

        [Fact]
        public void GenerateSelectByIdTest()
        {
            string expectedQuery = "SELECT [Label], [Id] FROM [Hurace].[Sex] WHERE [Id] = @Id";
            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Sex>();
            (var generatedQuery, var queryParameters) = queryGenerator.GenerateGetByIdQuery(1);
            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Equal(1, queryParameters[0].Value);
        }

        [Fact]
        public void GenerateGetLastIndentQueryTest()
        {
            string expected = "SELECT IDENT_CURRENT('[Hurace].[Skier]')";
            string actual = new SimpleSqlQueryGenerator<Domain.Skier>().GenerateGetLastIdentityQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GenerateInsertQueryTest()
        {
            string expectedParameterName = "Name";
            string expectedParameterValue = "AUS";
            string expectedQuery = $"INSERT INTO [Hurace].[Country] ([Name]) VALUES (@{expectedParameterName})";

            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Country>();
            (var generatedQuery, var queryParameters) = queryGenerator.GenerateCreateQuery(new Domain.Country
            {
                Name = expectedParameterValue
            });

            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Single(queryParameters);
            Assert.Equal(expectedParameterName, queryParameters[0].ParameterName);
            Assert.Equal(expectedParameterValue, queryParameters[0].Value);
        }

        [Fact]
        public void GenerateUpdateQueryTest()
        {
            string expectedQuery = "UPDATE [Hurace].[StartPosition] SET [StartListId] = @StartListId," +
                " [SkierId] = @SkierId, [Position] = @Position WHERE [Id] = @Id";

            var queryGenerator = new SimpleSqlQueryGenerator<Domain.StartPosition>();

            (var generatedQuery, var generatedParameters) =
                queryGenerator.GenerateUpdateQuery(
                    new Domain.StartPosition
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
        public void GenerateDeleteQueryTest()
        {
            string expectedQuery = "DELETE FROM [Hurace].[StartPosition] WHERE Id = @Id";
            var queryGenerator = new SimpleSqlQueryGenerator<Domain.StartPosition>();
            (var generatedQuery, var generatedParameters) = queryGenerator.GenerateDeleteByIdQuery(135);

            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Equal(135, generatedParameters[0].Value);
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
                "(@FirstName, @LastName, @DateOfBirth, @ImageUrl, @CountryId, @SexId, @IsRemoved)";

            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Skier>();
            (var generatedQuery, var queryParameters) = queryGenerator.GenerateCreateQuery(
                new Domain.Skier
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
            Assert.Equal(fn, queryParameters.FirstOrDefault(qp => qp.ParameterName == "FirstName").Value);
            Assert.Equal(ln, queryParameters.FirstOrDefault(qp => qp.ParameterName == "LastName").Value);
            Assert.Equal(dob, queryParameters.FirstOrDefault(qp => qp.ParameterName == "DateOfBirth").Value);
            Assert.Equal(url, queryParameters.FirstOrDefault(qp => qp.ParameterName == "ImageUrl").Value);
            Assert.Equal(countryId, queryParameters.FirstOrDefault(qp => qp.ParameterName == "CountryId").Value);
            Assert.Equal(sexId, queryParameters.FirstOrDefault(qp => qp.ParameterName == "SexId").Value);
            Assert.Equal(isRemoved ? "TRUE" : "FALSE", queryParameters.FirstOrDefault(qp => qp.ParameterName == "IsRemoved").Value);
        }

        [Fact]
        public void GenerateSelectByIdWithInvalidIdTest()
        {
            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Sex>();
            Assert.Throws<ArgumentOutOfRangeException>(() => queryGenerator.GenerateGetByIdQuery(-1));
        }

        [Fact]
        public static void GenerateCreateQueryWithInvalidParametersTest()
        {
            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Sex>();
            Assert.Throws<ArgumentNullException>(() => queryGenerator.GenerateCreateQuery(null));
        }

        [Fact]
        public static void GenerateUpdateQueryWithInvalidParameterTest1()
        {
            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Sex>();
            Assert.Throws<ArgumentNullException>(() => queryGenerator.GenerateUpdateQuery(null));
        }

        [Fact]
        public static void GenerateUpdateQueryWithInvalidParameterTest2()
        {
            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Country>();
            Assert.Throws<ArgumentOutOfRangeException>(
                () => queryGenerator.GenerateUpdateQuery(
                    new Domain.Country
                    {
                        Name = "AUS",
                        Id = -3
                    }));
        }

        [Fact]
        public void GenerateDeleteByIdQueryWithInvalidParameterTest()
        {
            var queryGenerator = new SimpleSqlQueryGenerator<Domain.Sex>();
            Assert.Throws<ArgumentOutOfRangeException>(() => queryGenerator.GenerateDeleteByIdQuery(-1));
        }
    }
}
