using System;
using Xunit;

namespace Hurace.Domain.Tests
{
    public class AssociatedDomainObjectTests
    {
        [Fact]
        public void AccessForeignKeyOverReferenceTest()
        {
            int expectedId = 5;
            string expectedName = "Austria";

            var associatedCountry = new Associated<Country>(
                new Country
                {
                    Id = expectedId,
                    Name = expectedName
                });

            Assert.Equal(expectedId, associatedCountry.ForeignKey);
            Assert.True(associatedCountry.HasReference);
            Assert.Equal(expectedId, associatedCountry.Reference.Id);
            Assert.Equal(expectedName, associatedCountry.Reference.Name);
        }

        [Fact]
        public void SetForeignKeyOnExistingForeignKeyWorking()
        {
            var expectedForeignKey = 5;
            var associatedCountry = new Associated<Country>(expectedForeignKey);
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
            Assert.False(associatedCountry.HasReference);

            expectedForeignKey = 10;
            associatedCountry.ForeignKey = expectedForeignKey;
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
            Assert.False(associatedCountry.HasReference);
        }

        [Fact]
        public void SetForeignKeyOnExistingReferenceFailing()
        {
            var associatedCountry = new Associated<Country>(new Country());
            Assert.True(associatedCountry.HasReference);
            Assert.Throws<InvalidOperationException>(() => associatedCountry.ForeignKey = 0);
        }

        [Fact]
        public void SetForeignKeyOnExistingReferenceWorking()
        {
            var associatedCountry = new Associated<Country>(new Country());
            Assert.True(associatedCountry.HasReference);

            var expectedForeignKey = 5;
            associatedCountry.Reference = null;
            associatedCountry.ForeignKey = expectedForeignKey;
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
            Assert.False(associatedCountry.HasReference);
        }

        [Fact]
        public void SetReferenceOnExistingReferenceWorking()
        {
            var expectedCountry = new Country
            {
                Id = 0,
                Name = "Germany"
            };

            var associatedCountry = new Associated<Country>(expectedCountry);
            Assert.Equal(expectedCountry.Id, associatedCountry.Reference.Id);
            Assert.Equal(expectedCountry.Name, associatedCountry.Reference.Name);
            Assert.True(associatedCountry.HasReference);

            expectedCountry = new Country
            {
                Id = 1,
                Name = "India"
            };
            associatedCountry.Reference = expectedCountry;
            Assert.Equal(expectedCountry.Id, associatedCountry.Reference.Id);
            Assert.Equal(expectedCountry.Name, associatedCountry.Reference.Name);
            Assert.True(associatedCountry.HasReference);
        }

        [Fact]
        public void SetReferenceOnExistingForeignKeyFailing()
        {
            var associatedCountry = new Associated<Country>(5);
            Assert.False(associatedCountry.HasReference);
            Assert.Throws<InvalidOperationException>(() => associatedCountry.Reference = null);
        }

        [Fact]
        public void SetReferenceOnExistingForeignKeyWorking()
        {
            var associatedCountry = new Associated<Country>(5);
            Assert.False(associatedCountry.HasReference);

            var expectedCountry = new Country
            {
                Id = 1,
                Name = "India"
            };
            associatedCountry.ForeignKey = null;
            associatedCountry.Reference = expectedCountry;
            Assert.Equal(expectedCountry.Id, associatedCountry.Reference.Id);
            Assert.Equal(expectedCountry.Name, associatedCountry.Reference.Name);
            Assert.True(associatedCountry.HasReference);
        }
    }
}
