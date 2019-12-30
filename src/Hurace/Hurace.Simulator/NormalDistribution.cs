using System;

#pragma warning disable CA5394 // Do not use insecure randomness
namespace Hurace.Simulator
{
    internal static class NormalDistribution
    {
        internal static double NextDouble(Random randomizer, double mu, double sigma)
        {
            if (randomizer is null)
                throw new ArgumentNullException(nameof(randomizer));

            double a = randomizer.NextDouble();
            double b = randomizer.NextDouble();
            double c = Math.Sqrt(-2.0 * Math.Log(a)) * Math.Cos(2.0 * Math.PI * b);

            return (c * sigma) + mu;
        }
    }
}
