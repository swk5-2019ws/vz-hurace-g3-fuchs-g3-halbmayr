using FakeItEasy;
using Hurace.Core.BL;
using Hurace.Core.DAL;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hurace.Core.Tests.BL
{
    public class RaceManagerTests
    {
        [Fact]
        public async Task GetAllRacesPlainTest()
        {
            var fakeRaceDao = A.Fake<IDataAccessObject<Entities.Race>>();
            var fakeRaceTypeDao = A.Fake<IDataAccessObject<Entities.RaceType>>();

            A.CallTo(() => fakeRaceDao.GetAllConditionalAsync(A<IQueryCondition>.Ignored))
                .Returns(new List<Entities.Race>()
                {
                    new Entities.Race()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        Description = "This is a fancy description",
                        Id = 0,
                        FirstStartListId = 0,
                        SecondStartListId = 1,
                        NumberOfSensors = 5,
                        RaceTypeId = 0,
                        VenueId = 0
                    }
                });
            A.CallTo(() => fakeRaceTypeDao.GetAllConditionalAsync(A<IQueryCondition>.Ignored))
                .Returns(new List<Entities.RaceType>()
                {
                    new Entities.RaceType()
                    {
                        Id = 0,
                        Label = "Riesentorlauf"
                    }
                });

            var domainObjectMapper = new DomainObjectMapper(raceDao: fakeRaceDao, raceTypeDao: fakeRaceTypeDao);

            var raceManager = new RaceManager(domainObjectMapper, fakeRaceDao);

            var races = await raceManager.GetAllRaces();

            Assert.Single(races);
            //add missing checks
            Assert.True(false);
            //add throw checks if there happens access deeper down
        }

        [Fact]
        public async Task GetAllRacesTest()
        {
            var fakeRaceDao = A.Fake<IDataAccessObject<Entities.Race>>();
            var fakeRaceTypeDao = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var fakeStartPositionDao = A.Fake<IDataAccessObject<Entities.StartPosition>>();

            A.CallTo(() => fakeRaceDao.GetAllConditionalAsync(A<IQueryCondition>.Ignored))
                .Returns(new List<Entities.Race>()
                {
                    new Entities.Race()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        Description = "This is a fancy description",
                        Id = 0,
                        FirstStartListId = 0,
                        SecondStartListId = 1,
                        NumberOfSensors = 5,
                        RaceTypeId = 0,
                        VenueId = 0
                    }
                });
            A.CallTo(() => fakeRaceTypeDao.GetAllConditionalAsync(A<IQueryCondition>.Ignored))
                .Returns(new List<Entities.RaceType>()
                {
                    new Entities.RaceType()
                    {
                        Id = 0,
                        Label = "Riesentorlauf"
                    }
                });
            A.CallTo(() => fakeStartPositionDao.GetAllConditionalAsync(A<IQueryCondition>.Ignored))
                .Returns(new List<Entities.StartPosition>()
                {
                    new Entities.StartPosition()
                    {
                        Id = 0,
                        Position = 1,
                        SkierId = 0,
                        StartListId = 0
                    },
                    new Entities.StartPosition()
                    {
                        Id = 1,
                        Position = 2,
                        SkierId = 1,
                        StartListId = 0
                    },
                    new Entities.StartPosition()
                    {
                        Id = 2,
                        Position = 1,
                        SkierId = 1,
                        StartListId = 1
                    },
                    new Entities.StartPosition()
                    {
                        Id = 3,
                        Position = 2,
                        SkierId = 2,
                        StartListId = 1
                    }
                });

            var domainObjectMapper = new DomainObjectMapper(
                raceDao: fakeRaceDao,
                raceTypeDao: fakeRaceTypeDao,
                startPositionDao: fakeStartPositionDao);

            var raceManager = new RaceManager(domainObjectMapper, fakeRaceDao);
            var races = await raceManager.GetAllRaces();

            foreach (var expectedRace in races)
            {
                var startLists = new List<IEnumerable<Domain.StartPosition>>()
                {
                    await expectedRace.FirstStartList,
                    await expectedRace.SecondStartList
                };

                foreach (var startList in startLists)
                {
                    foreach (var startPosition in startList)
                    {
                        var actualRace = await startPosition.Race;

                        Assert.Equal(expectedRace.Date, actualRace.Date);
                        Assert.Equal(expectedRace.Description, actualRace.Description);
                        Assert.Equal(expectedRace.Id, actualRace.Id);
                        Assert.Equal(expectedRace.NumberOfSensors, actualRace.NumberOfSensors);
                        Assert.Equal(expectedRace.RaceType, actualRace.RaceType);
                    }
                }
            }
        }
    }
}
