using Hurace.Timer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public delegate void OnTimeMeasured(Domain.Race race, Domain.Skier skier, Domain.TimeMeasurement measurement);

    public interface IRaceExecutionManager
    {
        event OnTimeMeasured OnTimeMeasured;

        Domain.Race TrackedRace { get; }
        Domain.Skier TrackedSkier { get; }
        IRaceClock RaceClock { get; set; }

        Task<bool> IsRaceStartable(int raceId);
        void StartTimeTracking(IRaceClock raceClock, Domain.Race race, Domain.Skier skier);
    }
}
