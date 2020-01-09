using System;
using Xunit;

#pragma warning disable IDE0017 // Simplify object initialization
namespace Hurace.Domain.Tests
{
    public class AssociatedDomainObjectTests
    {
        [Fact]
        public void SetForeignKeyOnExistingForeignKeyWorking()
        {
            var expectedForeignKey = 5;
            var associatedCountry = new Associated<Country>(expectedForeignKey);
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
            Assert.True(associatedCountry.Initialised);

            expectedForeignKey = 10;
            associatedCountry.ForeignKey = expectedForeignKey;
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
            Assert.True(associatedCountry.Initialised);
        }

        [Fact]
        public void SetForeignKeyOnExistingReferenceFailing()
        {
            var associatedCountry = new Associated<Country>(new Country());
            Assert.Throws<InvalidOperationException>(() => associatedCountry.ForeignKey = 0);
        }

        [Fact]
        public void SetForeignKeyOnExistingReferenceWorking()
        {
            var associatedCountry = new Associated<Country>(new Country());
            Assert.True(associatedCountry.Initialised);

            var expectedForeignKey = 5;
            associatedCountry.Reference = null;
            associatedCountry.ForeignKey = expectedForeignKey;
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
            Assert.True(associatedCountry.Initialised);
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
            Assert.True(associatedCountry.Initialised);

            expectedCountry = new Country
            {
                Id = 1,
                Name = "India"
            };
            associatedCountry.Reference = expectedCountry;
            Assert.Equal(expectedCountry.Id, associatedCountry.Reference.Id);
            Assert.Equal(expectedCountry.Name, associatedCountry.Reference.Name);
            Assert.True(associatedCountry.Initialised);
        }

        [Fact]
        public void SetNullReferenceOnExistingForeignKeyWorking()
        {
            int expectedForeignKey = 5;
            var associatedCountry = new Associated<Country>(expectedForeignKey);
            associatedCountry.Reference = null;
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
            Assert.True(associatedCountry.Initialised);
        }

        [Fact]
        public void SetReferenceOnExistingForeignKeyFailing()
        {
            var associatedCountry = new Associated<Country>(5);
            Assert.Throws<InvalidOperationException>(() => associatedCountry.Reference = new Country());
        }

        [Fact]
        public void SetReferenceOnExistingForeignKeyWorking()
        {
            var associatedCountry = new Associated<Country>(5);
            Assert.True(associatedCountry.Initialised);

            var expectedCountry = new Country
            {
                Id = 1,
                Name = "India"
            };

            associatedCountry.ForeignKey = null;
            Assert.False(associatedCountry.Initialised);

            associatedCountry.Reference = expectedCountry;
            Assert.Equal(expectedCountry.Id, associatedCountry.Reference.Id);
            Assert.Equal(expectedCountry.Name, associatedCountry.Reference.Name);
            Assert.True(associatedCountry.Initialised);
        }

        [Fact]
        public void CreateEmptyAssociatedAndInitializeForeignKeyAfterwards()
        {
            var associatedCountry = new Associated<Country>();
            Assert.Null(associatedCountry.ForeignKey);
            Assert.Null(associatedCountry.Reference);
            Assert.False(associatedCountry.Initialised);

            var expectedFk = 5;
            associatedCountry.ForeignKey = expectedFk;
            Assert.Equal(expectedFk, associatedCountry.ForeignKey);
            Assert.Null(associatedCountry.Reference);
            Assert.True(associatedCountry.Initialised);
        }

        [Fact]
        public void CreateEmptyAssociateAndInitializeReferenceAfterwards()
        {
            var associatedCountry = new Associated<Country>();
            Assert.Null(associatedCountry.ForeignKey);
            Assert.Null(associatedCountry.Reference);
            Assert.False(associatedCountry.Initialised);

            var expectedId = 1;
            var expectedName = "Tranzilvanien";
            var expectedReference = new Country
            {
                Id = expectedId,
                Name = expectedName
            };
            associatedCountry.Reference = expectedReference;
            Assert.Null(associatedCountry.ForeignKey);
            Assert.Equal(expectedId, associatedCountry.Reference.Id);
            Assert.Equal(expectedName, associatedCountry.Reference.Name);
            Assert.True(associatedCountry.Initialised);
        }
    }
}
