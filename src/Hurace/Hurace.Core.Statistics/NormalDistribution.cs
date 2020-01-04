using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurace.Core.Statistics
{
    public static class NormalDistribution
    {
        public static double CalculateStandardDeviation(IEnumerable<int> values)
        {
            if (!values.Any())
                throw new InvalidOperationException($"{nameof(values)} is empty -> can't calculate the standard deviation");

            double average = values.Average();

            var sumOfSquares = values
                .Select(m => (m - average) * (m - average))
                .Sum();

            return Math.Sqrt(sumOfSquares / values.Count());
        }

        public static double DenormalizeValue(double mean, double stdDev, double value) => (value * stdDev) + mean;

        public static (double lowerBoundary, double upperBoundary) GetBoundaries(
            double mean,
            double stdDev,
            double areaCoverage)
        {
            if (areaCoverage <= 0 || areaCoverage >= 1)
                throw new InvalidOperationException($"{nameof(areaCoverage)} has to be in boundary ]0,1[ but is {areaCoverage}");

            var standardNormalDistribution = new Accord.Statistics.Distributions.Univariate.NormalDistribution(0, 1);

            var lowerBoundaryPercentage = (1 - areaCoverage) / 2;
            var upperBoundaryPercentage = 1 - lowerBoundaryPercentage;

            var standardizedLowerBoundary = standardNormalDistribution
                .InverseDistributionFunction(lowerBoundaryPercentage);

            var standardizedUpperBoundary = standardNormalDistribution
                .InverseDistributionFunction(upperBoundaryPercentage);

            return (
                DenormalizeValue(mean, stdDev, standardizedLowerBoundary),
                DenormalizeValue(mean, stdDev, standardizedUpperBoundary));
        }
    }
}
