using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xunit;

namespace Hurace.Core.Tests.DbUtilityTests
{
    public class RowMapperTests
    {
        [Fact]
        public void MapWithNullSkipableElements()
        {
            var rowMapper = new Db.Utilities.RowMapper<Domain.StartPosition>(null);

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
        public void MapWithNullRow()
        {
            var rowMapper = new Db.Utilities.RowMapper<Domain.StartPosition>(null);

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
        public void MapWithNoSkipableElements()
        {
            var rowMapper = new Db.Utilities.RowMapper<Domain.StartPosition>(new List<string>());

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
        public void MapWithSkipableElements()
        {
            var skipableElements = new List<string>()
            {
                nameof(Domain.StartPosition.Id),
                nameof(Domain.StartPosition.StartListId)
            };

            var rowMapper = new Db.Utilities.RowMapper<Domain.StartPosition>(skipableElements);

            var mockedDataRecord = A.Fake<IDataRecord>();

            int expectedSkierId = 502;
            int expectedPosition = 503;

            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("SkierId")]).Returns(expectedSkierId);
            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("Position")]).Returns(expectedPosition);

            var mappedStartPosition = rowMapper.Map(mockedDataRecord);

            Assert.Equal(default, mappedStartPosition.Id);
            Assert.Equal(default, mappedStartPosition.StartListId);
            Assert.Equal(expectedSkierId, mappedStartPosition.SkierId);
            Assert.Equal(expectedPosition, mappedStartPosition.Position);
        }

        [Fact]
        public void MapWithStruct()
        {
            var skipableElements = new List<string>()
            {
                nameof(Domain.Race.Id),
                nameof(Domain.Race.RaceTypeId),
                nameof(Domain.Race.VenueId),
                nameof(Domain.Race.FirstStartListId),
                nameof(Domain.Race.SecondStartListId),
                nameof(Domain.Race.NumberOfSensors),
                nameof(Domain.Race.Description),
                nameof(Domain.Race.RaceDataIds)
            };

            var rowMapper = new Db.Utilities.RowMapper<Domain.Race>(skipableElements);

            var mockedDataRecord = A.Fake<IDataRecord>();

            var expectedDate = new DateTime(2019, 11, 13);

            A.CallTo(() => mockedDataRecord[A<string>.That.IsEqualTo("Date")]).Returns(expectedDate);

            var mappedStartPosition = rowMapper.Map(mockedDataRecord);

            Assert.Equal(default, mappedStartPosition.Id);
            Assert.Equal(default, mappedStartPosition.RaceTypeId);
            Assert.Equal(default, mappedStartPosition.VenueId);
            Assert.Equal(default, mappedStartPosition.FirstStartListId);
            Assert.Equal(default, mappedStartPosition.SecondStartListId);
            Assert.Equal(default, mappedStartPosition.NumberOfSensors);
            Assert.Equal(default, mappedStartPosition.Description);
            Assert.Equal(default, mappedStartPosition.RaceDataIds);
            Assert.Equal(expectedDate, mappedStartPosition.Date);
        }
    }
}
