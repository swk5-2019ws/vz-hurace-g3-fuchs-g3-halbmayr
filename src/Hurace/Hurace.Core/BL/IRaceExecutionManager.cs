using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public delegate Task OnTimeMeasured(Domain.ProcessedTimeMeasurement processedMeasurement, bool lastMeasurement);

    public interface IRaceExecutionManager
    {
        event OnTimeMeasured OnTimeMeasured;

        Timer.IRaceClock RaceClock { get; set; }

        Task<bool> IsRaceStartable(int raceId);
        Task StartTimeTrackingAsync(
            Domain.Race race,
            bool firstStartList,
            int position);
        Task StopTimeTrackingAsync(
            Domain.RaceState reason);
        Task GenerateSecondStartListIfNeeded();
    }
}
