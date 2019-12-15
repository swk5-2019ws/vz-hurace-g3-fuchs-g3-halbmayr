using System;
using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Race : DomainObjectBase
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int NumberOfSensors { get; set; }
        public RaceType RaceType { get; set; }
        public Venue Venue { get; set; }
        public IEnumerable<StartPosition> FirstStartList { get; set; }
        public IEnumerable<StartPosition> SecondStartList { get; set; }
    }
}
