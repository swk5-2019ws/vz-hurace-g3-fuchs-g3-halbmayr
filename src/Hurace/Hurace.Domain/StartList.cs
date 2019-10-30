using System.Collections.Generic;

namespace Hurace.Domain
{
    public class StartList
    {
        public int StartListId { get; set; }
        public Race Race { get; set; }
        public List<StartPosition> StartPositions { get; set; }
        public List<RaceData> RaceData { get; set; }
    }
}
