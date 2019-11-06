namespace InsertScriptGenerator.Core
{
    internal class TimeMeasurement
    {
        public int Id { get; set; }
        public int RaceDataId { get; set; }
        public int SensorId { get; set; }
        public int Measurement { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[TimeMeasurement] ([Id], [RaceDataId], [SensorId], [Measurement]) "
                + $"VALUES ({Id}, {RaceDataId}, {SensorId}, {Measurement});";
        }
    }
}
