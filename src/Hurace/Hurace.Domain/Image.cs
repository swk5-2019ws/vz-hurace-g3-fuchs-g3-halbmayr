

namespace Hurace.Domain
{
    public class Image : DomainObjectBase
    {
        public byte[] Content { get; set; }
        public int SkierId { get; set; }
    }
}
