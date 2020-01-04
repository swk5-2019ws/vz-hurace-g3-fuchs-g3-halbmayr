using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurace.Core.Statistics
{
    public static class NormalDistribution
    {
        /// <summary>
        /// Calculates the mean of the passed set of <see cref="int"/>
        /// </summary>
        /// <param name="values">set of <see cref="int"/> to calculate the mean value from</param>
        /// <returns>the calculated mean</returns>
        public static double CalculateMean(IEnumerable<int> values)
        {
            return values.Average();
        }

        /// <summary>
        /// Calculates the standard-deviation of the passed set of <see cref="int"/>
        /// </summary>
        /// <param name="values">set of <see cref="int"/> to calculate the standard-deviation from</param>
        /// <returns>the calculated standard-deviation</returns>
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

        /// <summary>
        /// Calculates the x-value boundaries of the area that covers a certain amount 
        /// of cumulative density function of the passed normal-distribution.
        /// </summary>
        /// <param name="mean">mean of the passed normal-distribution</param>
        /// <param name="stdDev">standard-deviation of the passed normal-distribution</param>
        /// <param name="areaCoverage">describes the percentage of area that should be covered below
        /// the normal-distribution function. This percentage has to be in the interval ]0,1[</param>
        /// <returns>a touple containing the lower and upper boundary x-value of the
        /// passed area</returns>
        public static (double lowerBoundary, double upperBoundary) GetBoundaries(
            double mean,
            double stdDev,
            double areaCoverage)
        {
            if (areaCoverage <= 0 || areaCoverage >= 1)
                throw new InvalidOperationException($"{nameof(areaCoverage)} has to be in boundary ]0,1[ but is {areaCoverage}");

            var distribution = new Accord.Statistics.Distributions.Univariate.NormalDistribution(mean, stdDev);

            var lowerBoundaryPercentage = (1 - areaCoverage) / 2;
            var upperBoundaryPercentage = 1 - lowerBoundaryPercentage;

            return (
                distribution.InverseDistributionFunction(lowerBoundaryPercentage),
                distribution.InverseDistributionFunction(upperBoundaryPercentage));
        }
    }
}
