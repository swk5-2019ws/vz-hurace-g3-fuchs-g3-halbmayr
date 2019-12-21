using System;
using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Race : DomainObjectBase
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int NumberOfSensors { get; set; }
        public Associated<RaceType> RaceType { get; set; }
        public Associated<Venue> Venue { get; set; }
        public Associated<Season> Season { get; set; }
        public IEnumerable<Associated<StartPosition>> FirstStartList { get; set; }
        public IEnumerable<Associated<StartPosition>> SecondStartList { get; set; }
        public IEnumerable<Associated<Skier>> Skiers { get; set; }
    }
}
