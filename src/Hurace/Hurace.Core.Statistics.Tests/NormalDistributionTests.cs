using System;
using System.Collections.Generic;
using Xunit;

namespace Hurace.Core.Statistics.Tests
{
    public class NormalDistributionTests
    {
        [Fact]
        public void CalculateMeanTest1()
        {
            var values = new List<int> { 1, 2, 3 };
            var expectedMean = 2;

            var mean = NormalDistribution.CalculateMean(values);

            Assert.Equal(expectedMean, Math.Round(mean, 0));
        }

        [Fact]
        public void CalculateMeanTest2()
        {
            var values = new List<int>
            {
                21, 68, 96, 80, 36, 70, 60, 14, 24, 30, 95, 20, 62, 44, 40, 53, 77, 50, 39,
                33, 79, 46, 2, 48, 11, 64, 34, 91, 22, 10, 58, 4, 69, 67, 83, 29, 37, 65, 49,
                1, 84, 87, 16, 71, 17, 25, 94, 74, 6,  51, 89, 35, 92, 15, 63, 54, 72, 42, 98,
                99, 23, 90, 31, 88, 8, 93, 78, 3, 9, 45, 85, 76, 47, 13, 97, 5, 32, 41, 52,
                18, 75, 82, 73, 57, 86, 19, 43, 100, 66, 59, 28, 7, 12, 81, 56, 38, 26, 61, 55, 27
            };
            var expectedMean = 50.5;

            var mean = NormalDistribution.CalculateMean(values);

            Assert.Equal(expectedMean, Math.Round(mean, 1));
        }

        [Fact]
        public void CalculateMeanFailingTest()
        {
            var values = new List<int>();

            var mean = NormalDistribution.CalculateMean(values);
        }

        [Fact]
        public void CalculateStandardDeviationTest1()
        {
            var values = new List<int> { 1, 2, 3 };
            var expectedStandardDeviation = 0.82;

            var standardDeviation = NormalDistribution.CalculateStandardDeviation(values);

            Assert.Equal(expectedStandardDeviation, Math.Round(standardDeviation, 2));
        }

        [Fact]
        public void CalculateStandardDeviationTest2()
        {
            var values = new List<int>
            {
                21, 68, 96, 80, 36, 70, 60, 14, 24, 30, 95, 20, 62, 44, 40, 53, 77, 50, 39,
                33, 79, 46, 2, 48, 11, 64, 34, 91, 22, 10, 58, 4, 69, 67, 83, 29, 37, 65, 49,
                1, 84, 87, 16, 71, 17, 25, 94, 74, 6,  51, 89, 35, 92, 15, 63, 54, 72, 42, 98,
                99, 23, 90, 31, 88, 8, 93, 78, 3, 9, 45, 85, 76, 47, 13, 97, 5, 32, 41, 52,
                18, 75, 82, 73, 57, 86, 19, 43, 100, 66, 59, 28, 7, 12, 81, 56, 38, 26, 61, 55, 27
            };
            var expectedStandardDeviation = 28.866;

            var standardDeviation = NormalDistribution.CalculateStandardDeviation(values);

            Assert.Equal(expectedStandardDeviation, Math.Round(standardDeviation, 3));
        }

        [Fact]
        public void CalculateStandardDeviationTest3()
        {
            var values = new List<int> { 1 };
            var expectedStandardDeviation = 0;

            var standardDeviation = NormalDistribution.CalculateStandardDeviation(values);

            Assert.Equal(expectedStandardDeviation, standardDeviation);
        }

        [Fact]
        public void CalculateStandardDeviationFailingTest()
        {
            var values = new List<int> { };

            Assert.Throws<InvalidOperationException>(
                () => NormalDistribution.CalculateStandardDeviation(values));
        }

        [Fact]
        public void CalculateBoundariesTest()
        {
            var expectedLowerBoundary = -1.96;
            var expectedUpperBoundary = 1.96;

            (var actualLowerBoundary, var actualUpperBoundardy) = NormalDistribution.CalculateBoundaries(0, 1, 0.95);

            Assert.Equal(expectedLowerBoundary, Math.Round(actualLowerBoundary, 2));
            Assert.Equal(expectedUpperBoundary, Math.Round(actualUpperBoundardy, 2));
        }

        [Fact]
        public void CalculateBoundariesWithInvalidParametersTest()
        {
            var expectedLowerBoundary = double.NegativeInfinity;
            var expectedUpperBoundary = double.PositiveInfinity;

            (var actualLowerBoundary, var actualUpperBoundardy) = NormalDistribution.CalculateBoundaries(0, double.NaN, 0.95);

            Assert.Equal(expectedLowerBoundary, Math.Round(actualLowerBoundary, 2));
            Assert.Equal(expectedUpperBoundary, Math.Round(actualUpperBoundardy, 2));
        }
    }
}
