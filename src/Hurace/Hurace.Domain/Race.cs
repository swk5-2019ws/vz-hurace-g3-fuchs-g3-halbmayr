using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Race
    {
        public int RaceId { get; set; }
        public string RaceTypeLabel { get; set; }
        public string VenueName { get; set; }
        public int FirstStartListId { get; set; }
        public int SecondStartListId { get; set; }
        public int NumberOfSensors { get; set; }
        public string Description { get; set; }
        public List<int> RaceDataIds { get; set; }
    }
}
