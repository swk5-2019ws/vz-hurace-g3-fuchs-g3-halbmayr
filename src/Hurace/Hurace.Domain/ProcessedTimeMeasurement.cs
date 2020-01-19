using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Domain
{
    public sealed class ProcessedTimeMeasurement
    {
        public string SensorString { get; set; }
        public TimeSpan Measurement { get; set; }
        public TimeSpan BestDifference { get; set; }
    }
}
