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

            var raceEntity = new Entities.Race()
            {
                Date = DateTime.Now.AddDays(-1),
                Description = "This is a fancy description",
                Id = 0,
                FirstStartListId = 0,
                SecondStartListId = 1,
                NumberOfSensors = 5,
                RaceTypeId = 0,
                VenueId = 0
            };

            var raceTypeEntity = new Entities.RaceType()
            {
                Id = 0,
                Label = "Riesentorlauf"
            };

            A.CallTo(() => fakeRaceDao.GetAllConditionalAsync(A<IQueryCondition>._))
                .Returns(new List<Entities.Race>() { raceEntity });
            A.CallTo(() => fakeRaceDao.GetByIdAsync(0))
                .Returns(raceEntity);
            A.CallTo(() => fakeRaceTypeDao.GetAllConditionalAsync(A<IQueryCondition>._))
                .Returns(new List<Entities.RaceType>() { raceTypeEntity });

            var domainObjectMapper = new DomainObjectMapper(raceDao: fakeRaceDao, raceTypeDao: fakeRaceTypeDao);

            var raceManager = new RaceManager(domainObjectMapper, fakeRaceDao, fakeRaceTypeDao);

            var races = await raceManager.GetAllRacesAsync();

            Assert.Single(races);

            var actualRace = races.First();
            Assert.Equal(raceEntity.Date, actualRace.Date);
            Assert.Equal(raceEntity.Description, actualRace.Description);
            Assert.Equal(raceEntity.Id, actualRace.Id);
            Assert.Equal(raceEntity.NumberOfSensors, actualRace.NumberOfSensors);
            Assert.Equal(raceTypeEntity.Label, actualRace.RaceType);

            await Assert.ThrowsAsync<InvalidOperationException>(() => actualRace.FirstStartList);
            await Assert.ThrowsAsync<InvalidOperationException>(() => actualRace.SecondStartList);
            await Assert.ThrowsAsync<InvalidOperationException>(() => actualRace.Venue);
        }

        [Fact]
        public async Task GetAllRacesTest()
        {
            var fakeRaceDao = A.Fake<IDataAccessObject<Entities.Race>>();
            var fakeRaceTypeDao = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var fakeStartPositionDao = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var fakeVenueDao = A.Fake<IDataAccessObject<Entities.Venue>>();

            A.CallTo(() => fakeRaceDao.GetAllConditionalAsync(A<IQueryCondition>._))
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
            A.CallTo(() => fakeRaceTypeDao.GetAllConditionalAsync(A<IQueryCondition>._))
                .Returns(new List<Entities.RaceType>()
                {
                    new Entities.RaceType()
                    {
                        Id = 0,
                        Label = "Riesentorlauf"
                    }
                });
            A.CallTo(() => fakeStartPositionDao.GetAllConditionalAsync(A<IQueryCondition>._))
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
            A.CallTo(() => fakeVenueDao.GetAllConditionalAsync(A<IQueryCondition>._))
                .Returns(new List<Entities.Venue>()
                {
                    new Entities.Venue()
                    {
                        CountryId = 0,
                        Id = 0,
                        Name = "Austria"
                    }
                });

            var domainObjectMapper = new DomainObjectMapper(
                raceDao: fakeRaceDao,
                raceTypeDao: fakeRaceTypeDao,
                startPositionDao: fakeStartPositionDao,
                venueDao: fakeVenueDao);

            var raceManager = new RaceManager(domainObjectMapper, fakeRaceDao, fakeRaceTypeDao);
            var races = await raceManager.GetAllRacesAsync();

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

            // todo: add more sophisticated tests here like checking all other members of race
        }

        [Fact]
        public async Task RaceUpdateTest()
        {
            var fakeRaceDao = A.Fake<IDataAccessObject<Entities.Race>>();
            var fakeRaceTypeDao = A.Fake<IDataAccessObject<Entities.RaceType>>();
            
            var raceEntity = new Entities.Race()
            {
                Date = DateTime.Now.AddDays(-1),
                Description = "This is a fancy description",
                Id = 0,
                FirstStartListId = 0,
                SecondStartListId = 1,
                NumberOfSensors = 5,
                RaceTypeId = 0,
                VenueId = 0
            };

            var raceTypeEntity1 = new Entities.RaceType()
            {
                Id = 0,
                Label = "Riesentorlauf"
            };
            var raceTypeEntity2 = new Entities.RaceType()
            {
                Id = 1,
                Label = "Slalom"
            };

            var expectedUpdatedRaceEntity = new Entities.Race()
            {
                Date = DateTime.Now.AddDays(50),
                Description = "This is not a fancy description",
                NumberOfSensors = 1000,
                RaceTypeId = 1
            };
            Entities.Race actualUpdatedRaceEntity = null;
            A.CallTo(() => fakeRaceDao.GetAllConditionalAsync(A<IQueryCondition>._))
                .Returns(new List<Entities.Race>() { raceEntity });

            A.CallTo(() => fakeRaceDao.UpdateAsync(A<Entities.Race>._))
                .Invokes((Entities.Race newRace) => actualUpdatedRaceEntity = newRace)
                .Returns(true);

            A.CallTo(() => fakeRaceDao.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .Returns(raceEntity).Once()
                .Then.Returns(expectedUpdatedRaceEntity);

            A.CallTo(() => fakeRaceTypeDao.GetAllConditionalAsync(A<IQueryCondition>.That.IsNull()))
                .Returns(new List<Entities.RaceType>() { raceTypeEntity1, raceTypeEntity2 });

            A.CallTo(() => fakeRaceTypeDao.GetAllConditionalAsync(A<IQueryCondition>.That.IsNotNull()))
                .Returns(new List<Entities.RaceType>() { raceTypeEntity2 });

            var domainObjectMapper = new DomainObjectMapper(raceDao: fakeRaceDao, raceTypeDao: fakeRaceTypeDao);

            var raceManager = new RaceManager(domainObjectMapper, fakeRaceDao, fakeRaceTypeDao);

            var expectedRaceDomainObject = (await raceManager.GetAllRacesAsync()).First();

            Assert.False(expectedRaceDomainObject.PropertiesChanged);

            expectedRaceDomainObject.Date = expectedUpdatedRaceEntity.Date;
            Assert.True(expectedRaceDomainObject.PropertiesChanged);

            expectedRaceDomainObject.Description = expectedUpdatedRaceEntity.Description;
            Assert.True(expectedRaceDomainObject.PropertiesChanged);

            expectedRaceDomainObject.NumberOfSensors = expectedUpdatedRaceEntity.NumberOfSensors;
            Assert.True(expectedRaceDomainObject.PropertiesChanged);

            expectedRaceDomainObject.RaceType = "Slalom";
            Assert.True(expectedRaceDomainObject.PropertiesChanged);

            Assert.True(await raceManager.UpdateRaceAsync(expectedRaceDomainObject));

            Assert.Equal(expectedUpdatedRaceEntity.Date, actualUpdatedRaceEntity.Date);
            Assert.Equal(expectedUpdatedRaceEntity.Description, actualUpdatedRaceEntity.Description);
            Assert.Equal(expectedUpdatedRaceEntity.NumberOfSensors, actualUpdatedRaceEntity.NumberOfSensors);
            Assert.Equal(expectedUpdatedRaceEntity.RaceTypeId, actualUpdatedRaceEntity.RaceTypeId);

            var actualRaceDomainObject = (await raceManager.GetAllRacesAsync()).First();
            Assert.False(actualRaceDomainObject.PropertiesChanged);

            Assert.Equal(expectedRaceDomainObject.Date, actualRaceDomainObject.Date);
            Assert.Equal(expectedRaceDomainObject.Description, actualRaceDomainObject.Description);
            Assert.Equal(expectedRaceDomainObject.Id, actualRaceDomainObject.Id);
            Assert.Equal(expectedRaceDomainObject.NumberOfSensors, actualRaceDomainObject.NumberOfSensors);
            Assert.Equal(expectedRaceDomainObject.RaceType, actualRaceDomainObject.RaceType);

            Assert.False(actualRaceDomainObject.PropertiesChanged);
        }
    }
}
