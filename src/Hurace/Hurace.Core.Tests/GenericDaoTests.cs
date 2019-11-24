using Hurace.Core.Dal.AdoPersistence;
using Hurace.Core.Db.Connection;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CA5394 // Do not use insecure randomness
#pragma warning disable IDE0045 // Convert to conditional expression
namespace Hurace.Core.Tests
{
    public class GenericDaoTests : IDisposable
    {
        #region Private fields

        private bool disposed = false;

        private readonly TransactionScope transactionScope;
        private readonly Random rnd = new Random();

        #endregion

        #region Transaction Setup Boilerplate Code


        public GenericDaoTests()
        {
            transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                transactionScope.Dispose();
            }

            disposed = true;
        }

        #endregion

        [Theory]
        [InlineData(typeof(Domain.Country), 52)]
        [InlineData(typeof(Domain.Race), 37)]
        [InlineData(typeof(Domain.RaceData), 3710)]
        [InlineData(typeof(Domain.RaceState), 5)]
        [InlineData(typeof(Domain.RaceType), 2)]
        [InlineData(typeof(Domain.Season), 2)]
        [InlineData(typeof(Domain.SeasonPlan), 62)]
        [InlineData(typeof(Domain.Sex), 2)]
        [InlineData(typeof(Domain.Skier), 521)]
        [InlineData(typeof(Domain.StartList), 74)]
        [InlineData(typeof(Domain.StartPosition), 3710)]
        [InlineData(typeof(Domain.TimeMeasurement), 18745)]
        [InlineData(typeof(Domain.Venue), 31)]
        public async Task GetAllUnconditionalTest(Type domainType, int expectedResultCount)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoGetAllMethod = this.GetAdoDaoMethodInfo(domainType, "GetAllConditionalAsync");

            var emptyParameterList = new object[] { null };

            var allDomainObjectsDynamic = await (dynamic)daoGetAllMethod.Invoke(adoDaoInstance, emptyParameterList);
            var allDomainObjects = (IEnumerable<object>)allDomainObjectsDynamic;

            Assert.Equal(expectedResultCount, allDomainObjects.Count());

            var expectedDomainObject = this.GenerateTestableCompareObject(domainType);
            var actualDomainObject = allDomainObjects.Skip(expectedDomainObject.Id).First();

