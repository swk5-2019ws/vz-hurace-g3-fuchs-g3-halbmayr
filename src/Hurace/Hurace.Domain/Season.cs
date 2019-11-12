using System;
using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Season : DomainObjectBase
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<int> VenueIds { get; }
    }
}
