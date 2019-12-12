using Hurace.Core.DAL.AdoPersistence;
using Hurace.Core.Db.Connection;
using Hurace.Core.Db.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CA5394 // Do not use insecure randomness
#pragma warning disable IDE0045 // Convert to conditional expression
#pragma warning disable IDE0046 // Convert to conditional expression
namespace Hurace.Core.Tests.DAL
{
    public class GenericDaoTests : IDisposable
    {
        #region Private fields

        private bool disposed = false;

        private readonly TransactionScope transactionScope;

        private static readonly Random rnd = new Random();

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

        [Fact]
        public void InitializeNewDaoWithInvalidConnectionFactoryTest()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericDao<Entities.Skier>(null));
        }

        [Fact]
        public void CreateWithInvalidNewObject()
        {
            var skierDao = new GenericDao<Entities.Skier>(new DefaultConnectionFactory());
            Assert.ThrowsAsync<ArgumentNullException>(() => skierDao.CreateAsync(null));
        }

        [Fact]
        public void UpdateWithInvalidNewObject()
        {
            var skierDao = new GenericDao<Entities.Skier>(new DefaultConnectionFactory());
            Assert.ThrowsAsync<ArgumentNullException>(() => skierDao.UpdateAsync(null));
        }

        [Theory]
        [InlineData(typeof(Entities.Country), 52)]
        [InlineData(typeof(Entities.Race), 37)]
        [InlineData(typeof(Entities.RaceData), 3710)]
        [InlineData(typeof(Entities.RaceState), 5)]
        [InlineData(typeof(Entities.RaceType), 2)]
        [InlineData(typeof(Entities.Season), 2)]
        [InlineData(typeof(Entities.SeasonPlan), 62)]
        [InlineData(typeof(Entities.Sex), 2)]
        [InlineData(typeof(Entities.Skier), 521)]
        [InlineData(typeof(Entities.StartList), 74)]
        [InlineData(typeof(Entities.StartPosition), 3710)]
        [InlineData(typeof(Entities.TimeMeasurement), 18745)]
        [InlineData(typeof(Entities.Venue), 31)]
        public async Task GetAllUnconditionalTests(Type domainType, int expectedResultCount)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoGetAllMethod = GetAdoDaoMethodInfo(domainType, "GetAllConditionalAsync");

            var emptyParameterList = new object[] { null };

            var allDomainObjectsDynamic = await (dynamic)daoGetAllMethod.Invoke(adoDaoInstance, emptyParameterList);
            var allDomainObjects = (IEnumerable<object>)allDomainObjectsDynamic;

            Assert.Equal(expectedResultCount, allDomainObjects.Count());

            var expectedDomainObject = GenerateTestableCompareObject(domainType);
            var actualDomainObject = allDomainObjects.Skip(expectedDomainObject.Id).First();

            foreach (var currentProperty in expectedDomainObject.GetType().GetProperties())
            {
                Assert.Equal(currentProperty.GetValue(expectedDomainObject), currentProperty.GetValue(actualDomainObject));
            }
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task GetAllConditionalTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoGetAllMethod = GetAdoDaoMethodInfo(domainType, "GetAllConditionalAsync");

            var emptyParameterList = new object[]
            {
                GenerateQueryConditionsForRuntimeType(domainType)
            };

            var allDomainObjectsDynamic = await (dynamic)daoGetAllMethod.Invoke(adoDaoInstance, emptyParameterList);
            var allDomainObjects = (IEnumerable<object>)allDomainObjectsDynamic;

            var expectedDomainObject = GenerateTestableCompareObject(domainType);
            Assert.Single(allDomainObjects);

            var actualDomainObject = allDomainObjects.First();

            foreach (var currentProperty in expectedDomainObject.GetType().GetProperties())
            {
                Assert.Equal(currentProperty.GetValue(expectedDomainObject), currentProperty.GetValue(actualDomainObject));
            }
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task GetByIdTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoGetByIdMethod = GetAdoDaoMethodInfo(domainType, "GetByIdAsync");

            var expectedDomainObject = GenerateTestableCompareObject(domainType);

            var emptyParameterList = new object[] { expectedDomainObject.Id };

            var actualDomainObjectDynamic = await (dynamic)daoGetByIdMethod.Invoke(adoDaoInstance, emptyParameterList);
            var actualDomainObject = (object)actualDomainObjectDynamic;

            foreach (var currentProperty in expectedDomainObject.GetType().GetProperties())
            {
                Assert.Equal(currentProperty.GetValue(expectedDomainObject), currentProperty.GetValue(actualDomainObject));
            }
        }

        [Theory]
        [InlineData(typeof(Entities.Country), 50000)]
        [InlineData(typeof(Entities.Race), 50000)]
        [InlineData(typeof(Entities.RaceData), 50000)]
        [InlineData(typeof(Entities.RaceState), 50000)]
        [InlineData(typeof(Entities.RaceType), 50000)]
        [InlineData(typeof(Entities.Season), 50000)]
        [InlineData(typeof(Entities.SeasonPlan), 50000)]
        [InlineData(typeof(Entities.Sex), 50000)]
        [InlineData(typeof(Entities.Skier), 50000)]
        [InlineData(typeof(Entities.StartList), 50000)]
        [InlineData(typeof(Entities.StartPosition), 50000)]
        [InlineData(typeof(Entities.TimeMeasurement), 50000)]
        [InlineData(typeof(Entities.Venue), 50000)]
        public async Task GetByIdWithNonExistingId(Type domainType, int queryId)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoGetByIdMethod = GetAdoDaoMethodInfo(domainType, "GetByIdAsync");

            var emptyParameterList = new object[] { queryId };

            var actualDomainObjectDynamic = await (dynamic)daoGetByIdMethod.Invoke(adoDaoInstance, emptyParameterList);
            var actualDomainObject = (object)actualDomainObjectDynamic;

            Assert.Null(actualDomainObject);
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Venue))]
        public async Task CreateTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoCreateMethod = GetAdoDaoMethodInfo(domainType, "CreateAsync");

            var expectedDomainObject = domainType.GetConstructor(Array.Empty<Type>())
                .Invoke(Array.Empty<object>());
            AlterDomainObjectRandomly(expectedDomainObject);

            var parameterList = new object[] { expectedDomainObject };

            var actualDomainObjectIdDynamic = await (dynamic)daoCreateMethod.Invoke(adoDaoInstance, parameterList);
            var actualDomainObjectId = (int)actualDomainObjectIdDynamic;

            var daoGetByIdMethod = GetAdoDaoMethodInfo(domainType, "GetByIdAsync");

            parameterList = new object[] { actualDomainObjectId };

            var actualDomainObject = await (dynamic)daoGetByIdMethod.Invoke(adoDaoInstance, parameterList);

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
            var seasonDao = new GenericDao<Entities.Season>(connectionFactory);

            var expectedDomainObject = new Entities.Season()
            {
                Name = "'DELETE FROM [Hurace].[Sex];--",
                StartDate = DateTime.Now.AddDays(-365).Date,
                EndDate = DateTime.Now.Date
            };

            var actualDomainObjectId = await seasonDao.CreateAsync(expectedDomainObject);
            var actualDomainObject = await seasonDao.GetByIdAsync(actualDomainObjectId);

            Assert.Equal(expectedDomainObject.Name, actualDomainObject.Name);
            Assert.Equal(expectedDomainObject.StartDate, actualDomainObject.StartDate);
            Assert.Equal(expectedDomainObject.EndDate, actualDomainObject.EndDate);

            var sexDao = new GenericDao<Entities.Sex>(connectionFactory);
            Assert.Equal(2, (await sexDao.GetAllConditionalAsync()).Count());
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task UpdateExistingDomainObjectTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoUpdateMethod = GetAdoDaoMethodInfo(domainType, "UpdateAsync", new Type[] { domainType });

            var expectedUpdatedDomainObject = GenerateTestableCompareObject(domainType);
            AlterDomainObjectRandomly(expectedUpdatedDomainObject);

            var updateParameterList = new object[] { expectedUpdatedDomainObject };
            bool success = (bool)await (dynamic)daoUpdateMethod.Invoke(adoDaoInstance, updateParameterList);

            Assert.True(success);

            var getByIdAsyncMethod = GetAdoDaoMethodInfo(domainType, "GetByIdAsync");
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
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task UpdateNotExistentDomainObjectTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoUpdateMethod = GetAdoDaoMethodInfo(domainType, "UpdateAsync");

            var expectedUpdatedDomainObject = GenerateTestableCompareObject(domainType);
            expectedUpdatedDomainObject.Id = 50000;

            var updateParameterList = new object[] { expectedUpdatedDomainObject };
            bool success = (bool)await (dynamic)daoUpdateMethod.Invoke(adoDaoInstance, updateParameterList);

            Assert.False(success);
        }

        [Fact]
        public async Task UpdateMultipleDomainObjectsTests()
        {
            var raceDao = new GenericDao<Entities.Race>(new DefaultConnectionFactory());

            var comparisonRaceIds = new int[] { 10, 20, 30 };

            var updateCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.Or,
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.Equals, comparisonRaceIds[0]),
                    () => new QueryConditionBuilder()
                        .DeclareConditionNode(
                            QueryConditionNodeType.Or,
                            () => new QueryConditionBuilder()
                                .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.Equals, comparisonRaceIds[1]),
                            () => new QueryConditionBuilder()
                                .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.Equals, comparisonRaceIds[2])))
                .Build();

            var expectedNumberOfSensors = 10;
            var expectedDate = DateTime.Now.Date;

            foreach (var raceId in comparisonRaceIds)
            {
                var raceEntity = await raceDao.GetByIdAsync(raceId);
                Assert.NotEqual(expectedNumberOfSensors, raceEntity.NumberOfSensors);
                Assert.NotEqual(expectedDate, raceEntity.Date);
            }

            var updateColumns = new
            {
                NumberOfSensors = expectedNumberOfSensors,
                Date = expectedDate
            };

            var affectedRows = await raceDao.UpdateAsync(updateColumns, updateCondition);

            Assert.Equal(comparisonRaceIds.Length, affectedRows);

            foreach (var raceId in comparisonRaceIds)
            {
                var raceEntity = await raceDao.GetByIdAsync(raceId);
                Assert.Equal(expectedNumberOfSensors, raceEntity.NumberOfSensors);
                Assert.Equal(expectedDate, raceEntity.Date);
            }
        }

        public static IEnumerable<object[]> UpdateWithConditionThatCapturesNothingTestsData
        {
            get
            {
                yield return new object[] { typeof(Entities.Country), new { Name = "Test" } };
                yield return new object[] { typeof(Entities.Race), new { NumberOfSensors = 10 } };
                yield return new object[] { typeof(Entities.RaceData), new { StartListId = 10 } };
                yield return new object[] { typeof(Entities.RaceState), new { Label = "Test" } };
                yield return new object[] { typeof(Entities.RaceType), new { Label = "Test" } };
                yield return new object[] { typeof(Entities.Season), new { Name = "Test" } };
                yield return new object[] { typeof(Entities.Sex), new { Label = "Test" } };
                yield return new object[] { typeof(Entities.Skier), new { FirstName = "Test" } };
                yield return new object[] { typeof(Entities.StartPosition), new { Position = 10 } };
                yield return new object[] { typeof(Entities.TimeMeasurement), new { Measurement = 10 } };
                yield return new object[] { typeof(Entities.Venue), new { Name = "Test" } };
            }
        }

        [Theory]
        [MemberData(nameof(UpdateWithConditionThatCapturesNothingTestsData))]
        public async Task UpdateWithConditionThatCapturesNothingTests(Type domainType, object updatedColumns)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var getAllMethod = GetAdoDaoMethodInfo(
                domainType,
                "GetAllConditionalAsync");

            var condition = new QueryConditionBuilder()
                .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.GreaterThan, 50000)
                .Build();

            var getAllParameterList = new object[] { condition };

            var allCapturedDomainObjectsDynamic = await (dynamic)getAllMethod.Invoke(adoDaoInstance, getAllParameterList);
            var allCapturedDomainObjects = (IEnumerable<object>)allCapturedDomainObjectsDynamic;

            Assert.Empty(allCapturedDomainObjects);

            var updateMethod = GetAdoDaoMethodInfo(
                domainType,
                "UpdateAsync",
                new Type[] { typeof(object), typeof(IQueryCondition) });

            var updateParameters = new object[] { updatedColumns, condition };

            var affectedRows = (int)await (dynamic)updateMethod.Invoke(adoDaoInstance, updateParameters);

            Assert.Equal(0, affectedRows);
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task UpdateWithInvalidUpdatedValuesTests1(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var updateMethod = GetAdoDaoMethodInfo(
                domainType,
                "UpdateAsync",
                new Type[] { typeof(object), typeof(IQueryCondition) });

            var parameterList = new object[]
            {
                null,
                new QueryConditionBuilder()
                    .DeclareCondition("Id", QueryConditionType.Equals, 1)
                    .Build()
            };

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => (dynamic)updateMethod.Invoke(adoDaoInstance, parameterList));
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task UpdateWithInvalidUpdatedValuesTests2(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var updateMethod = GetAdoDaoMethodInfo(
                domainType,
                "UpdateAsync",
                new Type[] { typeof(object), typeof(IQueryCondition) });

            var parameterList = new object[]
            {
                new { },
                new QueryConditionBuilder()
                    .DeclareCondition("Id", QueryConditionType.Equals, 1)
                    .Build()
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => (dynamic)updateMethod.Invoke(adoDaoInstance, parameterList));
        }

        public static IEnumerable<object[]> UpdateWithInvalidUpdatedValuesTests3TestData
        {
            get
            {
                yield return new object[] { typeof(Entities.Country), new { Name = 10 } };
                yield return new object[] { typeof(Entities.Race), new { NumberOfSensors = "Test" } };
                yield return new object[] { typeof(Entities.RaceData), new { StartListId = "Test" } };
                yield return new object[] { typeof(Entities.RaceState), new { Label = 10 } };
                yield return new object[] { typeof(Entities.RaceType), new { Label = 10 } };
                yield return new object[] { typeof(Entities.Season), new { Name = 10 } };
                yield return new object[] { typeof(Entities.SeasonPlan), new { VenueId = "Test" } };
                yield return new object[] { typeof(Entities.Sex), new { Label = 10 } };
                yield return new object[] { typeof(Entities.Skier), new { FirstName = 10 } };
                yield return new object[] { typeof(Entities.StartPosition), new { Position = "Test" } };
                yield return new object[] { typeof(Entities.TimeMeasurement), new { Measurement = "Test" } };
                yield return new object[] { typeof(Entities.Venue), new { Name = 10 } };
            }
        }

        [Theory]
        [MemberData(nameof(UpdateWithInvalidUpdatedValuesTests3TestData))]
        public async Task UpdateWithInvalidUpdatedValuesTests3(Type domainType, object updateObject)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var updateMethod = GetAdoDaoMethodInfo(
                domainType,
                "UpdateAsync",
                new Type[] { typeof(object), typeof(IQueryCondition) });

            var condition = new QueryConditionBuilder()
                .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.Equals, 0)
                .Build();

            var parameterList = new object[] { updateObject, condition };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => (dynamic)updateMethod.Invoke(adoDaoInstance, parameterList));
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task UpdateWithIdUpdateTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var updateMethod = GetAdoDaoMethodInfo(
                domainType,
                "UpdateAsync",
                new Type[] { typeof(object), typeof(IQueryCondition) });

            var condition = new QueryConditionBuilder()
                .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.Equals, 0)
                .Build();

            var parameterList = new object[] { new { Id = 0 }, condition };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => (dynamic)updateMethod.Invoke(adoDaoInstance, parameterList));
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task UpdateWithInvalidQueryConditionTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var updateMethod = GetAdoDaoMethodInfo(
                domainType,
                "UpdateAsync",
                new Type[] { typeof(object), typeof(IQueryCondition) });

            var parameterList = new object[] { new object { }, null };

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => (dynamic)updateMethod.Invoke(adoDaoInstance, parameterList));
        }

        [Theory]
        [InlineData(typeof(Entities.Country), false)]
        [InlineData(typeof(Entities.Race), true)]
        [InlineData(typeof(Entities.RaceData), false)]
        [InlineData(typeof(Entities.RaceState), false)]
        [InlineData(typeof(Entities.RaceType), false)]
        [InlineData(typeof(Entities.Season), false)]
        [InlineData(typeof(Entities.SeasonPlan), true)]
        [InlineData(typeof(Entities.Sex), false)]
        [InlineData(typeof(Entities.Skier), false)]
        [InlineData(typeof(Entities.StartList), false)]
        [InlineData(typeof(Entities.StartPosition), true)]
        [InlineData(typeof(Entities.TimeMeasurement), true)]
        [InlineData(typeof(Entities.Venue), false)]
        public async Task DeleteByIdExistingIdTests(Type domainType, bool expectedSuccess)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoDeleteByIdMethod = GetAdoDaoMethodInfo(domainType, "DeleteByIdAsync");

            int deleteIdParam = 0;
            var paramList = new object[] { deleteIdParam };

            if (expectedSuccess)
            {
                var actualSuccess = (bool)await (dynamic)daoDeleteByIdMethod.Invoke(adoDaoInstance, paramList);

                Assert.Equal(expectedSuccess, actualSuccess);

                var daoGetByIdMethod = GetAdoDaoMethodInfo(domainType, "GetByIdAsync");
                var expectedNotFoundDomainObject =
                    (Entities.EntityObjectBase)await
                        (dynamic)daoGetByIdMethod.Invoke(adoDaoInstance, paramList);

                Assert.Null(expectedNotFoundDomainObject);
            }
            else
            {
                await Assert.ThrowsAsync<SqlException>(
                    () => (dynamic)daoDeleteByIdMethod.Invoke(adoDaoInstance, paramList));
            }
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task DeleteByNotExistingIdTest(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var daoDeleteByIdMethod = GetAdoDaoMethodInfo(domainType, "DeleteByIdAsync");

            int deleteIdParam = 50000;
            var paramList = new object[] { deleteIdParam };
            var success = (bool)await (dynamic)daoDeleteByIdMethod.Invoke(adoDaoInstance, paramList);

            Assert.False(success);
        }

        [Theory]
        [InlineData(typeof(Entities.Country), false)]
        [InlineData(typeof(Entities.Race), true)]
        [InlineData(typeof(Entities.RaceData), false)]
        [InlineData(typeof(Entities.RaceState), false)]
        [InlineData(typeof(Entities.RaceType), false)]
        [InlineData(typeof(Entities.Season), false)]
        [InlineData(typeof(Entities.SeasonPlan), true)]
        [InlineData(typeof(Entities.Sex), false)]
        [InlineData(typeof(Entities.Skier), false)]
        [InlineData(typeof(Entities.StartList), false)]
        [InlineData(typeof(Entities.StartPosition), true)]
        [InlineData(typeof(Entities.TimeMeasurement), true)]
        [InlineData(typeof(Entities.Venue), false)]
        public async Task DeleteWithConditionTests(Type domainType, bool expectedSuccess)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var deleteMethod = GetAdoDaoMethodInfo(
                domainType,
                "DeleteAsync",
                new Type[] { typeof(IQueryCondition) });

            var condition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.Or,
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.Equals, 0),
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.Equals, 1))
                .Build();

            var parameterList = new object[] { condition };

            if (expectedSuccess)
            {
                var affectedRows = (int)await (dynamic)deleteMethod.Invoke(adoDaoInstance, parameterList);
                Assert.Equal(2, affectedRows);

                var getAllMethod = GetAdoDaoMethodInfo(
                    domainType,
                    "GetAllConditionalAsync");

                var matchingObjects = (IEnumerable<object>)await (dynamic)getAllMethod.Invoke(adoDaoInstance, parameterList);
                Assert.Empty(matchingObjects);
            }
            else
            {
                await Assert.ThrowsAsync<SqlException>(
                    () => (dynamic)deleteMethod.Invoke(adoDaoInstance, parameterList));
            }
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task DeleteWithConditionThatCapturesNothingTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var getAllMethod = GetAdoDaoMethodInfo(
                domainType,
                "GetAllConditionalAsync");

            var condition = new QueryConditionBuilder()
                .DeclareCondition(nameof(Entities.EntityObjectBase.Id), QueryConditionType.GreaterThan, 50000)
                .Build();

            var getAllParameterList = new object[] { condition };

            var allCapturedDomainObjectsDynamic = await (dynamic)getAllMethod.Invoke(adoDaoInstance, getAllParameterList);
            var allCapturedDomainObjects = (IEnumerable<object>)allCapturedDomainObjectsDynamic;

            Assert.Empty(allCapturedDomainObjects);

            var deleteMethod = GetAdoDaoMethodInfo(
                domainType,
                "DeleteAsync",
                new Type[] { typeof(IQueryCondition) });

            var deleteParameters = new object[] { condition };

            var affectedRows = (int)await (dynamic)deleteMethod.Invoke(adoDaoInstance, deleteParameters);

            Assert.Equal(0, affectedRows);
        }

        [Theory]
        [InlineData(typeof(Entities.Country))]
        [InlineData(typeof(Entities.Race))]
        [InlineData(typeof(Entities.RaceData))]
        [InlineData(typeof(Entities.RaceState))]
        [InlineData(typeof(Entities.RaceType))]
        [InlineData(typeof(Entities.Season))]
        [InlineData(typeof(Entities.SeasonPlan))]
        [InlineData(typeof(Entities.Sex))]
        [InlineData(typeof(Entities.Skier))]
        [InlineData(typeof(Entities.StartList))]
        [InlineData(typeof(Entities.StartPosition))]
        [InlineData(typeof(Entities.TimeMeasurement))]
        [InlineData(typeof(Entities.Venue))]
        public async Task DeleteWithInvalidConditionTests(Type domainType)
        {
            var adoDaoInstance = GetAdoDaoInstance(domainType);
            var deleteMethod = GetAdoDaoMethodInfo(
                domainType,
                "DeleteAsync");

            var parameterList = new object[] { null };

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => (dynamic)deleteMethod.Invoke(adoDaoInstance, parameterList));
        }

        #region Helper Methods

        private static object GetAdoDaoInstance(Type domainType)
        {
            var adoDao = typeof(GenericDao<>)
                .MakeGenericType(domainType);

            var constructorParameterTypeList = new Type[] { typeof(IConnectionFactory) };
            var constructorParameterList = new object[] { new DefaultConnectionFactory() };

            var adoDaoInstance = adoDao.GetConstructor(constructorParameterTypeList)
                .Invoke(constructorParameterList);

            return adoDaoInstance;
        }

        private static MethodInfo GetAdoDaoMethodInfo(
            Type domainType,
            string methodName,
            Type[] searchedMethodParameterTypes = null)
        {
            var adoDao = typeof(GenericDao<>)
                .MakeGenericType(domainType);

            var equallyNamedMethods = adoDao.GetMethods()
                .Where(m => m.Name == methodName);

            if (equallyNamedMethods.Count() == 1 || searchedMethodParameterTypes == null)
            {
                return equallyNamedMethods.First();
            }
            else
            {
                return equallyNamedMethods
                    .Where(m => m.GetParameters().Length == searchedMethodParameterTypes.Length)
                    .First(m =>
                    {
                        var actualMethodParameterTypes = m.GetParameters();
                        for (int i = 0; i < actualMethodParameterTypes.Length; i++)
                        {
                            if (actualMethodParameterTypes[i].ParameterType != searchedMethodParameterTypes[i])
                            {
                                return false;
                            }
                        }
                        return true;
                    });
            }
        }

        private static IQueryCondition GenerateQueryConditionsForRuntimeType(Type currentDomainType)
        {
            QueryConditionBuilder conditionBuilder = null;

            var testableDomainObject = GenerateTestableCompareObject(currentDomainType);

            foreach (var currentProperty in currentDomainType.GetProperties())
            {
                var newQueryConditionBuilder = new QueryConditionBuilder()
                    .DeclareCondition(
                        currentProperty.Name,
                        QueryConditionType.Equals,
                        currentProperty.GetValue(testableDomainObject));

                if (conditionBuilder == null)
                {
                    conditionBuilder = newQueryConditionBuilder;
                }
                else
                {
                    conditionBuilder = new QueryConditionBuilder()
                        .DeclareConditionNode(
                            QueryConditionNodeType.And,
                            () => conditionBuilder,
                            () => newQueryConditionBuilder);
                }
            }

            return conditionBuilder.Build();
        }

        private static Entities.EntityObjectBase GenerateTestableCompareObject(Type currentDomainType)
        {
            Entities.EntityObjectBase testObject;

            switch (currentDomainType.Name)
            {
                case nameof(Entities.Country):
                    testObject = new Entities.Country()
                    {
                        Id = 5,
                        Name = "SUI"
                    };
                    return testObject;
                case nameof(Entities.Race):
                    testObject = new Entities.Race()
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
                case nameof(Entities.RaceData):
                    testObject = new Entities.RaceData()
                    {
                        Id = 311,
                        StartListId = 6,
                        SkierId = 164,
                        RaceStateId = 1
                    };
                    return testObject;
                case nameof(Entities.RaceState):
                    testObject = new Entities.RaceState()
                    {
                        Id = 2,
                        Label = "NichtAbgeschlossen"
                    };
                    return testObject;
                case nameof(Entities.RaceType):
                    testObject = new Entities.RaceType()
                    {
                        Id = 0,
                        Label = "Riesentorlauf"
                    };
                    return testObject;
                case nameof(Entities.Season):
                    testObject = new Entities.Season()
                    {
                        Id = 1,
                        Name = "Jährliche Saison 2018",
                        StartDate = new DateTime(2018, 1, 1),
                        EndDate = new DateTime(2018, 12, 31)
                    };
                    return testObject;
                case nameof(Entities.SeasonPlan):
                    testObject = new Entities.SeasonPlan()
                    {
                        Id = 53,
                        VenueId = 22,
                        SeasonId = 1
                    };
                    return testObject;
                case nameof(Entities.Sex):
                    testObject = new Entities.Sex()
                    {
                        Id = 0,
                        Label = "Weiblich"
                    };
                    return testObject;
                case nameof(Entities.Skier):
                    testObject = new Entities.Skier()
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
                case nameof(Entities.StartList):
                    testObject = new Entities.StartList()
                    {
                        Id = 41
                    };
                    return testObject;
                case nameof(Entities.StartPosition):
                    testObject = new Entities.StartPosition()
                    {
                        Id = 53,
                        SkierId = 0,
                        StartListId = 0,
                        Position = 54
                    };
                    return testObject;
                case nameof(Entities.TimeMeasurement):
                    testObject = new Entities.TimeMeasurement()
                    {
                        Id = 226,
                        SensorId = 1,
                        Measurement = 59000,
                        RaceDataId = 45,
                        IsValid = true
                    };
                    return testObject;
                case nameof(Entities.Venue):
                    testObject = new Entities.Venue()
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

        private static void AlterDomainObjectRandomly(object domainObject)
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
