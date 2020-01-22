using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Domain
{
    public sealed class RankedSkier : Skier
    {
        public int Rank { get; set; }
        public TimeSpan ElapsedTimeInFirstRun { get; set; }
        public string ElapsedTimeInFirstRunString { get; set; }
        public IEnumerable<string> ElapsedMeasurementStringsInFirstRun { get; set; }

        public TimeSpan ElapsedTimeInSecondRun { get; set; }
        public string ElapsedTimeInSecondRunString { get; set; }
        public IEnumerable<string> ElapsedMeasurementStringsInSecondRun { get; set; }

        public TimeSpan ElapsedTotalTime { get; set; }
        public string ElapsedTotalTimeString { get; set; }
    }
}
