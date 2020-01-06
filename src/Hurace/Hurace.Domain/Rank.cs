using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Domain
{
    public sealed class RankedSkier : Skier
    {
        public int Rank { get; set; }
        public Associated<RaceData> FirstRun { get; set; }
        public Associated<RaceData> SecondRun { get; set; }
    }
}
