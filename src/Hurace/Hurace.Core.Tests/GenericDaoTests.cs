using Hurace.Core.Dal.AdoPersistence;
using Hurace.Core.Db.Connection;
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
namespace Hurace.Core.Tests
{
    public class GenericDaoTests : IDisposable
    {
        #region Private fields

        #endregion

        #region Transaction Setup Boilerplate Code

        private bool disposed = false;

        private readonly TransactionScope transactionScope;

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
        [InlineData(typeof(Domain.Image), 521)]
        [InlineData(typeof(Domain.Race), 37)]
        [InlineData(typeof(Domain.RaceData), 3694)]
        [InlineData(typeof(Domain.RaceState), 4)]
        [InlineData(typeof(Domain.RaceType), 2)]
        [InlineData(typeof(Domain.Season), 2)]
        [InlineData(typeof(Domain.SeasonPlan), 62)]
        [InlineData(typeof(Domain.Sex), 2)]
        [InlineData(typeof(Domain.Skier), 521)]
        [InlineData(typeof(Domain.StartList), 74)]
        [InlineData(typeof(Domain.StartPosition), 3694)]
        [InlineData(typeof(Domain.TimeMeasurement), 19001)]
        [InlineData(typeof(Domain.Venue), 31)]
        public async Task GetAllTest(Type domainType, int expectedResultCount)
        {
            var adoDao = typeof(GenericDao<>)
                .MakeGenericType(domainType);

            var constructorParameterTypeList = new Type[] { typeof(IConnectionFactory) };
            var constructorParameterList = new object[] { new DefaultConnectionFactory() };

            var adoDaoInstance = adoDao.GetConstructor(constructorParameterTypeList)
                .Invoke(constructorParameterList);

            var getAllMethod = adoDao.GetMethods()
                .FirstOrDefault(m => m.Name == "GetAllAsync");

            var genericDomainObjectListType = typeof(List<>).MakeGenericType(domainType);

            var emptyParameterList = Array.Empty<object>();

            var allDomainObjectsUnconverted = await (dynamic)getAllMethod.Invoke(adoDaoInstance, emptyParameterList);
            var allDomainObjects = (IEnumerable<object>)allDomainObjectsUnconverted;

            Assert.Equal(expectedResultCount, allDomainObjects.Count());

            var expectedDomainObject = this.GenerateTestableCompareObject(domainType);
            var actualDomainObject = allDomainObjects.Skip(expectedDomainObject.Id).First();

            foreach (var currentProperty in expectedDomainObject.GetType().GetProperties())
            {
                Assert.Equal(currentProperty.GetValue(expectedDomainObject), currentProperty.GetValue(actualDomainObject));
            }
        }

        #region Helper Methods

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
                case nameof(Domain.Image):
                    testObject = new Domain.Image()
                    {
                        Id = 2,
                        Content = Array.Empty<byte>()
                    };
                    return testObject;
                case nameof(Domain.Race):
                    testObject = new Domain.Race()
                    {
                        Id = 13,
                        RaceTypeId = 1,
                        FirstStartListId = 26,
                        SecondStartListId = 27,
                        NumberOfSensors = 5,
                        Description = "Norwegian legend Sondre Norheim first began the trend of skis with " +
                                      "curved sides, bindings with stiff heel bands made of willow, and the" +
                                      " slalom turn style.",
                        VenueId = 12,
                        Date = new DateTime(2017, 12, 28)
                    };
                    return testObject;
                case nameof(Domain.RaceData):
                    testObject = new Domain.RaceData()
                    {
                        Id = 311,
                        StartListId = 6,
                        SkierId = 347,
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
                        ImageId = 120
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
                        SkierId = 279,
                        StartListId = 1,
                        Position = 9
                    };
                    return testObject;
                case nameof(Domain.TimeMeasurement):
                    testObject = new Domain.TimeMeasurement()
                    {
                        Id = 226,
                        SensorId = 4,
                        Measurement = 158000,
                        RaceDataId = 37
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

        #endregion
    }
}
