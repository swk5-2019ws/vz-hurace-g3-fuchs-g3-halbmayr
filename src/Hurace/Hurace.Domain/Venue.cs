using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Venue : DomainObjectBase
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
