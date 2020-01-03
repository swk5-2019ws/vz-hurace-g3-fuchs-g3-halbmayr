using FakeItEasy;
using Hurace.Core.DAL;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hurace.Core.Tests.BL
{
    public class RaceExecutionManagerTests
    {
        [Fact]
        public void CallCtorWithWrongParameterTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Core.BL.RaceExecutionManager(null));
        }

        [Fact]
        public async Task RaceIsStartableTest1()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceDataDaoFake = A.Fake<IDataAccessObject<Entities.RaceData>>();
            var raceStateDaoFake = A.Fake<IDataAccessObject<Entities.RaceState>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var timeMeasurementDaoFake = A.Fake<IDataAccessObject<Entities.TimeMeasurement>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            int raceId = 0;
            A.CallTo(() => raceDaoFake.GetByIdAsync(A<int>._))
                .ReturnsLazily(() => new Entities.Race()
                {
                    Date = DateTime.Now.Date,
                    Id = raceId,
                    FirstStartListId = 0,
                    SecondStartListId = 1
                });
            A.CallTo(() => startPositionDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.StartPosition>
                {
                    new Entities.StartPosition
                    {
                        Id = 0,
                        Position = 1,
                        StartListId = 0
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.StartPosition>
                {
                    new Entities.StartPosition
                    {
                        Id = 1,
                        Position = 1,
                        StartListId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.StartPosition>>(null));

            var informationManager = new Core.BL.InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceDataDaoFake,
                raceStateDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                timeMeasurementDaoFake,
                venueDaoFake);

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            Assert.True(await raceExecutionManager.IsRaceStartable(raceId));
        }

        [Fact]
        public async Task RaceIsStartableTest2()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceDataDaoFake = A.Fake<IDataAccessObject<Entities.RaceData>>();
            var raceStateDaoFake = A.Fake<IDataAccessObject<Entities.RaceState>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var timeMeasurementDaoFake = A.Fake<IDataAccessObject<Entities.TimeMeasurement>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            int raceId = 0;
            A.CallTo(() => raceDaoFake.GetByIdAsync(A<int>._))
                .ReturnsLazily(() => new Entities.Race()
                {
                    Date = DateTime.Now.Date,
                    Id = raceId,
                    FirstStartListId = 0,
                    SecondStartListId = 1
                });
            A.CallTo(() => startPositionDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.StartPosition>
                {
                    new Entities.StartPosition
                    {
                        Id = 0,
                        Position = 1,
                        StartListId = 0
                    }
                }).Once().Then
                .ReturnsLazily(
                    () => new List<Entities.StartPosition> { }
                ).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.StartPosition>>(null));

            var informationManager = new Core.BL.InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceDataDaoFake,
                raceStateDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                timeMeasurementDaoFake,
                venueDaoFake);

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            Assert.True(await raceExecutionManager.IsRaceStartable(raceId));
        }

        [Fact]
        public async Task RaceNotStartableTest1()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceDataDaoFake = A.Fake<IDataAccessObject<Entities.RaceData>>();
            var raceStateDaoFake = A.Fake<IDataAccessObject<Entities.RaceState>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var timeMeasurementDaoFake = A.Fake<IDataAccessObject<Entities.TimeMeasurement>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            int raceId = 0;
            A.CallTo(() => raceDaoFake.GetByIdAsync(A<int>._))
                .ReturnsLazily(() => new Entities.Race()
                {
                    Date = DateTime.Now.Date.AddDays(-1),
                    Id = raceId,
                    FirstStartListId = 0,
                    SecondStartListId = 1
                });
            A.CallTo(() => startPositionDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.StartPosition>
                {
                    new Entities.StartPosition
                    {
                        Id = 0,
                        Position = 1,
                        StartListId = 0
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.StartPosition>
                {
                    new Entities.StartPosition
                    {
                        Id = 1,
                        Position = 1,
                        StartListId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.StartPosition>>(null));

            var informationManager = new Core.BL.InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceDataDaoFake,
                raceStateDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                timeMeasurementDaoFake,
                venueDaoFake);

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            Assert.False(await raceExecutionManager.IsRaceStartable(raceId));
        }

        [Fact]
        public async Task RaceNotStartableTest2()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceDataDaoFake = A.Fake<IDataAccessObject<Entities.RaceData>>();
            var raceStateDaoFake = A.Fake<IDataAccessObject<Entities.RaceState>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var timeMeasurementDaoFake = A.Fake<IDataAccessObject<Entities.TimeMeasurement>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            int raceId = 0;
            A.CallTo(() => raceDaoFake.GetByIdAsync(A<int>._))
                .ReturnsLazily(() => new Entities.Race()
                {
                    Date = DateTime.Now.Date,
                    Id = raceId,
                    FirstStartListId = 0,
                    SecondStartListId = 1
                });
            A.CallTo(() => startPositionDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(
                    () => new List<Entities.StartPosition> { }
                ).Once().Then
                .ReturnsLazily(() => new List<Entities.StartPosition>
                {
                    new Entities.StartPosition
                    {
                        Id = 0,
                        Position = 1,
                        StartListId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.StartPosition>>(null));

            var informationManager = new Core.BL.InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceDataDaoFake,
                raceStateDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                timeMeasurementDaoFake,
                venueDaoFake);

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            Assert.False(await raceExecutionManager.IsRaceStartable(raceId));
        }

        [Fact]
        public async Task StartTimeTrackingOnFirstStartListTest()
        {
            var countryDaoFake = A.Fake<IDataAccessObject<Entities.Country>>();
            var raceDaoFake = A.Fake<IDataAccessObject<Entities.Race>>();
            var raceDataDaoFake = A.Fake<IDataAccessObject<Entities.RaceData>>();
            var raceStateDaoFake = A.Fake<IDataAccessObject<Entities.RaceState>>();
            var raceTypeDaoFake = A.Fake<IDataAccessObject<Entities.RaceType>>();
            var skierDaoFake = A.Fake<IDataAccessObject<Entities.Skier>>();
            var seasonDaoFake = A.Fake<IDataAccessObject<Entities.Season>>();
            var seasonPlanDaoFake = A.Fake<IDataAccessObject<Entities.SeasonPlan>>();
            var sexDaoFake = A.Fake<IDataAccessObject<Entities.Sex>>();
            var startListDaoFake = A.Fake<IDataAccessObject<Entities.StartList>>();
            var startPositionDaoFake = A.Fake<IDataAccessObject<Entities.StartPosition>>();
            var timeMeasurementDaoFake = A.Fake<IDataAccessObject<Entities.TimeMeasurement>>();
            var venueDaoFake = A.Fake<IDataAccessObject<Entities.Venue>>();

            int raceId = 0;
            A.CallTo(() => raceDaoFake.GetByIdAsync(A<int>._))
                .ReturnsLazily(() => new Entities.Race
                {
                    Date = DateTime.Now.Date,
                    FirstStartListId = 0,
                    SecondStartListId = 1,
                    Id = raceId,
                    NumberOfSensors = 5
                });

            A.CallTo(() => skierDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily(() => new Entities.Skier
                {
                    FirstName = "Manuel",
                    LastName = "Fuchs",
                    Id = 0,
                    DateOfBirth = new DateTime(1996, 12, 6),
                    IsRemoved = false
                });
            A.CallTo(() => skierDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily(() => new Entities.Skier
                {
                    FirstName = "Thomas",
                    LastName = "Halbmayr",
                    Id = 1,
                    DateOfBirth = new DateTime(1994, 11, 25),
                    IsRemoved = false
                });

            A.CallTo(() => startPositionDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.StartPosition>
                {
                    new Entities.StartPosition
                    {
                        Id = 0,
                        Position = 1,
                        StartListId = 0,
                        SkierId = 0
                    },
                    new Entities.StartPosition
                    {
                        Id = 1,
                        Position = 2,
                        StartListId = 0,
                        SkierId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.StartPosition>
                {
                    new Entities.StartPosition
                    {
                        Id = 0,
                        Position = 1,
                        StartListId = 1,
                        SkierId = 1
                    },
                    new Entities.StartPosition
                    {
                        Id = 1,
                        Position = 2,
                        StartListId = 1,
                        SkierId = 0
                    }
                }).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.StartPosition>>(null));

            A.CallTo(() => raceStateDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.RaceState>
                {
                    new Entities.RaceState
                    {
                        Id = 0,
                        Label = "Startbereit"
                    }
                });

            A.CallTo(() => raceDataDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.RaceData>
                {
                    new Entities.RaceData
                    {
                        Id = 0,
                        RaceStateId = 0,
                        SkierId = 0,
                        StartListId = 0
                    },
                    new Entities.RaceData
                    {
                        Id = 1,
                        RaceStateId = 0,
                        SkierId = 1,
                        StartListId = 0
                    },
                    new Entities.RaceData
                    {
                        Id = 2,
                        RaceStateId = 0,
                        SkierId = 0,
                        StartListId = 1
                    },
                    new Entities.RaceData
                    {
                        Id = 3,
                        RaceStateId = 0,
                        SkierId = 1,
                        StartListId = 1
                    }
                });

            var informationManager = new Core.BL.InformationManager(
                countryDaoFake,
                raceDaoFake,
                raceDataDaoFake,
                raceStateDaoFake,
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                timeMeasurementDaoFake,
                venueDaoFake);

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            var fakedRaceClock = A.Fake<Timer.IRaceClock>();
            raceExecutionManager.RaceClock = fakedRaceClock;

            var race = await informationManager
                .GetRaceByIdAsync(
                    raceId,
                    startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.None,
                    skierLoadingType: Domain.Associated<Domain.Skier>.LoadingType.None)
                .ConfigureAwait(false);

            await raceExecutionManager.StartTimeTracking(race, true, 1).ConfigureAwait(false);
        }
    }
}
