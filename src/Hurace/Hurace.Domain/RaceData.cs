using System.Collections.Generic;

namespace Hurace.Domain
{
    public class RaceData : DomainObjectBase
    {
        public StartPosition StartPosition { get; set; }
        public RaceState RaceState { get; set; }
        public IEnumerable<Associated<TimeMeasurement>> TimeMeasurement { get; set; }
    }
}
