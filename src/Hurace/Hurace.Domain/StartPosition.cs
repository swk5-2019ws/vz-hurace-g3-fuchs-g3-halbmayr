namespace Hurace.Domain
{
    public class StartPosition : DomainObjectBase
    {
        public int StartListId { get; set; }
        public int SkierId { get; set; }
        public int Position { get; set; }
    }
}
