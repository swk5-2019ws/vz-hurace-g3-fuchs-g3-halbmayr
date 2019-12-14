using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Domain
{
    public class Venue : DomainObjectBase
    {
        public string Name { get; set; }
        public Country Country { get; set; }
        public IEnumerable<Season> Seasons { get; set; }
    }
}