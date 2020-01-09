using Hurace.Timer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public class RaceExecutionManager : IRaceExecutionManager
    {
        private Domain.Race trackedRace;
        private Domain.Skier trackedSkier;
        private Domain.RaceData trackedRaceData;
        private int? trackedPosition;
        private bool trackingFirstStartList;
        private Domain.Race lastTrackedRace;
        private bool lastTrackingFirstStartList;

        private readonly Semaphore triggerHandlerSem;

        private IDictionary<int, (double mean, double standardDeviation)> measurementDistributionDictionary;
        private IDictionary<int, DateTime> measurementTimeDictionary;
        private IDictionary<int, int> measurementDictionary;
        private IDictionary<int, int> bestMeasurementDictionary;

        private IRaceClock raceClock;

        private readonly IInformationManager informationManager;

        public RaceExecutionManager(IInformationManager informationManager)
        {
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));

            this.triggerHandlerSem = new Semaphore(1, 1);
            this.bestMeasurementDictionary = new Dictionary<int, int>();
        }

        public event OnTimeMeasured OnTimeMeasured;

        public IRaceClock RaceClock
        {
            get => raceClock;
            set
            {
                if (this.trackedRace is null)
                {
                    raceClock = value;
                }
                else
                {
                    throw new InvalidOperationException("Cannot replace RaceClock while tracking a skier");
                }
            }
        }

        public async Task StartTimeTrackingAsync(
            Domain.Race race,
            bool firstStartList,
            int position)
        {
            if (race is null)
                throw new ArgumentNullException(nameof(race));
            else if (this.RaceClock is null)
                throw new InvalidOperationException("Set RaceClock before tracking a race");
            else if (!await informationManager.IsNextStartPositionAsync(race, firstStartList, position).ConfigureAwait(false))
                throw new InvalidOperationException($"Position {position} is not the next position");
            else if (race.Venue is null || !race.Venue.Initialised)
                throw new ArgumentNullException($"FK of {nameof(Domain.Venue)} has to be set in passed {nameof(Domain.Race)} instance");
            else if (race.RaceType is null || !race.RaceType.Initialised)
                throw new ArgumentNullException($"FK of {nameof(Domain.RaceType)} has to be set in passed {nameof(Domain.Race)} instance");

            this.trackedRace = race;
            this.trackedPosition = position;
            this.trackingFirstStartList = firstStartList;
            this.trackedRaceData =
                await informationManager.GetRaceDataByRaceAndStartlistAndPositionAsync(race, firstStartList, position)
                    .ConfigureAwait(false);

            this.trackedSkier = await informationManager.GetSkierByRaceAndStartlistAndPositionAsync(race, firstStartList, position)
                .ConfigureAwait(false);

            var raceVenueId = race.Venue.ForeignKey ?? race.Venue.Reference.Id;
            var raceTypeId = race.RaceType.ForeignKey ?? race.RaceType.Reference.Id;
            this.measurementDistributionDictionary =
                await informationManager.CalculateNormalDistributionOfMeasumentsPerSensorAsync(
                        raceVenueId, raceTypeId)
                    .ConfigureAwait(false);

            var raceStates = await informationManager.GetAllRaceStatesAsync().ConfigureAwait(false);
            trackedRaceData.RaceState = new Domain.Associated<Domain.RaceState>(raceStates.First(rs => rs.Label == "Laufend"));

            await informationManager.UpdateRaceStateOfRaceDataAsync(trackedRaceData).ConfigureAwait(false);

            this.raceClock.TimingTriggered += OnRaceSensorTriggered;

            this.measurementDictionary = new Dictionary<int, int>();
            this.measurementTimeDictionary = new Dictionary<int, DateTime>();

            if (this.lastTrackedRace == null ||
                this.lastTrackedRace.Id != this.trackedRace.Id ||
                this.lastTrackingFirstStartList != this.trackingFirstStartList)
            {
                this.bestMeasurementDictionary = new Dictionary<int, int>();
            }
        }

        public async Task StopTimeTrackingAsync(Domain.RaceState reason)
        {
            this.raceClock.TimingTriggered -= OnRaceSensorTriggered;

            trackedRaceData.RaceState = new Domain.Associated<Domain.RaceState>(reason);
            await informationManager.UpdateRaceStateOfRaceDataAsync(trackedRaceData).ConfigureAwait(false);

            this.lastTrackedRace = this.trackedRace;
            this.lastTrackingFirstStartList = this.trackingFirstStartList;

            this.trackedRace = null;
            this.trackedSkier = null;
            this.trackedRaceData = null;
            this.trackedPosition = null;
            this.measurementTimeDictionary = null;
        }

        public async Task<bool> IsRaceStartable(int raceId)
        {
            var race = await informationManager.GetRaceByIdAsync(
                    raceId,
                    overallRaceStateLoadingType: Domain.Associated<Domain.RaceState>.LoadingType.None,
                    startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.ForeignKey)
                .ConfigureAwait(false);

            return race.Date == DateTime.Now.Date &&
                race.FirstStartList.Any();
        }
        private async void OnRaceSensorTriggered(int sensorId, DateTime measuredTime)
        {
            triggerHandlerSem.WaitOne();
            if (trackedRace is null)
                return;

            if (0 <= sensorId && sensorId < this.trackedRace.NumberOfSensors)
            {

                var wasAlreadyMeasured =
                    (await this.informationManager.GetTimeMeasurementByRaceDataAndSensorIdAsync(this.trackedRaceData.Id, sensorId)
                    .ConfigureAwait(false)) != null;

                bool newMeasurementPersistable = sensorId == 0 || (sensorId > 0 && this.measurementTimeDictionary.ContainsKey(0));
                bool isTimeMeasurementValid = !wasAlreadyMeasured;
                int difference = 0;

                if (sensorId > 0 && this.measurementTimeDictionary.ContainsKey(0))
                {
                    difference = Convert.ToInt32(
                        (measuredTime - this.measurementTimeDictionary[0]).TotalMilliseconds);

                    if (sensorId < this.trackedRace.NumberOfSensors - 1)
                    {
                        if (this.measurementDistributionDictionary.ContainsKey(sensorId))
                        {
                            //time measurements has reference measurements
                            (var mean, var stdDev) = measurementDistributionDictionary[sensorId];
                            var lowerBoundary = Statistics.NormalDistribution.CalculateLowerBoundary(mean, stdDev, 0.95);

                            isTimeMeasurementValid =
                                isTimeMeasurementValid &&
                                lowerBoundary <= difference;
                        }
                        else if (this.measurementTimeDictionary.ContainsKey(sensorId - 1))
                            isTimeMeasurementValid =
                                isTimeMeasurementValid &&
                                (measuredTime - this.measurementTimeDictionary[sensorId - 1]).TotalMilliseconds >= 500;
                        else
                            newMeasurementPersistable = false;
                    }
                }

#if DEBUG
                Debug.WriteLine(
                    $"{nameof(RaceExecutionManager)}: {sensorId} " +
                    $"{(newMeasurementPersistable && isTimeMeasurementValid ? "valid" : "invalid")}");
#endif

                if (newMeasurementPersistable)
                {
                    var newTimeMeasurement =
                       await informationManager.CreateTimeMeasurementAsync(
                           Convert.ToInt32(difference),
                           sensorId,
                           this.trackedRaceData.Id,
                           isTimeMeasurementValid)
                       .ConfigureAwait(false);

                    if (isTimeMeasurementValid || (sensorId == 0 && !this.measurementDictionary.ContainsKey(0)))
                    {
                        this.measurementTimeDictionary[sensorId] = measuredTime;
                    }

                    if (isTimeMeasurementValid)
                    {
                        this.measurementDictionary[sensorId] = difference;
                        this.measurementTimeDictionary[sensorId] = measuredTime;

                        var lastMeasurement = sensorId == this.trackedRace.NumberOfSensors - 1;

                        int bestDifference = 0;
                        if (this.bestMeasurementDictionary != null &&
                            this.bestMeasurementDictionary.ContainsKey(sensorId))
                        {
                            bestDifference = difference - this.bestMeasurementDictionary[sensorId];
                        }

                        if (lastMeasurement)
                        {
                            await this.GenerateSecondStartListIfNeeded().ConfigureAwait(false);

                            if (bestDifference <= 0)
                                this.bestMeasurementDictionary = this.measurementDictionary;

                            var raceStates = await informationManager.GetAllRaceStatesAsync().ConfigureAwait(false);

                            await this.StopTimeTrackingAsync(raceStates.First(rs => rs.Label == "Abgeschlossen"))
                                .ConfigureAwait(false);
                        }

                        await (OnTimeMeasured?.Invoke(
                                new Domain.ProcessedTimeMeasurement
                                {
                                    SensorString = $"Sensor {sensorId}",
                                    Measurement = TimeSpan.FromMilliseconds(newTimeMeasurement.Measurement),
                                    BestDifference = bestDifference != 0
                                        ? TimeSpan.FromMilliseconds(bestDifference)
                                        : TimeSpan.MaxValue
                                },
                                lastMeasurement))
                            .ConfigureAwait(false);
                    }
                }
            }
            triggerHandlerSem.Release();
        }

        public async Task GenerateSecondStartListIfNeeded()
        {
            if (this.trackingFirstStartList &&
                await this.informationManager.IsLastSkierOfStartList(this.trackedRaceData)
                    .ConfigureAwait(false))
            {
                await this.informationManager.GenerateSecondStartList(this.trackedRace.Id)
                    .ConfigureAwait(false);
            }
        }
    }
}
