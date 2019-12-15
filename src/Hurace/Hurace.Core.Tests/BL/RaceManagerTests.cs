using FakeItEasy;
using Hurace.Core.BL;
using Hurace.Core.DAL;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hurace.Core.Tests.BL
{
    public class RaceManagerTests
    {
        [Fact]
        public async Task GetAllRacesTest()
        {
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fance description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "This is the second fance description",
                    Id = 1,
                    NumberOfSensors = 6,
                    RaceTypeId = 1
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "This is the third fance description",
                    Id = 2,
                    NumberOfSensors = 10,
                    RaceTypeId = 0
                }
            };

            var raceTypeEntities = new List<Entities.RaceType>
            {
                new Entities.RaceType
                {
                    Id = 0,
                    Label = "Racetype 1"
                },
                new Entities.RaceType
                {
                    Id = 1,
                    Label = "Racetype 2"
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);
            A.CallTo(() => raceTypeDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceTypeEntities);

            var raceManager = new RaceManager(null, raceDaoFake, raceTypeDaoFake, null, null, null);
            var raceDomainObjects = await raceManager.GetAllRacesAsync();

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);
                var matchingRaceTypeE = raceTypeEntities.First(raceTypeEntity => raceTypeEntity.Id == raceDO.RaceType.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);
                Assert.Equal(matchingRaceTypeE.Id, raceDO.RaceType.Id);
                Assert.Equal(matchingRaceTypeE.Label, raceDO.RaceType.Label);
            }

            //todo: validate all other missing dependencies that GetAllRacesAsync returns (venues, seasons, country)
            Assert.True(false);
        }

        //todo
        // test where get all finds not a matching racetype id
        // test where get all gets no raceentitites
    }
}
