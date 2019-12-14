namespace Hurace.Domain
{
    public class TimeMeasurement : DomainObjectBase
    {
        public int SensorId { get; set; }
        public int Measurement { get; set; }
    }
}