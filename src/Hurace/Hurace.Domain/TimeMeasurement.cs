namespace Hurace.Domain
{
    public sealed class TimeMeasurement : DomainObjectBase
    {
        public int SensorId { get; set; }
        public int Measurement { get; set; }
    }
}