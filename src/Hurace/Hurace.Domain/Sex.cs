using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Sex : DomainObjectBase
    {
        public string Label { get; set; }
        public List<int> SkierIds { get; }
    }
}
