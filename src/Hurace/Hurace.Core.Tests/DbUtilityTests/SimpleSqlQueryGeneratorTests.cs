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
            var ssqg= new Db.Utilities.SimpleSqlQueryGenerator<Domain.Sex>();
            string createdString = ssqg.GenerateGetAllQuery();
            Assert.Equal(expectedQuery, createdString);
        }

        [Fact]
        public void GenerateSimpleSelectByIdEmptyList()
        {
            string expectedQuery = "SELECT [Label], [Id] FROM [Hurace].[Sex] WHERE [Id] = 1";
            var ssqg = new Db.Utilities.SimpleSqlQueryGenerator<Domain.Sex>();
            string createdString = ssqg.GenerateGetByIdQuery(1);
            Assert.Equal(expectedQuery, createdString);
        }

        [Fact]
        public void GenerateSimpleInsertQuery()
        {
            string expectedQuery = "INSERT INTO [Hurace].[Country] ([Name]) VALUES ('AUS')";
            var ssqg = new Db.Utilities.SimpleSqlQueryGenerator<Domain.Country>();
            string createdString = ssqg.GenerateCreateQuery(new Domain.Country{ Name = "AUS"});
            Assert.Equal(expectedQuery, createdString);
        }

        //INSERT INTO [Hurace].[StartPosition] ([Id], [StartListId], [SkierId], [Position]) VALUES (58, 1, 149, '18');
        [Fact]
        public void GenerateSimpleUpdateQuery()
        {
            string expectedQuery = "UPDATE [Hurace].[StartPosition] SET [StartListId] = 35, [SkierId] = 18, [Position] = 25 WHERE [Id] = 135";
            var ssqg = new Db.Utilities.SimpleSqlQueryGenerator<Domain.StartPosition>( );
            string createdString = ssqg.GenerateUpdateQuery(135, new Domain.StartPosition
            {
                Position = 25,
                SkierId = 18,
                StartListId = 35
            });
            Assert.Equal(expectedQuery, createdString);
        }

        [Fact]
        public void GenerateSimpleDeleteQuery()
        {
            string expectedQuery = "DELETE FROM [Hurace].[StartPosition] WHERE Id = 135";
            var ssqg = new Db.Utilities.SimpleSqlQueryGenerator<Domain.StartPosition>();
            string createdString = ssqg.GenerateDeleteByIdQuery(135);
            Assert.Equal(expectedQuery, createdString);
        }

        
        public class ParameterizedTests
        {

            public static IEnumerable<object[]> GetInsertSkiers()
            {
                yield return new object[] { "INSERT INTO [Hurace].[Skier] ([FirstName], [LastName], [DateOfBirth], [CountryId], [SexId], [ImageId]) VALUES " +
                    "('Viktoria', 'Rebensburg', '1989-10-04T00:00:00', 0, 0, 0)",
                    "Viktoria", "Rebensburg", new DateTime(1989,10,04), 0, 0, 0, 0 };
                yield return new object[] { "INSERT INTO [Hurace].[Skier] ([FirstName], [LastName], [DateOfBirth], [CountryId], [SexId], [ImageId]) VALUES " +
                    "('Tessa', 'Worley', '1989-10-04T00:00:00', 1, 1, 0)",
                    "Tessa", "Worley", new DateTime(1989,10,04), 1, 1, 0, 0 };
            }

            [Theory]
            [MemberData(nameof(GetInsertSkiers))]
            public void TestSkierQuerys(string expectedQuery, string  fn, string ln, DateTime dob, int countryId, int SexId, int imageId, int id)
            {
                var ssqg = new Db.Utilities.SimpleSqlQueryGenerator<Domain.Skier>();
                string createdString = ssqg.GenerateCreateQuery(
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
                Assert.Equal(expectedQuery, createdString);
            }
        }
    }
}
