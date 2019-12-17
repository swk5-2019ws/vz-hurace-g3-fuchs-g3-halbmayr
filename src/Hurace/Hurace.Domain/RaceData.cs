using System.Collections.Generic;

namespace Hurace.Domain
{
    public class RaceData : DomainObjectBase
    {
        public Associated<StartPosition> StartPosition { get; set; }
        public Associated<RaceState> RaceState { get; set; }
        public IEnumerable<Associated<TimeMeasurement>> TimeMeasurement { get; set; }
    }
}
