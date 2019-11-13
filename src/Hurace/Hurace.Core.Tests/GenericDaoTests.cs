using Hurace.Core.Dal.AdoPersistence;
using Hurace.Core.Db.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hurace.Core.Tests
{
    public class GenericDaoTests
    {
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
        public async Task TestGetAll(Type domainType, int expectedResultCount)
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
            var genericGetAllTask = typeof(Task<>).MakeGenericType(genericDomainObjectListType);

            var emptyParameterList = Array.Empty<object>();
            var getAllDomainObjectsBlankTask = (Task)getAllMethod.Invoke(adoDaoInstance, emptyParameterList);

            await getAllDomainObjectsBlankTask;

            var allDomainObjectsUnconverted = ((dynamic)getAllDomainObjectsBlankTask).Result;

            var allDomainObjectsConverted = Convert.ChangeType(allDomainObjectsUnconverted, genericDomainObjectListType);

            var allDomainObjects = (IEnumerable<object>)allDomainObjectsConverted;

            Assert.Equal(expectedResultCount, allDomainObjects.Count());
        }
    }
}
