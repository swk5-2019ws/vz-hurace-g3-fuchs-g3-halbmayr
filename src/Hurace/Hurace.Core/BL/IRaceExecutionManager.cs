using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public delegate void OnTimeMeasured(Domain.Race race, Domain.Skier skier, Domain.TimeMeasurement measurement);

    public interface IRaceExecutionManager
    {
        event OnTimeMeasured OnTimeMeasured;

        Timer.IRaceClock RaceClock { get; set; }

        Task<bool> IsRaceStartable(int raceId);
        Task StartTimeTracking(
            Domain.Race race,
            bool firstStartList,
            int position);
    }
}
