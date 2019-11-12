namespace Hurace.Domain
{
    public class TimeMeasurement : DomainObjectBase
    {
        public int RaceDataId { get; set; }
        public int SensorId { get; set; }
        public int MeasurementId { get; set; }
    }
}
