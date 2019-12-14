using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Domain
{
    public class RaceData : DomainObjectBase
    {
        public StartPosition StartPosition { get; set; }
        public RaceState RaceState { get; set; }
        public IEnumerable<TimeMeasurement> TimeMeasurement { get; set; }
    }
}
