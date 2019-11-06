using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> VenueIds { get; }
    }
}
