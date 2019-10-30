namespace Hurace.Domain
{
    public class TimeMeasurement
    {
        public int TimeMeasurementId { get; set; }
        public Race Race { get; set; }
        public StartList StartList { get; set; }
        public Skier Skier { get; set; }
        public int SensorId { get; set; }
        public RaceData Measurement { get; set; }
    }
}
