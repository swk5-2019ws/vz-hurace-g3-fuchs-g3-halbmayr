namespace Hurace.Domain
{
    public class TimeMeasurement : DomainObjectBase
    {
        public int Measurement { get; set; }
        public int SensorId { get; set; }
        public int RaceDataId { get; set; }
    }
}
