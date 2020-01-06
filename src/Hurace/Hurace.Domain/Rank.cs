using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Domain
{
    public sealed class RankedSkier : Skier
    {
        public int Rank { get; set; }
        public TimeSpan ElapsedTimeInFirstRun { get; set; }
        public TimeSpan ElapsedTimeInSecondRun { get; set; }
        public TimeSpan ElapsedTotalTime { get; set; }
    }
}
