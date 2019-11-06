using System.Collections.Generic;

namespace Hurace.Domain
{
    public class RaceState
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public List<int> RaceDataIds { get; }
    }
}
