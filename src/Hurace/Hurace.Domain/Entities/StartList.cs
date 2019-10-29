using System.Collections.Generic;

namespace Hurace.Domain.Entities
{
    public class StartList
    {
        public int StartListId { get; set; }
        public Race Race { get; set; }
        public List<StartPosition> StartPositions { get; set; }
        public List<RaceData> RaceData { get; set; }
    }
}
