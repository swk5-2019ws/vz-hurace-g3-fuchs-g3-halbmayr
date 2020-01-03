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
        public async Task RaceIsStartableTest()
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
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            Assert.True(await raceExecutionManager.IsRaceStartable(raceId));
        }

        [Fact]
        public async Task RaceNotStartableTest1()
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
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            Assert.False(await raceExecutionManager.IsRaceStartable(raceId));
        }

        [Fact]
        public async Task RaceNotStartableTest2()
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
                .ReturnsLazily(
                    () => new List<Entities.StartPosition> { }
                ).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.StartPosition>>(null));

            var informationManager = new Core.BL.InformationManager(
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

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            Assert.False(await raceExecutionManager.IsRaceStartable(raceId));
        }

        [Fact]
        public async Task RaceNotStartableTest3()
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
                raceTypeDaoFake,
                seasonDaoFake,
                seasonPlanDaoFake,
                sexDaoFake,
                skierDaoFake,
                startListDaoFake,
                startPositionDaoFake,
                venueDaoFake);

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);

            Assert.False(await raceExecutionManager.IsRaceStartable(raceId));
        }

        [Fact]
        public void TimeTrackingTest()
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

            var informationManager = new Core.BL.InformationManager(
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

            var raceExecutionManager = new Core.BL.RaceExecutionManager(informationManager);


        }
    }
}
