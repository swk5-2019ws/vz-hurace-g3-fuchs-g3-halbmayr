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
        public void InstantiateRaceInformationManagerWrongTest1()
        {
            IDataAccessObject<Entities.Country> countryDaoFake = null;
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest2()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            IDataAccessObject<Entities.Race> raceDaoFake = null;
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest3()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            IDataAccessObject<Entities.RaceType> raceTypeDaoFake = null;
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest4()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            IDataAccessObject<Entities.Skier> skierDaoFake = null;
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest5()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            IDataAccessObject<Entities.Season> seasonDaoFake = null;
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest6()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            IDataAccessObject<Entities.SeasonPlan> seasonPlanDaoFake = null;
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest7()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            IDataAccessObject<Entities.Sex> sexDaoFake = null;
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest8()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            IDataAccessObject<Entities.StartList> startListDaoFake = null;
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest9()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            IDataAccessObject<Entities.StartPosition> startPositionDaoFake = null;
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }

        [Fact]
        public void InstantiateRaceInformationManagerWrongTest10()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            IDataAccessObject<Entities.Venue> venueDaoFake = null;

            Assert.Throws<ArgumentNullException>(() =>
                new InformationManager(
                    countryDaoFake,
                    raceDaoFake,
                    raceTypeDaoFake,
                    seasonDaoFake,
                    seasonPlanDaoFake,
                    sexDaoFake,
                    skierDaoFake,
                    startListDaoFake,
                    startPositionDaoFake,
                    venueDaoFake));
        }


        [Fact]
        public async Task GetAllRacesWithRaceTypeReferenceTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

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

            A.CallTo(() => raceTypeDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily((call) => Task.FromResult(raceTypeEntities.First(rte => rte.Id == 0)));
            A.CallTo(() => raceTypeDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily((call) => Task.FromResult(raceTypeEntities.First(rte => rte.Id == 1)));

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.None,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.None);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);
                var matchingRaceTypeE = raceTypeEntities.First(raceTypeEntity => raceTypeEntity.Id == raceDO.RaceType.Reference.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                Assert.Equal(matchingRaceTypeE.Id, raceDO.RaceType.Reference.Id);
                Assert.Equal(matchingRaceTypeE.Label, raceDO.RaceType.Reference.Label);
            }
        }

        [Fact]
        public async Task GetAllRacesWithRaceTypeForeignKeyTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

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

            A.CallTo(() => raceTypeDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily((call) => Task.FromResult(raceTypeEntities.First(rte => rte.Id == 0)));
            A.CallTo(() => raceTypeDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily((call) => Task.FromResult(raceTypeEntities.First(rte => rte.Id == 1)));

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.None,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.None);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);
                var matchingRaceTypeE = raceTypeEntities.First(raceTypeEntity => raceTypeEntity.Id == raceDO.RaceType.ForeignKey);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                Assert.Equal(matchingRaceTypeE.Id, raceDO.RaceType.ForeignKey);
                Assert.Null(raceDO.RaceType.Reference);
            }
        }

        [Fact]
        public async Task GetAllRacesWithNotFoundRaceTypeEntityTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fancy description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "This is the second fancy description",
                    Id = 1,
                    NumberOfSensors = 6,
                    RaceTypeId = 1
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "This is the third fancy description",
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
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);

            A.CallTo(() => raceTypeDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily((call) => Task.FromResult(raceTypeEntities.First(rte => rte.Id == 0)));
            A.CallTo(() => raceTypeDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily((call) => Task.FromResult<Entities.RaceType>(null));

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.None,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.None);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                if (raceDO.RaceType.Reference is null)
                {
                    Assert.Null(raceDO.RaceType.ForeignKey);
                    Assert.Null(raceDO.RaceType.Reference);
                }
                else
                {
                    var matchingRaceTypeE = raceTypeEntities.FirstOrDefault(
                        raceTypeEntity => raceTypeEntity.Id == raceDO.RaceType.Reference.Id);

                    Assert.Equal(matchingRaceTypeE.Id, raceDO.RaceType.Reference.Id);
                    Assert.Equal(matchingRaceTypeE.Label, raceDO.RaceType.Reference.Label);
                }
            }
        }

        [Fact]
        public async Task GetAllRacesWithVenueReferenceTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fancy description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0,
                    VenueId = 10
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "This is the second fancy description",
                    Id = 1,
                    NumberOfSensors = 6,
                    RaceTypeId = 1,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "This is the third fancy description",
                    Id = 2,
                    NumberOfSensors = 10,
                    RaceTypeId = 0,
                    VenueId = 1
                }
            };

            var venueEntities = new List<Entities.Venue>
            {
                new Entities.Venue
                {
                    Id = 10,
                    Name = "Austria"
                },
                new Entities.Venue
                {
                    Id = 0,
                    Name = "Halbmayerhausen"
                },
                new Entities.Venue
                {
                    Id = 1,
                    Name = "Hagenberg"
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);

            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(10)))
                .ReturnsLazily((call) => Task.FromResult(venueEntities.First(rte => rte.Id == 10)));
            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily((call) => Task.FromResult(venueEntities.First(rte => rte.Id == 0)));
            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily((call) => Task.FromResult(venueEntities.First(rte => rte.Id == 1)));

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.None,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.None);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                var matchingVenueE = venueEntities.FirstOrDefault(
                    venueE => venueE.Id == raceDO.Venue.Reference.Id);

                Assert.Equal(matchingVenueE.Id, raceDO.Venue.Reference.Id);
                Assert.Equal(matchingVenueE.Name, raceDO.Venue.Reference.Name);
            }
        }

        [Fact]
        public async Task GetAllRacesWithVenueForeignKeyTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fancy description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0,
                    VenueId = 10
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "This is the second fancy description",
                    Id = 1,
                    NumberOfSensors = 6,
                    RaceTypeId = 1,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "This is the third fancy description",
                    Id = 2,
                    NumberOfSensors = 10,
                    RaceTypeId = 0,
                    VenueId = 1
                }
            };

            var venueEntities = new List<Entities.Venue>
            {
                new Entities.Venue
                {
                    Id = 10,
                    Name = "Austria"
                },
                new Entities.Venue
                {
                    Id = 0,
                    Name = "Halbmayerhausen"
                },
                new Entities.Venue
                {
                    Id = 1,
                    Name = "Hagenberg"
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);

            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(10)))
                .ReturnsLazily((call) => Task.FromResult(venueEntities.First(rte => rte.Id == 10)));
            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily((call) => Task.FromResult(venueEntities.First(rte => rte.Id == 0)));
            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily((call) => Task.FromResult(venueEntities.First(rte => rte.Id == 1)));

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.None,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.None);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                var matchingVenueE = venueEntities.FirstOrDefault(
                    venueE => venueE.Id == raceDO.Venue.ForeignKey);

                Assert.Equal(matchingVenueE.Id, raceDO.Venue.ForeignKey);
                Assert.Null(raceDO.Venue.Reference);
            }
        }

        [Fact]
        public async Task GetAllRacesWithNotExistingVenueTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fancy description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0,
                    VenueId = 10
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "This is the second fancy description",
                    Id = 1,
                    NumberOfSensors = 6,
                    RaceTypeId = 1,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "This is the third fancy description",
                    Id = 2,
                    NumberOfSensors = 10,
                    RaceTypeId = 0,
                    VenueId = 1
                }
            };

            var venueEntities = new List<Entities.Venue>
            {
                new Entities.Venue
                {
                    Id = 10,
                    Name = "Austria"
                },
                new Entities.Venue
                {
                    Id = 0,
                    Name = "Halbmayerhausen"
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);

            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(10)))
                .ReturnsLazily(() => venueEntities.First(rte => rte.Id == 10));
            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily(() => venueEntities.First(rte => rte.Id == 0));
            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily(() => Task.FromResult<Entities.Venue>(null));

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.None,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.None);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                if (raceDO.Venue.Reference is null)
                    Assert.Equal(matchingRaceE.VenueId, raceDO.Venue.ForeignKey);
                else
                {
                    var matchingVenueE = venueEntities.FirstOrDefault(
                        venueE => venueE.Id == raceDO.Venue.ForeignKey);

                    Assert.Equal(matchingVenueE.Id, raceDO.Venue.ForeignKey);
                    Assert.Null(raceDO.Venue.Reference);
                }
            }
        }

        [Fact]
        public async Task GetAllRacesWithSeasonReferenceTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fancy description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "This is the second fancy description",
                    Id = 1,
                    NumberOfSensors = 6,
                    RaceTypeId = 1,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "This is the third fancy description",
                    Id = 2,
                    NumberOfSensors = 10,
                    RaceTypeId = 0,
                    VenueId = 0
                }
            };

            var venueEntities = new List<Entities.Venue>
            {
                new Entities.Venue
                {
                    Id = 0,
                    Name = "Halbmayerhausen"
                }
            };

            var seasonPlanEntities = new List<Entities.SeasonPlan>
            {
                new Entities.SeasonPlan
                {
                    Id = 0,
                    SeasonId = 0,
                    VenueId = 0
                },
                new Entities.SeasonPlan
                {
                    Id = 1,
                    SeasonId = 1,
                    VenueId = 0
                }
            };

            var seasonEntities = new List<Entities.Season>
            {
                new Entities.Season
                {
                    Id = 0,
                    Name = "Season 1",
                    StartDate = DateTime.Now.AddDays(-20),
                    EndDate = DateTime.Now.AddDays(-8)
                },
                new Entities.Season
                {
                    Id = 1,
                    Name = "Season 2",
                    StartDate = DateTime.Now.AddDays(-7),
                    EndDate = DateTime.Now.AddDays(5)
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);

            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily(() => venueEntities.First(rte => rte.Id == 0));

            A.CallTo(() => seasonPlanDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => seasonPlanEntities);

            A.CallTo(() => seasonDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.Season> { seasonEntities.First() }).Twice().Then
                .ReturnsLazily(() => new List<Entities.Season> { seasonEntities.Skip(1).First() });

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.None,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.None,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                var matchingSeasonE = seasonEntities.FirstOrDefault(
                    seasonE => seasonE.Id == raceDO.Season.Reference.Id);

                Assert.Equal(matchingSeasonE.Id, raceDO.Season.Reference.Id);
                Assert.Equal(matchingSeasonE.Name, raceDO.Season.Reference.Name);
                Assert.Equal(matchingSeasonE.StartDate, raceDO.Season.Reference.StartDate);
                Assert.Equal(matchingSeasonE.EndDate, raceDO.Season.Reference.EndDate);
                Assert.InRange(matchingRaceE.Date, raceDO.Season.Reference.StartDate, raceDO.Season.Reference.EndDate);
            }
        }

        [Fact]
        public async Task GetAllRacesWithSeasonForeignKeyTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fancy description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "This is the second fancy description",
                    Id = 1,
                    NumberOfSensors = 6,
                    RaceTypeId = 1,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "This is the third fancy description",
                    Id = 2,
                    NumberOfSensors = 10,
                    RaceTypeId = 0,
                    VenueId = 0
                }
            };

            var venueEntities = new List<Entities.Venue>
            {
                new Entities.Venue
                {
                    Id = 0,
                    Name = "Halbmayerhausen"
                }
            };

            var seasonPlanEntities = new List<Entities.SeasonPlan>
            {
                new Entities.SeasonPlan
                {
                    Id = 0,
                    SeasonId = 0,
                    VenueId = 0
                },
                new Entities.SeasonPlan
                {
                    Id = 1,
                    SeasonId = 1,
                    VenueId = 0
                }
            };

            var seasonEntities = new List<Entities.Season>
            {
                new Entities.Season
                {
                    Id = 0,
                    Name = "Season 1",
                    StartDate = DateTime.Now.AddDays(-20),
                    EndDate = DateTime.Now.AddDays(-8)
                },
                new Entities.Season
                {
                    Id = 1,
                    Name = "Season 2",
                    StartDate = DateTime.Now.AddDays(-7),
                    EndDate = DateTime.Now.AddDays(5)
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);

            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily(() => venueEntities.First(rte => rte.Id == 0));

            A.CallTo(() => seasonPlanDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => seasonPlanEntities);

            A.CallTo(() => seasonDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.Season> { seasonEntities.First() }).Twice().Then
                .ReturnsLazily(() => new List<Entities.Season> { seasonEntities.Skip(1).First() });

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.None,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.None,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.ForeignKey);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                var matchingSeasonE = seasonEntities.FirstOrDefault(
                    seasonE => seasonE.Id == raceDO.Season.ForeignKey);

                Assert.Equal(matchingSeasonE.Id, raceDO.Season.ForeignKey);
            }
        }

        [Fact]
        public async Task GetAllRacesWithNotExistingSeasonTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fancy description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "This is the second fancy description",
                    Id = 1,
                    NumberOfSensors = 6,
                    RaceTypeId = 1,
                    VenueId = 0
                },
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-5),
                    Description = "This is the third fancy description",
                    Id = 2,
                    NumberOfSensors = 10,
                    RaceTypeId = 0,
                    VenueId = 0
                }
            };

            var venueEntities = new List<Entities.Venue>
            {
                new Entities.Venue
                {
                    Id = 0,
                    Name = "Halbmayerhausen"
                }
            };

            var seasonPlanEntities = new List<Entities.SeasonPlan>
            {
                new Entities.SeasonPlan
                {
                    Id = 0,
                    SeasonId = 0,
                    VenueId = 0
                },
                new Entities.SeasonPlan
                {
                    Id = 1,
                    SeasonId = 1,
                    VenueId = 0
                }
            };

            var seasonEntities = new List<Entities.Season>
            {
                new Entities.Season
                {
                    Id = 0,
                    Name = "Season 1",
                    StartDate = DateTime.Now.AddDays(-20),
                    EndDate = DateTime.Now.AddDays(-8)
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);

            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily(() => venueEntities.First(rte => rte.Id == 0));

            A.CallTo(() => seasonPlanDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => seasonPlanEntities);

            A.CallTo(() => seasonDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.Season> { seasonEntities.First() }).Twice().Then
                .ReturnsLazily(() => new List<Entities.Season> { });

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceDomainObjects = await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.None,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.None,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.ForeignKey);

            foreach (var raceDO in raceDomainObjects)
            {
                var matchingRaceE = raceEntities.First(raceEntity => raceEntity.Id == raceDO.Id);

                Assert.Equal(matchingRaceE.Date, raceDO.Date);
                Assert.Equal(matchingRaceE.Description, raceDO.Description);
                Assert.Equal(matchingRaceE.Id, raceDO.Id);
                Assert.Equal(matchingRaceE.NumberOfSensors, raceDO.NumberOfSensors);

                var matchingSeasonE = seasonEntities.FirstOrDefault(
                    seasonE => seasonE.Id == raceDO.Season.ForeignKey);

                Assert.Null(raceDO.Season.Reference);
            }
        }

        [Fact]
        public async Task GetRaceByIdWithStartlistReferences()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            var raceEntities = new List<Entities.Race>
            {
                new Entities.Race
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "This is the first fancy description",
                    Id = 0,
                    NumberOfSensors = 1,
                    RaceTypeId = 0,
                    VenueId = 0,
                    FirstStartListId = 0,
                    SecondStartListId = 1
                }
            };

            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => raceEntities);

            var startPositionEntities = new List<Entities.StartPosition>
            {
                new Entities.StartPosition
                {
                    Id = 0,
                    Position = 1,
                    SkierId = 0,
                    StartListId = 0
                },
                new Entities.StartPosition
                {
                    Id = 1,
                    Position = 2,
                    SkierId = 1,
                    StartListId = 0
                },
                new Entities.StartPosition
                {
                    Id = 2,
                    Position = 1,
                    SkierId = 0,
                    StartListId = 1
                },
                new Entities.StartPosition
                {
                    Id = 3,
                    Position = 2,
                    SkierId = 1,
                    StartListId = 1
                }
            };

            A.CallTo(() => startPositionDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => startPositionEntities.Where(sp => sp.StartListId == 0)).Once().Then
                .ReturnsLazily(() => startPositionEntities.Where(sp => sp.StartListId == 1));

            var skierEntities = new List<Entities.Skier>
            {
                new Entities.Skier
                {
                    Id = 0,
                    FirstName = "Thomas",
                    LastName = "Halbimayr"
                },
                new Entities.Skier
                {
                    Id = 1,
                    FirstName = "Manuela",
                    LastName = "Wolf (haha, funny joke vom thomas)"
                }
            };

            A.CallTo(() => skierDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily(() => skierEntities.First(s => s.Id == 0));
            A.CallTo(() => skierDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily(() => skierEntities.First(s => s.Id == 1));

            var raceManager = new InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var race = await raceManager.GetRaceByIdAsync(
                0,
                startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.Reference,
                skierLoadingType: Domain.Associated<Domain.Skier>.LoadingType.Reference);

            Assert.False(true);
            await Task.CompletedTask;
        }
    }
}
