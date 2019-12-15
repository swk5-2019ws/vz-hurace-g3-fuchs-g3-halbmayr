using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.BL
{
    public delegate void OnTimeMeasured(Domain.Race race, Domain.Skier skier, Domain.TimeMeasurement measurement);

    public interface IRaceExecutionManager
    {
        Domain.Race TrackedRace { get; }
        Domain.Skier TrackedSkier { get; }

        event OnTimeMeasured OnTimeMeasured;

        void StartTimeTracking(Domain.Race race, Domain.Skier skier);
    }
}
