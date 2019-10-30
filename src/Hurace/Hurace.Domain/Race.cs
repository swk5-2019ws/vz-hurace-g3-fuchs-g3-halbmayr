using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Race
    {
        public int RaceId { get; set; }
        public RaceType RaceType { get; set; }
        public Venue Venue { get; set; }
        public StartList FirstStartList { get; set; }
        public StartList SecondStartList { get; set; }
        public int NumberOfSensors { get; set; }
        public string Description { get; set; }
        public List<RaceData> RaceData { get; set; }
    }
}
