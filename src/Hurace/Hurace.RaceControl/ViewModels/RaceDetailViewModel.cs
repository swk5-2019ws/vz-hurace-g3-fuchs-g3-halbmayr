using Hurace.Core.BL;
using Hurace.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceDetailViewModel : BaseViewModel
    {
        private readonly RaceClockResolver raceClockResolver;
        private readonly IRaceInformationManager raceInformationManager;
        private readonly IRaceExecutionManager raceExecutionManager;

        public RaceDetailViewModel(
            RaceClockResolver raceClockResolver,
            IRaceInformationManager raceInformationManager,
            IRaceExecutionManager raceExecutionManager)
        {
            this.raceClockResolver = raceClockResolver ?? throw new ArgumentNullException(nameof(raceClockResolver));
            this.raceInformationManager = raceInformationManager ?? throw new ArgumentNullException(nameof(raceInformationManager));
            this.raceExecutionManager = raceExecutionManager ?? throw new ArgumentNullException(nameof(raceExecutionManager));
        }

        public Race Race { get; set; }

        public async Task UpdateSelectedRace(int raceId)
        {
            this.Race = await this.raceInformationManager.GetRaceById(
                raceId,
                raceTypeLoadingType: Associated<RaceType>.LoadingType.Reference,
                venueLoadingType: Associated<Venue>.LoadingType.Reference,
                seasonsOfVenueLoadingType: Associated<Season>.LoadingType.Reference,
                startListLoadingType: Associated<StartPosition>.LoadingType.Reference);
        }

        public bool CanStartExecution()
        {
            return false;
        }

        public void StartExecution()
        {
            throw new NotImplementedException();
        }

        public bool CanStartSimulatedExecution()
        {
            return false;
        }

        public void StartSimulatedExecution()
        {
            throw new NotImplementedException();
        }
    }
}
