using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Venue : DomainObjectBase
    {
        public string Name { get; set; }
        public Associated<Country> Country { get; set; }
        public IEnumerable<Associated<Season>> Seasons { get; set; }
    }
}