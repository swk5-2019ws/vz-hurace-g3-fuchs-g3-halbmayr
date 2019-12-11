namespace Hurace.Entities
{
    public class TimeMeasurement : EntityObjectBase
    {
        public int Measurement { get; set; }
        public int SensorId { get; set; }
        public int RaceDataId { get; set; }
        public bool IsValid { get; set; }
    }
}
