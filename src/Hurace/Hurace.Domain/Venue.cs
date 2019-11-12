using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Venue : DomainObjectBase
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
        public List<int> SeasonIds { get; }
        public List<int> RaceIds { get; }
    }
}
