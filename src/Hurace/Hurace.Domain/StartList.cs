using System.Collections.Generic;

namespace Hurace.Domain
{
    public class StartList : DomainObjectBase
    {
        public int RaceId { get; set; }
        public List<int> StartPositionIds { get; }
        public List<int> RaceDataIds { get; }
    }
}