            foreach (var currentProperty in expectedDomainObject.GetType().GetProperties())
            {
                Assert.Equal(currentProperty.GetValue(expectedDomainObject), currentProperty.GetValue(actualDomainObject));
            }
        }

        [Theory]
        [InlineData(typeof(Domain.Country))]
        [InlineData(typeof(Domain.Race))]
        [InlineData(typeof(Domain.RaceData))]
        [InlineData(typeof(Domain.RaceState))]
        [InlineData(typeof(Domain.RaceType))]
        [InlineData(typeof(Domain.Season))]
        [InlineData(typeof(Domain.SeasonPlan))]
        [InlineData(typeof(Domain.Sex))]
        [InlineData(typeof(Domain.Skier))]
        [InlineData(typeof(Domain.StartList))]
        [InlineData(typeof(Domain.StartPosition))]
        [InlineData(typeof(Domain.TimeMeasurement))]
        [InlineData(typeof(Domain.Venue))]
        public async Task GetAllConditionalTest(Type domainType)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoGetAllMethod = this.GetAdoDaoMethodInfo(domainType, "GetAllConditionalAsync");

            var tmp = new StringBuilder();
            var emptyParameterList = new object[]
            {
                GenerateQueryConditionsForRuntimeType(domainType)
            };

            var allDomainObjectsDynamic = await (dynamic)daoGetAllMethod.Invoke(adoDaoInstance, emptyParameterList);
            var allDomainObjects = (IEnumerable<object>)allDomainObjectsDynamic;

            var expectedDomainObject = this.GenerateTestableCompareObject(domainType);
            Assert.Single(allDomainObjects);

            var actualDomainObject = allDomainObjects.First();

            foreach (var currentProperty in expectedDomainObject.GetType().GetProperties())
            {
                Assert.Equal(currentProperty.GetValue(expectedDomainObject), currentProperty.GetValue(actualDomainObject));
            }
        }

        [Theory]
        [InlineData(typeof(Domain.Country))]
        [InlineData(typeof(Domain.Race))]
        [InlineData(typeof(Domain.RaceData))]
        [InlineData(typeof(Domain.RaceState))]
        [InlineData(typeof(Domain.RaceType))]
        [InlineData(typeof(Domain.Season))]
        [InlineData(typeof(Domain.SeasonPlan))]
        [InlineData(typeof(Domain.Sex))]
        [InlineData(typeof(Domain.Skier))]
        [InlineData(typeof(Domain.StartList))]
        [InlineData(typeof(Domain.StartPosition))]
        [InlineData(typeof(Domain.TimeMeasurement))]
        [InlineData(typeof(Domain.Venue))]
        public async Task GetByIdTest(Type domainType)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoGetByIdMethod = this.GetAdoDaoMethodInfo(domainType, "GetByIdAsync");

            var expectedDomainObject = this.GenerateTestableCompareObject(domainType);

            var emptyParameterList = new object[] { expectedDomainObject.Id };

            var actualDomainObjectDynamic = await (dynamic)daoGetByIdMethod.Invoke(adoDaoInstance, emptyParameterList);
            var actualDomainObject = (object)actualDomainObjectDynamic;

            foreach (var currentProperty in expectedDomainObject.GetType().GetProperties())
            {
                Assert.Equal(currentProperty.GetValue(expectedDomainObject), currentProperty.GetValue(actualDomainObject));
            }
        }

        [Theory]
        [InlineData(typeof(Domain.Country), 50000)]
        [InlineData(typeof(Domain.Race), 50000)]
        [InlineData(typeof(Domain.RaceData), 50000)]
        [InlineData(typeof(Domain.RaceState), 50000)]
        [InlineData(typeof(Domain.RaceType), 50000)]
        [InlineData(typeof(Domain.Season), 50000)]
        [InlineData(typeof(Domain.SeasonPlan), 50000)]
        [InlineData(typeof(Domain.Sex), 50000)]
        [InlineData(typeof(Domain.Skier), 50000)]
        [InlineData(typeof(Domain.StartList), 50000)]
        [InlineData(typeof(Domain.StartPosition), 50000)]
        [InlineData(typeof(Domain.TimeMeasurement), 50000)]
        [InlineData(typeof(Domain.Venue), 50000)]
        public async Task GetByIdWithNonExistingId(Type domainType, int queryId)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoGetByIdMethod = this.GetAdoDaoMethodInfo(domainType, "GetByIdAsync");

            var emptyParameterList = new object[] { queryId };

            var actualDomainObjectDynamic = await (dynamic)daoGetByIdMethod.Invoke(adoDaoInstance, emptyParameterList);
            var actualDomainObject = (object)actualDomainObjectDynamic;

            Assert.Null(actualDomainObject);
        }

        [Theory]
        [InlineData(typeof(Domain.Country))]
        [InlineData(typeof(Domain.RaceState))]
        [InlineData(typeof(Domain.RaceType))]
        [InlineData(typeof(Domain.Season))]
        [InlineData(typeof(Domain.Sex))]
        [InlineData(typeof(Domain.Venue))]
        public async Task CreateTest(Type domainType)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoCreateMethod = this.GetAdoDaoMethodInfo(domainType, "CreateAsync");

            var expectedDomainObject = domainType.GetConstructor(Array.Empty<Type>())
                .Invoke(Array.Empty<object>());
            this.AlterDomainObjectRandomly(expectedDomainObject);

            var parameterList = new object[] { expectedDomainObject };

            var actualDomainObjectDynamic = await (dynamic)daoCreateMethod.Invoke(adoDaoInstance, parameterList);
            var actualDomainObject = (object)actualDomainObjectDynamic;

            foreach (var currentProperty in domainType.GetProperties())
            {
                if (currentProperty.Name == "Id")
                {
                    Assert.NotEqual(0, (int)currentProperty.GetValue(actualDomainObject));
                }
                else
                {
                    Assert.Equal(
                        currentProperty.GetValue(expectedDomainObject),
                        currentProperty.GetValue(actualDomainObject));
                }
            }
        }

        [Fact]
        public async Task CreateWithSqlInjectionTest()
        {
            var connectionFactory = new DefaultConnectionFactory();
            var seasonDao = new GenericDao<Domain.Season>(connectionFactory);

            var expectedDomainObject = new Domain.Season()
            {
                Name = "'DELETE FROM [Hurace].[Sex];--",
                StartDate = DateTime.Now.AddDays(-365).Date,
                EndDate = DateTime.Now.Date
            };

            var actualDomainObject = await seasonDao.CreateAsync(expectedDomainObject);

            Assert.Equal(expectedDomainObject.Name, actualDomainObject.Name);
            Assert.Equal(expectedDomainObject.StartDate, actualDomainObject.StartDate);
            Assert.Equal(expectedDomainObject.EndDate, actualDomainObject.EndDate);

            var sexDao = new GenericDao<Domain.Sex>(connectionFactory);
            Assert.Equal(2, (await sexDao.GetAllConditionalAsync()).Count());
        }

        [Theory]
        [InlineData(typeof(Domain.Country))]
        [InlineData(typeof(Domain.Race))]
        [InlineData(typeof(Domain.RaceData))]
        [InlineData(typeof(Domain.RaceState))]
        [InlineData(typeof(Domain.RaceType))]
        [InlineData(typeof(Domain.Season))]
        [InlineData(typeof(Domain.SeasonPlan))]
        [InlineData(typeof(Domain.Sex))]
        [InlineData(typeof(Domain.Skier))]
        [InlineData(typeof(Domain.StartPosition))]
        [InlineData(typeof(Domain.TimeMeasurement))]
        [InlineData(typeof(Domain.Venue))]
        public async Task UpdateExistingDomainObjects(Type domainType)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoUpdateMethod = this.GetAdoDaoMethodInfo(domainType, "UpdateAsync");

            var expectedUpdatedDomainObject = this.GenerateTestableCompareObject(domainType);
            this.AlterDomainObjectRandomly(expectedUpdatedDomainObject);

            var updateParameterList = new object[] { expectedUpdatedDomainObject };
            bool success = (bool)await (dynamic)daoUpdateMethod.Invoke(adoDaoInstance, updateParameterList);

            Assert.True(success);

            var getByIdAsyncMethod = this.GetAdoDaoMethodInfo(domainType, "GetByIdAsync");
            var getByIdParamList = new object[] { expectedUpdatedDomainObject.Id };

            var actualUpdatedDomainObject = await (dynamic)getByIdAsyncMethod.Invoke(adoDaoInstance, getByIdParamList);

            foreach (var currentProperty in domainType.GetProperties())
            {
                Assert.Equal(
                    currentProperty.GetValue(expectedUpdatedDomainObject),
                    currentProperty.GetValue(actualUpdatedDomainObject));
            }
        }

        [Theory]
        [InlineData(typeof(Domain.Country))]
        [InlineData(typeof(Domain.Race))]
        [InlineData(typeof(Domain.RaceData))]
        [InlineData(typeof(Domain.RaceState))]
        [InlineData(typeof(Domain.RaceType))]
        [InlineData(typeof(Domain.Season))]
        [InlineData(typeof(Domain.SeasonPlan))]
        [InlineData(typeof(Domain.Sex))]
        [InlineData(typeof(Domain.Skier))]
        [InlineData(typeof(Domain.StartPosition))]
        [InlineData(typeof(Domain.TimeMeasurement))]
        [InlineData(typeof(Domain.Venue))]
        public async Task UpdateNotExistentDomainObjects(Type domainType)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoUpdateMethod = this.GetAdoDaoMethodInfo(domainType, "UpdateAsync");

            var expectedUpdatedDomainObject = this.GenerateTestableCompareObject(domainType);
            expectedUpdatedDomainObject.Id = 50000;

            var updateParameterList = new object[] { expectedUpdatedDomainObject };
            bool success = (bool)await (dynamic)daoUpdateMethod.Invoke(adoDaoInstance, updateParameterList);

            Assert.False(success);
        }

        [Theory]
        [InlineData(typeof(Domain.Country), false)]
        [InlineData(typeof(Domain.Race), true)]
        [InlineData(typeof(Domain.RaceData), false)]
        [InlineData(typeof(Domain.RaceState), false)]
        [InlineData(typeof(Domain.RaceType), false)]
        [InlineData(typeof(Domain.Season), false)]
        [InlineData(typeof(Domain.SeasonPlan), true)]
        [InlineData(typeof(Domain.Sex), false)]
        [InlineData(typeof(Domain.Skier), false)]
        [InlineData(typeof(Domain.StartList), false)]
        [InlineData(typeof(Domain.StartPosition), true)]
        [InlineData(typeof(Domain.TimeMeasurement), true)]
        [InlineData(typeof(Domain.Venue), false)]
        public async Task DeleteByIdExistingIdTest(Type domainType, bool expectedSuccess)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoDeleteByIdMethod = this.GetAdoDaoMethodInfo(domainType, "DeleteByIdAsync");

            int deleteIdParam = 0;
            var paramList = new object[] { deleteIdParam };
            var actualSuccess = (bool)await (dynamic)daoDeleteByIdMethod.Invoke(adoDaoInstance, paramList);

            Assert.Equal(expectedSuccess, actualSuccess);

            var daoGetByIdMethod = this.GetAdoDaoMethodInfo(domainType, "GetByIdAsync");
            var expectedNotFoundDomainObject =
                (Domain.DomainObjectBase)await
                    (dynamic)daoGetByIdMethod.Invoke(adoDaoInstance, paramList);

            if (expectedSuccess)
            {
                Assert.Null(expectedNotFoundDomainObject);
            }
            else
            {
                Assert.NotNull(expectedNotFoundDomainObject);
                Assert.Equal(deleteIdParam, expectedNotFoundDomainObject.Id);
            }
        }

        [Fact]
        public void GenerateDaoWithInvalidConnectionFactory()
        {
            try
            {
                var skierDao = new GenericDao<Domain.Skier>(null);
                Assert.False(true);
            }
            catch (ArgumentNullException)
            {
                Assert.True(true);
            }
        }

        [Theory]
        [InlineData(typeof(Domain.Country))]
        [InlineData(typeof(Domain.Race))]
        [InlineData(typeof(Domain.RaceData))]
        [InlineData(typeof(Domain.RaceState))]
        [InlineData(typeof(Domain.RaceType))]
        [InlineData(typeof(Domain.Season))]
        [InlineData(typeof(Domain.SeasonPlan))]
        [InlineData(typeof(Domain.Sex))]
        [InlineData(typeof(Domain.Skier))]
        [InlineData(typeof(Domain.StartList))]
        [InlineData(typeof(Domain.StartPosition))]
        [InlineData(typeof(Domain.TimeMeasurement))]
        [InlineData(typeof(Domain.Venue))]
        public async Task DeleteByNotExistingIdTest(Type domainType)
        {
            var adoDaoInstance = this.GetAdoDaoInstance(domainType);
            var daoDeleteByIdMethod = this.GetAdoDaoMethodInfo(domainType, "DeleteByIdAsync");

            int deleteIdParam = 50000;
            var paramList = new object[] { deleteIdParam };
            var success = (bool)await (dynamic)daoDeleteByIdMethod.Invoke(adoDaoInstance, paramList);

            Assert.False(success);
        }

        #region Helper Methods

        private object GetAdoDaoInstance(Type domainType)
        {
            var adoDao = typeof(GenericDao<>)
                .MakeGenericType(domainType);

            var constructorParameterTypeList = new Type[] { typeof(IConnectionFactory) };
            var constructorParameterList = new object[] { new DefaultConnectionFactory() };

            var adoDaoInstance = adoDao.GetConstructor(constructorParameterTypeList)
                .Invoke(constructorParameterList);

            return adoDaoInstance;
        }

        private MethodInfo GetAdoDaoMethodInfo(Type domainType, string methodName)
        {
            var adoDao = typeof(GenericDao<>)
                .MakeGenericType(domainType);

            var requestedMethod = adoDao.GetMethods()
                .FirstOrDefault(m => m.Name == methodName);

            return requestedMethod;
        }

        private IQueryCondition GenerateQueryConditionsForRuntimeType(Type currentDomainType)
        {
            IQueryCondition condition = null;

            var testableDomainObject = this.GenerateTestableCompareObject(currentDomainType);

            foreach (var currentProperty in currentDomainType.GetProperties())
            {
                var newQueryCondition = new QueryCondition()
                {
                    ColumnToCheck = currentProperty.Name,
                    CompareValue = currentProperty.GetValue(testableDomainObject),
                    ConditionType = QueryCondition.Type.Equals
                };

                if (condition == null)
                {
                    condition = newQueryCondition;
                }
                else
                {
                    condition = new QueryConditionCombination()
                    {
                        CombinationType = QueryConditionCombination.Type.AND,
                        FirstCondition = condition,
                        SecondCondition = newQueryCondition
                    };
                }
            }

            return condition;
        }

        private Domain.DomainObjectBase GenerateTestableCompareObject(Type currentDomainType)
        {
            Domain.DomainObjectBase testObject;

            switch (currentDomainType.Name)
            {
                case nameof(Domain.Country):
                    testObject = new Domain.Country()
                    {
                        Id = 5,
                        Name = "SUI"
                    };
                    return testObject;
                case nameof(Domain.Race):
                    testObject = new Domain.Race()
                    {
                        Id = 13,
                        RaceTypeId = 1,
                        FirstStartListId = 26,
                        SecondStartListId = 27,
                        NumberOfSensors = 6,
                        Description = "Norwegian legend Sondre Norheim first began the trend " +
                                      "of skis with curved sides, bindings with stiff heel bands " +
                                      "made of willow, and the slalom turn style.",
                        VenueId = 12,
                        Date = new DateTime(2017, 12, 28)
                    };
                    return testObject;
                case nameof(Domain.RaceData):
                    testObject = new Domain.RaceData()
                    {
                        Id = 311,
                        StartListId = 6,
                        SkierId = 164,
                        RaceStateId = 1
                    };
                    return testObject;
                case nameof(Domain.RaceState):
                    testObject = new Domain.RaceState()
                    {
                        Id = 2,
                        Label = "NichtAbgeschlossen"
                    };
                    return testObject;
                case nameof(Domain.RaceType):
                    testObject = new Domain.RaceType()
                    {
                        Id = 0,
                        Label = "Riesentorlauf"
                    };
                    return testObject;
                case nameof(Domain.Season):
                    testObject = new Domain.Season()
                    {
                        Id = 1,
                        Name = "Jährliche Saison 2018",
                        StartDate = new DateTime(2018, 1, 1),
                        EndDate = new DateTime(2018, 12, 31)
                    };
                    return testObject;
                case nameof(Domain.SeasonPlan):
                    testObject = new Domain.SeasonPlan()
                    {
                        Id = 53,
                        VenueId = 22,
                        SeasonId = 1
                    };
                    return testObject;
                case nameof(Domain.Sex):
                    testObject = new Domain.Sex()
                    {
                        Id = 0,
                        Label = "Weiblich"
                    };
                    return testObject;
                case nameof(Domain.Skier):
                    testObject = new Domain.Skier()
                    {
                        Id = 120,
                        FirstName = "Marcel",
                        LastName = "Hirscher",
                        DateOfBirth = new DateTime(1989, 3, 2),
                        CountryId = 3,
                        SexId = 1,
                        ImageUrl = "https://data.fis-ski.com/general/load-competitor-picture/106332.html",
                        IsRemoved = false
                    };
                    return testObject;
                case nameof(Domain.StartList):
                    testObject = new Domain.StartList()
                    {
                        Id = 41
                    };
                    return testObject;
                case nameof(Domain.StartPosition):
                    testObject = new Domain.StartPosition()
                    {
                        Id = 53,
                        SkierId = 0,
                        StartListId = 0,
                        Position = 54
                    };
                    return testObject;
                case nameof(Domain.TimeMeasurement):
                    testObject = new Domain.TimeMeasurement()
                    {
                        Id = 226,
                        SensorId = 1,
                        Measurement = 59000,
                        RaceDataId = 45
                    };
                    return testObject;
                case nameof(Domain.Venue):
                    testObject = new Domain.Venue()
                    {
                        Id = 20,
                        Name = "Kitzbuehel",
                        CountryId = 3
                    };
                    return testObject;
                default:
                    throw new ArgumentException($"DomainType {nameof(currentDomainType)} is not recognized");
            }
        }

        private void AlterDomainObjectRandomly(object domainObject)
        {
            string defaultString = "test";
            foreach (var currentProperty in domainObject.GetType().GetProperties())
            {
                if (currentProperty.PropertyType == typeof(bool))
                {
                    currentProperty.SetValue(domainObject, rnd.Next(0, 2) == 1);
                }
                else if (currentProperty.PropertyType == typeof(int)
                    && !currentProperty.Name.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (currentProperty.Name == "NumberOfSensors")
                    {
                        currentProperty.SetValue(domainObject, rnd.Next(1, int.MaxValue));
                    }
                    else
                    {
                        currentProperty.SetValue(domainObject, rnd.Next(int.MinValue, int.MaxValue));
                    }
                }
                else if (currentProperty.PropertyType == typeof(string))
                {
                    currentProperty.SetValue(domainObject, defaultString);
                }
                else if (currentProperty.PropertyType == typeof(DateTime))
                {
                    currentProperty.SetValue(domainObject, DateTime.Now.Date);
                }
            }
        }

        #endregion
    }
}
