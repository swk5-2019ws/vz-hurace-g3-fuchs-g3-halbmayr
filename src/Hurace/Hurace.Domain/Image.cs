using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public int SkierIds { get; set; }
    }
}
