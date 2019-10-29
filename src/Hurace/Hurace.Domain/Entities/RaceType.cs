using System.Collections.Generic;

namespace Hurace.Domain.Entities
{
    public class RaceType
    {
        public string Label { get; set; }
        public List<Race> Races { get; set; }
    }
}
