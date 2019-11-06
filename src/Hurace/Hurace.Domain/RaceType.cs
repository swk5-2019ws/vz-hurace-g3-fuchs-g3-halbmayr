using System.Collections.Generic;

namespace Hurace.Domain
{
    public class RaceType
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public List<int> RaceIds { get; set; }
    }
}
