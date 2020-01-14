using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Api.Models
{
    public class RaceFilter
    {
        public IEnumerable<int> RaceTypeIds { get; set; }
        public IEnumerable<int> SeasonIds { get; set; }
    }
}
