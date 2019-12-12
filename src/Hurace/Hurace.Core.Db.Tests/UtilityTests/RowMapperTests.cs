using FakeItEasy;
using Hurace.Core.Db.Utilities;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace Hurace.Core.Db.Tests.UtilityTests
{
    public class RowMapperTests
    {
        [Fact]
        public void MapWithNullRow()
        {
            var rowMapper = new RowMapper<Entities.StartPosition>();

            try
            {
                rowMapper.Map(null);
                Assert.False(true, "ArgumentNullException expected");
            }
            catch (ArgumentNullException e)
            {
                string parameterName = rowMapper.GetType()
                        .GetMethod(nameof(rowMapper.Map))
                        .GetParameters()
                        .Select(p => p.Name)
                        .FirstOrDefault();

                Assert.Contains(parameterName, e.Message, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        [Fact]
        public void MapStartPosition()
        {
            var rowMapper = new RowMapper<Entities.StartPosition>();

            var mockedDataRecord = A.Fake<IDataRecord>();

            int expectedId = 500;
            int expectedStartListId = 501;
            int expectedSkierId = 502;
            int expectedPosition = 503;

            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("Id")]).Returns(expectedId);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("StartListId")]).Returns(expectedStartListId);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("SkierId")]).Returns(expectedSkierId);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("Position")]).Returns(expectedPosition);

            var mappedStartPosition = rowMapper.Map(mockedDataRecord);

            Assert.Equal(expectedId, mappedStartPosition.Id);
            Assert.Equal(expectedStartListId, mappedStartPosition.StartListId);
            Assert.Equal(expectedSkierId, mappedStartPosition.SkierId);
            Assert.Equal(expectedPosition, mappedStartPosition.Position);
        }

        [Fact]
        public void MapWithDateTimeStruct()
        {
            var rowMapper = new RowMapper<Entities.Race>();

            var mockedDataRecord = A.Fake<IDataRecord>();

            var expectedInt = 15;
            var expectedString = "Test";
            var expectedDate = new DateTime(2019, 11, 13);

            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("Id")]).Returns(expectedInt);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("RaceTypeId")]).Returns(expectedInt);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("VenueId")]).Returns(expectedInt);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("FirstStartListId")]).Returns(expectedInt);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("SecondStartListId")]).Returns(expectedInt);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("NumberOfSensors")]).Returns(expectedInt);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("Description")]).Returns(expectedString);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("Date")]).Returns(expectedDate);

            var mappedStartPosition = rowMapper.Map(mockedDataRecord);

            Assert.Equal(expectedInt, mappedStartPosition.Id);
            Assert.Equal(expectedInt, mappedStartPosition.RaceTypeId);
            Assert.Equal(expectedInt, mappedStartPosition.VenueId);
            Assert.Equal(expectedInt, mappedStartPosition.FirstStartListId);
            Assert.Equal(expectedInt, mappedStartPosition.SecondStartListId);
            Assert.Equal(expectedInt, mappedStartPosition.NumberOfSensors);
            Assert.Equal(expectedString, mappedStartPosition.Description);
            Assert.Equal(expectedDate, mappedStartPosition.Date);
        }
    }
}
