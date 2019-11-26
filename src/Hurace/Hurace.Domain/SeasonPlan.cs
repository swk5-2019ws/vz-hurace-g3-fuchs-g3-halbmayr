using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Domain
{
    public class SeasonPlan : DomainObjectBase
    {
        public int VenueId { get; set; }
        public int SeasonId { get; set; }
    }
}
