using System;

namespace Hurace.Entities
{
    public class Race : EntityObjectBase
    {
        public int RaceTypeId { get; set; }
        public int VenueId { get; set; }
        public int FirstStartListId { get; set; }
        public int SecondStartListId { get; set; }
        public int NumberOfSensors { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int GenderSpecificRaceId { get; set; }
    }
}
