

namespace Hurace.Domain
{
    public class Image : DomainObjectBase
    {
        public byte[] Content { get; set; }
        public int SkierIds { get; set; }
    }
}
