using System.Collections.Generic;

namespace Hurace.Domain
{
    public class RaceData
    {
        public int RaceId { get; set; }
        public int StartListId { get; set; }
        public int SkierId { get; set; }
        public string RaceStateLabel { get; set; }
        public List<int> TimeMeasurementIds { get; set; }
    }
}
