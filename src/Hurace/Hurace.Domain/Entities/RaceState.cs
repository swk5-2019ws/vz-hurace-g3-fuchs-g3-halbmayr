using System.Collections.Generic;

namespace Hurace.Domain.Entities
{
    public class RaceState
    {
        public string Label { get; set; }
        public List<RaceState> RaceStates { get; set; }
    }
}
