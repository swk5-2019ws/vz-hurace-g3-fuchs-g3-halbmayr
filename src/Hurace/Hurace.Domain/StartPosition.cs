namespace Hurace.Domain
{
    public sealed class StartPosition : DomainObjectBase
    {
        public int Position { get; set; }
        public Associated<Race> Race { get; set; }
        public Associated<Skier> Skier { get; set; }
    }
}