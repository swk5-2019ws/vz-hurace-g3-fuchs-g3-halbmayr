using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public interface IInformationManager
    {
        Task<IEnumerable<Domain.Country>> GetAllCountriesAsync();
        Task<Domain.Country> GetCountryByIdAsync(int id);

        Task CreateOrUpdateRace(Domain.Race race);
        Task<IEnumerable<Domain.Race>> GetAllRacesAsync(
            Domain.Associated<Domain.RaceType>.LoadingType raceTypeLoadingType = Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Venue>.LoadingType venueLoadingType = Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonLoadingType = Domain.Associated<Domain.Season>.LoadingType.None);
        Task<Domain.Race> GetRaceByIdAsync(
            int raceId,
            Domain.Associated<Domain.RaceType>.LoadingType raceTypeLoadingType = Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Venue>.LoadingType venueLoadingType = Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonLoadingType = Domain.Associated<Domain.Season>.LoadingType.None,
            Domain.Associated<Domain.StartPosition>.LoadingType startListLoadingType = Domain.Associated<Domain.StartPosition>.LoadingType.None,
            Domain.Associated<Domain.Skier>.LoadingType skierLoadingType = Domain.Associated<Domain.Skier>.LoadingType.None,
            Domain.Associated<Domain.Sex>.LoadingType skierSexLoadingType = Domain.Associated<Domain.Sex>.LoadingType.None,
            Domain.Associated<Domain.Country>.LoadingType skierCountryLoadingType = Domain.Associated<Domain.Country>.LoadingType.None);

        Task<Domain.RaceData> GetRaceDataByRaceAndStartlistAndPosition(
            Domain.Race race,
            bool firstStartList,
            int position,
            Domain.Associated<Domain.RaceState>.LoadingType raceStateLoadingType = Domain.Associated<Domain.RaceState>.LoadingType.Reference);
        Task<bool> UpdateRaceData(Domain.RaceData raceData);

        Task<IEnumerable<Domain.RaceState>> GetAllRaceStates();

        Task<IEnumerable<Domain.RaceType>> GetAllRaceTypesAsync();
        Task<Domain.RaceType> GetRaceTypeByIdAsync(int id);

        Task<IEnumerable<Domain.Season>> GetAllSeasonsAsync();
        Task<IEnumerable<Domain.Season>> GetAllSeasonsByVenueIdAsync(int venueId);
        Task<Domain.Season> GetSeasonByDateAsync(DateTime date);

        Task<IEnumerable<Domain.Skier>> GetAllSkiersAsync(
            Domain.Associated<Domain.Sex>.LoadingType sexLoadingType = Domain.Associated<Domain.Sex>.LoadingType.Reference,
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.Reference,
            Domain.Associated<Domain.StartPosition>.LoadingType startPositionLoadingType = Domain.Associated<Domain.StartPosition>.LoadingType.None);

        Task<Domain.Skier> GetSkierByIdAsync(
            int skierId,
            Domain.Associated<Domain.Sex>.LoadingType sexLoadingType = Domain.Associated<Domain.Sex>.LoadingType.Reference,
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.Reference,
            Domain.Associated<Domain.StartPosition>.LoadingType startPositionLoadingType = Domain.Associated<Domain.StartPosition>.LoadingType.None);

        Task<IEnumerable<Domain.StartPosition>> GetAllStartPositionsOfStartList(
            int startListId,
            Domain.Associated<Domain.Skier>.LoadingType skierLoadingType = Domain.Associated<Domain.Skier>.LoadingType.ForeignKey);
        Task<bool> IsNextStartposition(Domain.Race race, bool firstStartlist, int position);

        Task<Domain.Skier> GetSkierByRaceAndStartlistAndPosition(Domain.Race race, bool firstStartList, int position);

        Task<Domain.TimeMeasurement> CreateTimemeasurement(int measurement, int sensorId, int raceDataId, bool isValid);
        Task<Dictionary<int, (double mean, double standardDeviation)>> CalculateNormalDistributionOfMeasumentsPerSensor(
            int venueId, int raceTypeId);

        Task<IEnumerable<Domain.Venue>> GetAllVenuesAsync(
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None);
        Task<Domain.Venue> GetVenueByIdAsync(
            int id,
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None);
    }
}
