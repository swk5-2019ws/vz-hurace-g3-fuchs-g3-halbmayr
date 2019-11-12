using FakeItEasy;
using Hurace.Core.Dal;
using Hurace.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Hurace.Test.UnitTests.CoreTests
{
    public class DaoFactoryTests
    {
        [Fact]
        public async Task TestInjectFakedSkierDao()
        {
            IDataAccessObject<Skier> fakedSkierDao = A.Fake<IDataAccessObject<Skier>>();

            var fakedCollection = new List<Skier>()
                {
                    new Skier()
                    {
                        Id = 52,
                        DateOfBirth = new DateTime(1992, 7, 15),
                        FirstName = "Test first name",
                        LastName = "test last name"
                    }
                };

            A.CallTo(() => fakedSkierDao.GetAllAsync()).Returns(fakedCollection);

            DaoFactory.ApplyDaoSet(fakedSkierDao);

            var skierDao = DaoFactory.CreateSkierDao();

            Assert.Equal(fakedCollection, await skierDao.GetAllAsync());
        }
    }
}
