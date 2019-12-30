using System;
using Xunit;

#pragma warning disable IDE0017 // Simplify object initialization
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

            Assert.Null(associatedCountry.ForeignKey);
            Assert.Equal(expectedId, associatedCountry.Reference.Id);
            Assert.Equal(expectedName, associatedCountry.Reference.Name);
        }

        [Fact]
        public void SetForeignKeyOnExistingForeignKeyWorking()
        {
            var expectedForeignKey = 5;
            var associatedCountry = new Associated<Country>(expectedForeignKey);
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);

            expectedForeignKey = 10;
            associatedCountry.ForeignKey = expectedForeignKey;
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
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

            var expectedForeignKey = 5;
            associatedCountry.Reference = null;
            associatedCountry.ForeignKey = expectedForeignKey;
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
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

            expectedCountry = new Country
            {
                Id = 1,
                Name = "India"
            };
            associatedCountry.Reference = expectedCountry;
            Assert.Equal(expectedCountry.Id, associatedCountry.Reference.Id);
            Assert.Equal(expectedCountry.Name, associatedCountry.Reference.Name);
        }

        [Fact]
        public void SetNullReferenceOnExistingForeignKeyWorking()
        {
            int expectedForeignKey = 5;
            var associatedCountry = new Associated<Country>(expectedForeignKey);
            associatedCountry.Reference = null;
            Assert.Equal(expectedForeignKey, associatedCountry.ForeignKey);
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

            var expectedCountry = new Country
            {
                Id = 1,
                Name = "India"
            };
            associatedCountry.ForeignKey = null;
            associatedCountry.Reference = expectedCountry;
            Assert.Equal(expectedCountry.Id, associatedCountry.Reference.Id);
            Assert.Equal(expectedCountry.Name, associatedCountry.Reference.Name);
        }

        [Fact]
        public void CreateEmptyAssociatedAndInitializeForeignKeyAfterwards()
        {
            var associatedCountry = new Associated<Country>();
            Assert.Null(associatedCountry.ForeignKey);
            Assert.Null(associatedCountry.Reference);

            var expectedFk = 5;
            associatedCountry.ForeignKey = expectedFk;
            Assert.Equal(expectedFk, associatedCountry.ForeignKey);
            Assert.Null(associatedCountry.Reference);
        }

        [Fact]
        public void CreateEmptyAssociateAndInitializeReferenceAfterwards()
        {
            var associatedCountry = new Associated<Country>();
            Assert.Null(associatedCountry.ForeignKey);
            Assert.Null(associatedCountry.Reference);

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
        }
    }
}
