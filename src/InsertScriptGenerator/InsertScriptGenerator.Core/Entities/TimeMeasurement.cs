using System;

namespace InsertScriptGenerator.Core
{
    internal class TimeMeasurement
    {
        private static Random rnd = new Random();

        public TimeMeasurement()
        {
            this.IsValid = rnd.NextDouble() > 0.9 ? "FALSE" : "TRUE";
        }

        public int Id { get; set; }
        public int RaceDataId { get; set; }
        public int SensorId { get; set; }
        public int Measurement { get; set; }
        public string IsValid { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[TimeMeasurement] ([Id], [RaceDataId], [SensorId], [Measurement], [IsValid]) "
                + $"VALUES ({Id}, {RaceDataId}, {SensorId}, {Measurement}, '{IsValid}');";
        }
    }
}
