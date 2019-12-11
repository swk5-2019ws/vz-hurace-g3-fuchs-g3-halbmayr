using System;

namespace Hurace.Entities
{
    public class Season : EntityObjectBase
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
