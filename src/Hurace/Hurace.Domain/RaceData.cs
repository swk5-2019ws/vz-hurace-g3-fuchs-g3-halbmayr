using System.Collections.Generic;

namespace Hurace.Domain
{
    public class RaceData : DomainObjectBase
    {
        public int StartListId { get; set; }
        public int SkierId { get; set; }
        public int RaceStateId { get; set; }
    }
}
