using System;

namespace Hurace.Domain
{
    public class Season : DomainObjectBase
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}