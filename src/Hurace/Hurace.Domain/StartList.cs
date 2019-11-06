using System.Collections.Generic;

namespace Hurace.Domain
{
    public class StartList
    {
        public int StartListId { get; set; }
        public int RaceId { get; set; }
        public List<int> StartPositionIds { get; set; }
        public List<int> RaceDataId { get; set; }
    }
}
