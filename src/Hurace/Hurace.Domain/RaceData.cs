using System.Collections.Generic;

namespace Hurace.Domain
{
    public class RaceData
    {
        public Race Race { get; set; }
        public StartList StartList { get; set; }
        public Skier Skier { get; set; }
        public RaceState RaceState { get; set; }
        public List<TimeMeasurement> TimeMeasurements { get; set; }
    }
}
