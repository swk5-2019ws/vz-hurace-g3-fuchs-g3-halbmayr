using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Sex
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public List<int> SkierIds { get; }
    }
}
