

namespace Hurace.Domain
{
    public class Image
    {
        public int Id { get; set; }
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Content { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
        public int SkierIds { get; set; }
    }
}
