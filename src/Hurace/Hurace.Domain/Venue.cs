using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Venue
    {
        public string Name { get; set; }
        public List<Season> Seasons { get; set; }
        public List<Race> Race { get; set; }
    }
}
