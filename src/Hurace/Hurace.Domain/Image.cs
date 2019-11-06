using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Image
    {
        public int ImageId { get; set; }
        public byte[] Content { get; set; }
        public List<int> SkierIds { get; set; }
    }
}
