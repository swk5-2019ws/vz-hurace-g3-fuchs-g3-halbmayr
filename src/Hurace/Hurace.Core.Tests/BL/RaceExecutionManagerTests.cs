using FakeItEasy;
using Hurace.Core.DAL;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

#pragma warning disable IDE0039 // Use local function
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

            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily(() => new Entities.Venue
                {
                    Id = 0,
                    Name = "Hinterstoder"
                });
            A.CallTo(() => venueDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily(() => new Entities.Venue
                {
                    Id = 1,
                    Name = "Hinterklemm"
                });

            A.CallTo(() => raceTypeDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(0)))
                .ReturnsLazily(() => new Entities.RaceType
                {
                    Id = 0,
                    Label = "Riesentorlauf"
                });
            A.CallTo(() => raceTypeDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(1)))
                .ReturnsLazily(() => new Entities.RaceType
                {
                    Id = 1,
                    Label = "Riesenslalom"
                });

            var date = DateTime.Now.Date;
            var raceId = 0;
            A.CallTo(() => raceDaoFake.GetByIdAsync(A<int>.That.IsEqualTo(raceId)))
                .ReturnsLazily(() => new Entities.Race
                {
                    Date = date,
                    FirstStartListId = 0,
                    SecondStartListId = 1,
                    Id = raceId,
                    NumberOfSensors = 5,
                    VenueId = 0,
                    RaceTypeId = 0
                });
            A.CallTo(() => raceDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.Race>
                {
                    new Entities.Race
                    {
                        Date = date.AddDays(-2),
                        FirstStartListId = 2,
                        SecondStartListId = 3,
                        Id = 1,
                        NumberOfSensors = 5,
                        VenueId = 0,
                        RaceTypeId = 0
                    },
                    new Entities.Race
                    {
                        Date = date.AddDays(-1),
                        FirstStartListId = 4,
                        SecondStartListId = 5,
                        Id = 2,
                        NumberOfSensors = 7,
                        VenueId = 0,
                        RaceTypeId = 0
                    }
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

            var firstStartPosition = new Entities.StartPosition
            {
                Id = 0,
                Position = 1,
                StartListId = 0,
                SkierId = 0
            };
            var secondStartPosition = new Entities.StartPosition
            {
                Id = 1,
                Position = 2,
                StartListId = 0,
                SkierId = 1
            };
            var thirdStartPosition = new Entities.StartPosition
            {
                Id = 0,
                Position = 1,
                StartListId = 1,
                SkierId = 1
            };
            var fourthStartPosition = new Entities.StartPosition
            {
                Id = 1,
                Position = 2,
                StartListId = 1,
                SkierId = 0
            };
            var firstStartPositionSet = new List<Entities.StartPosition> { firstStartPosition, secondStartPosition };
            var secondStartPositionSet = new List<Entities.StartPosition> { thirdStartPosition, fourthStartPosition };
            A.CallTo(() => startPositionDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => firstStartPositionSet).Once().Then
                .ReturnsLazily(() => new List<Entities.StartPosition> { firstStartPosition }).Twice().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.StartPosition>>(null));

            A.CallTo(() => raceStateDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.RaceState>
                {
                    new Entities.RaceState
                    {
                        Id = 0,
                        Label = "Startbereit"
                    },
                    new Entities.RaceState
                    {
                        Id = 1,
                        Label = "Abgeschlossen"
                    },
                    new Entities.RaceState
                    {
                        Id = 2,
                        Label = "Laufend"
                    }
                });

            var firstRaceData = new Entities.RaceData
            {
                Id = 0,
                RaceStateId = 0,
                SkierId = 0,
                StartListId = 0
            };
            var secondRaceData = new Entities.RaceData
            {
                Id = 1,
                RaceStateId = 0,
                SkierId = 1,
                StartListId = 0
            };
            A.CallTo(() => raceDataDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.RaceData> { secondRaceData, firstRaceData }).Once().Then
                .ReturnsLazily(() => new List<Entities.RaceData> { firstRaceData }).Once().Then
                .ReturnsLazily(() => new List<Entities.RaceData>
                {
                    new Entities.RaceData
                    {
                        Id = 2,
                        RaceStateId = 1,
                        StartListId = 2,
                        SkierId = 0
                    },
                    new Entities.RaceData
                    {
                        Id = 3,
                        RaceStateId = 1,
                        StartListId = 2,
                        SkierId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.RaceData>
                {
                    new Entities.RaceData
                    {
                        Id = 4,
                        RaceStateId = 1,
                        StartListId = 3,
                        SkierId = 0
                    },
                    new Entities.RaceData
                    {
                        Id = 5,
                        RaceStateId = 1,
                        StartListId = 3,
                        SkierId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.RaceData>
                {
                    new Entities.RaceData
                    {
                        Id = 6,
                        RaceStateId = 1,
                        StartListId = 4,
                        SkierId = 0
                    },
                    new Entities.RaceData
                    {
                        Id = 7,
                        RaceStateId = 1,
                        StartListId = 4,
                        SkierId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.RaceData>
                {
                    new Entities.RaceData
                    {
                        Id = 8,
                        RaceStateId = 1,
                        StartListId = 5,
                        SkierId = 0
                    },
                    new Entities.RaceData
                    {
                        Id = 9,
                        RaceStateId = 1,
                        StartListId = 5,
                        SkierId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.RaceData>>(null));

            A.CallTo(() => timeMeasurementDaoFake.GetAllConditionalAsync(A<IQueryCondition>._))
                .ReturnsLazily(() => new List<Entities.TimeMeasurement>
                {
                    new Entities.TimeMeasurement
                    {
                        Id = 0,
                        IsValid = true,
                        Measurement = 0,
                        RaceDataId = 2,
                        SensorId = 0
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 1,
                        IsValid = true,
                        Measurement = 2000,
                        RaceDataId = 2,
                        SensorId = 1
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 2,
                        IsValid = true,
                        Measurement = 3000,
                        RaceDataId = 2,
                        SensorId = 2
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.TimeMeasurement>
                {
                    new Entities.TimeMeasurement
                    {
                        Id = 3,
                        IsValid = true,
                        Measurement = 0,
                        RaceDataId = 3,
                        SensorId = 0
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 4,
                        IsValid = true,
                        Measurement = 2000,
                        RaceDataId = 3,
                        SensorId = 1
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 5,
                        IsValid = true,
                        Measurement = 2750,
                        RaceDataId = 3,
                        SensorId = 2
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.TimeMeasurement>
                {
                    new Entities.TimeMeasurement
                    {
                        Id = 6,
                        IsValid = true,
                        Measurement = 0,
                        RaceDataId = 4,
                        SensorId = 0
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 7,
                        IsValid = true,
                        Measurement = 1900,
                        RaceDataId = 4,
                        SensorId = 1
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 8,
                        IsValid = true,
                        Measurement = 3250,
                        RaceDataId = 4,
                        SensorId = 2
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.TimeMeasurement>
                {
                    new Entities.TimeMeasurement
                    {
                        Id = 9,
                        IsValid = true,
                        Measurement = 0,
                        RaceDataId = 5,
                        SensorId = 0
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 10,
                        IsValid = true,
                        Measurement = 2100,
                        RaceDataId = 5,
                        SensorId = 1
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 11,
                        IsValid = true,
                        Measurement = 3000,
                        RaceDataId = 5,
                        SensorId = 2
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.TimeMeasurement>
                {
                    new Entities.TimeMeasurement
                    {
                        Id = 12,
                        IsValid = true,
                        Measurement = 0,
                        RaceDataId = 6,
                        SensorId = 0
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 13,
                        IsValid = true,
                        Measurement = 2050,
                        RaceDataId = 6,
                        SensorId = 1
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 14,
                        IsValid = true,
                        Measurement = 3100,
                        RaceDataId = 6,
                        SensorId = 2
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.TimeMeasurement>
                {
                    new Entities.TimeMeasurement
                    {
                        Id = 15,
                        IsValid = true,
                        Measurement = 0,
                        RaceDataId = 7,
                        SensorId = 0
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 16,
                        IsValid = true,
                        Measurement = 1950,
                        RaceDataId = 7,
                        SensorId = 1
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 17,
                        IsValid = true,
                        Measurement = 3050,
                        RaceDataId = 7,
                        SensorId = 2
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.TimeMeasurement>
                {
                    new Entities.TimeMeasurement
                    {
                        Id = 18,
                        IsValid = true,
                        Measurement = 0,
                        RaceDataId = 8,
                        SensorId = 0
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 19,
                        IsValid = true,
                        Measurement = 1900,
                        RaceDataId = 8,
                        SensorId = 1
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 20,
                        IsValid = true,
                        Measurement = 2950,
                        RaceDataId = 8,
                        SensorId = 2
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.TimeMeasurement>
                {
                    new Entities.TimeMeasurement
                    {
                        Id = 21,
                        IsValid = true,
                        Measurement = 0,
                        RaceDataId = 9,
                        SensorId = 0
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 22,
                        IsValid = true,
                        Measurement = 1910,
                        RaceDataId = 9,
                        SensorId = 1
                    },
                    new Entities.TimeMeasurement
                    {
                        Id = 23,
                        IsValid = true,
                        Measurement = 3010,
                        RaceDataId = 9,
                        SensorId = 2
                    }
                }).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.TimeMeasurement>>(null));

            Predicate<object> raceDataUpdateFilter =
                o =>
                {
                    var raceStateId = (int)o.GetType().GetProperty("RaceStateId").GetValue(o);
                    return raceStateId == 2;
                };

            object raceDataChanges = null;
            IQueryCondition raceDataUpdateCondition = null;

            Action<object, IQueryCondition> raceDataUpdater =
                (objectContainingChanges, condition) =>
                {
                    raceDataChanges = objectContainingChanges;
                    raceDataUpdateCondition = condition;
                };

            A.CallTo(() => raceDataDaoFake.UpdateAsync(
                    A<object>.That.Matches(o => raceDataUpdateFilter(o)),
                    A<IQueryCondition>._))
                .Invokes(raceDataUpdater)
                .ReturnsLazily(() => 1);

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
            raceExecutionManager.OnTimeMeasured +=
                (race, skier, measurement) =>
                {

                };

            var fakedRaceClock = A.Fake<Timer.IRaceClock>();
            raceExecutionManager.RaceClock = fakedRaceClock;

            var race = await informationManager
                .GetRaceByIdAsync(
                    raceId,
                    startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.None,
                    skierLoadingType: Domain.Associated<Domain.Skier>.LoadingType.None)
                .ConfigureAwait(false);

            await raceExecutionManager.StartTimeTracking(race, true, 1).ConfigureAwait(false);

            fakedRaceClock.TimingTriggered += Raise.FreeForm.With(0, DateTime.Now);

            var raceDataUpdateConditionStringBuilder = new StringBuilder();
            var raceDataUpdateConditionParameter = new List<QueryParameter>();
            raceDataUpdateCondition.AppendTo(raceDataUpdateConditionStringBuilder, raceDataUpdateConditionParameter);

            Assert.Equal("[Id] = @Id0", raceDataUpdateConditionStringBuilder.ToString());
            Assert.Equal("Id0", raceDataUpdateConditionParameter.First().ParameterName);
            Assert.Equal(0, (int)raceDataUpdateConditionParameter.First().Value);
        }

        [Fact]
        public async Task StartTimeTrackingOnFirstStartListFailingTest()
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
                        Id = 1,
                        RaceStateId = 0,
                        SkierId = 1,
                        StartListId = 0
                    },
                    new Entities.RaceData
                    {
                        Id = 0,
                        RaceStateId = 0,
                        SkierId = 0,
                        StartListId = 0
                    }
                }).Once().Then
                .ReturnsLazily(() => new List<Entities.RaceData>
                {
                    new Entities.RaceData
                    {
                        Id = 3,
                        RaceStateId = 0,
                        SkierId = 1,
                        StartListId = 1
                    },
                    new Entities.RaceData
                    {
                        Id = 2,
                        RaceStateId = 0,
                        SkierId = 0,
                        StartListId = 1
                    }
                }).Once().Then
                .ReturnsLazily(() => Task.FromResult<IEnumerable<Entities.RaceData>>(null));

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

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await raceExecutionManager.StartTimeTracking(race, true, 2).ConfigureAwait(false));
        }
    }
}
