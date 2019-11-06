using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Venue
    {
        public string Name { get; set; }
        public List<int> SeasonIds { get; set; }
        public List<int> RaceIds { get; set; }
    }
}
