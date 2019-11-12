using System.Collections.Generic;

namespace Hurace.Domain
{
    public class RaceState : DomainObjectBase
    {
        public string Label { get; set; }
        public List<int> RaceDataIds { get; }
    }
}
