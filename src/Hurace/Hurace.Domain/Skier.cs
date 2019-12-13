namespace Hurace.Domain
{
    public class Skier : DomainObjectBase
    {
        public Skier(int skierId)
            : base(skierId)
        {
            PropertiesChanged = false;
        }
    }
}