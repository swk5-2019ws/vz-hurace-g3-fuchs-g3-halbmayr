using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Hurace.Core.Tests.DbUtilityTests
{
    public class SimpleSqlQueryGeneratorTests
    {
        [Fact]
        public void GenerateSimpleSelectQueryEmptyList()
        {
            string expectedQuery = "SELECT [Label], [Id] FROM [Hurace].[Sex]";
            var queryGenerator = new Db.Utilities.SimpleSqlQueryGenerator<Domain.Sex>();
            string generatedQuery = queryGenerator.GenerateGetAllQuery();
            Assert.Equal(expectedQuery, generatedQuery);
        }

        [Fact]
        public void GenerateSimpleSelectByIdEmptyList()
        {
            string expectedQuery = "SELECT [Label], [Id] FROM [Hurace].[Sex] WHERE [Id] = @Id";
            var queryGenerator = new Db.Utilities.SimpleSqlQueryGenerator<Domain.Sex>();
            (var generatedQuery, var queryParameters) = queryGenerator.GenerateGetByIdQuery(1);
            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Equal(1, queryParameters[0].Value);
        }

        [Fact]
        public void GenerateSimpleInsertQuery()
        {
            string expectedParameterName = "Name";
            string expectedParameterValue = "AUS";
            string expectedQuery = $"INSERT INTO [Hurace].[Country] ([Name]) VALUES (@{expectedParameterName})";

            var queryGenerator = new Db.Utilities.SimpleSqlQueryGenerator<Domain.Country>();
            (var generatedQuery, var queryParameters) = queryGenerator.GenerateCreateQuery(new Domain.Country
            {
                Name = expectedParameterValue
            });

            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Single(queryParameters);
            Assert.Equal(expectedParameterName, queryParameters[0].ParameterName);
            Assert.Equal(expectedParameterValue, queryParameters[0].Value);
        }

        //INSERT INTO [Hurace].[StartPosition] ([Id], [StartListId], [SkierId], [Position]) VALUES (58, 1, 149, '18');
        [Fact]
        public void GenerateSimpleUpdateQuery()
        {
            string expectedQuery = "UPDATE [Hurace].[StartPosition] SET [StartListId] = @StartListId, [SkierId] = @SkierId, [Position] = @Position WHERE [Id] = @Id";
            var queryGenerator = new Db.Utilities.SimpleSqlQueryGenerator<Domain.StartPosition>();
            (var generatedQuery, var generatedParameters) = queryGenerator.GenerateUpdateQuery(new Domain.StartPosition
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
        public void GenerateSimpleDeleteQuery()
        {
            string expectedQuery = "DELETE FROM [Hurace].[StartPosition] WHERE Id = @Id";
            var queryGenerator = new Db.Utilities.SimpleSqlQueryGenerator<Domain.StartPosition>();
            (var generatedQuery, var generatedParameters) = queryGenerator.GenerateDeleteByIdQuery(135);

            Assert.Equal(expectedQuery, generatedQuery);

            Assert.Equal(135, generatedParameters[0].Value);
        }

        public static IEnumerable<object[]> GetInsertSkiers()
        {
            yield return new object[] { "Viktoria", "Rebensburg", new DateTime(1989, 10, 04), 0, 0, 0, 0 };
            yield return new object[] { "Tessa", "Worley", new DateTime(1989, 10, 04), 1, 1, 0, 0 };
        }

        [Theory]
        [MemberData(nameof(GetInsertSkiers))]
        public void TestSkierQuerys(string fn, string ln, DateTime dob, int countryId, int SexId, int imageId, int id)
        {
            string expectedQuery = "INSERT INTO [Hurace].[Skier] ([FirstName], [LastName], [DateOfBirth], [CountryId], [SexId], [ImageId]) VALUES " +
                "(@FirstName, @LastName, @DateOfBirth, @CountryId, @SexId, @ImageId)";
            var queryGenerator = new Db.Utilities.SimpleSqlQueryGenerator<Domain.Skier>();
            (var generatedQuery, var queryParameters) = queryGenerator.GenerateCreateQuery(
                new Domain.Skier
                {
                    FirstName = fn,
                    LastName = ln,
                    DateOfBirth = dob,
                    CountryId = countryId,
                    SexId = SexId,
                    ImageId = imageId,
                    Id = id
                });

            Assert.Equal(expectedQuery, generatedQuery);
            Assert.Equal(fn, queryParameters[0].Value);
            Assert.Equal(ln, queryParameters[1].Value);
            Assert.Equal(dob, queryParameters[2].Value);
            Assert.Equal(countryId, queryParameters[3].Value);
            Assert.Equal(SexId, queryParameters[4].Value);
            Assert.Equal(imageId, queryParameters[5].Value);
        }
    }
}
