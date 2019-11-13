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
            string expectedQuery = "SELECT [Label], [SkierIds], [Id] FROM [Hurace].[Sex]";
            

            var ssqg= new Db.Utilities.SimpleSqlQueryGenerator<Domain.Sex>(new List<string>());

            string createdString = ssqg.GenerateGetAllQuery();

            Assert.Equal(expectedQuery, createdString);
        }


        [Fact]
        public void GenerateSimpleInsertQuery()
        {
            //todo write test
            Assert.False(true);
        }

        [Fact]
        public void GenerateSimpleUpdateQuery()
        {
            //todo write test
            Assert.False(true);
        }

        [Fact]
        public void GenerateSimpleDeleteQuery()
        {
            //todo write test
            Assert.False(true);
        }
    }
}
