using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Venue : DomainObjectBase
    {
        public string Name { get; set; }
        public Country Country { get; set; }
        public IEnumerable<Season> Seasons { get; set; }
    }
}