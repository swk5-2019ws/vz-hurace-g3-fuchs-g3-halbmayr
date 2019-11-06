using System;
using System.Collections.Generic;
using System.Text;

namespace InsertScriptGenerator.Core
{
    internal class SeasonPlan
    {
        public int SeasonId { get; set; }
        public int VenueId { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[SeasonPlan] ([SeasonId], [VenueId]) "
                + $"VALUES ({SeasonId}, {VenueId});";
        }
    }
}
