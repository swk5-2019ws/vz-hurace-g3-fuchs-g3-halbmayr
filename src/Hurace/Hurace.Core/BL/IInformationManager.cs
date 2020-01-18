using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public interface IInformationManager
    {
        Task<IEnumerable<Domain.Country>> GetAllCountriesAsync();
        Task<Domain.Country> GetCountryByIdAsync(int id);

        Task<int> CreateOrUpdateRaceAsync(Domain.Race race);
        Task<IEnumerable<Domain.Race>> GetAllRacesAsync(
            Domain.Associated<Domain.RaceType>.LoadingType raceTypeLoadingType = Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Venue>.LoadingType venueLoadingType = Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonLoadingType = Domain.Associated<Domain.Season>.LoadingType.None);
        Task<IEnumerable<Domain.Race>> GetAllRacesOfRaceTypesAndSeasonsAsync(
            IEnumerable<int> raceTypeIdSet,
            IEnumerable<int> seasonIdSet);
        Task<Domain.Race> GetRaceByIdAsync(
            int raceId,
            Domain.Associated<Domain.RaceState>.LoadingType overallRaceStateLoadingType = Domain.Associated<Domain.RaceState>.LoadingType.Reference,
            Domain.Associated<Domain.RaceType>.LoadingType raceTypeLoadingType = Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Venue>.LoadingType venueLoadingType = Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonLoadingType = Domain.Associated<Domain.Season>.LoadingType.None,
            Domain.Associated<Domain.StartPosition>.LoadingType startListLoadingType = Domain.Associated<Domain.StartPosition>.LoadingType.None,
            Domain.Associated<Domain.Skier>.LoadingType skierLoadingType = Domain.Associated<Domain.Skier>.LoadingType.None,
            Domain.Associated<Domain.Sex>.LoadingType skierSexLoadingType = Domain.Associated<Domain.Sex>.LoadingType.None,
            Domain.Associated<Domain.Country>.LoadingType skierCountryLoadingType = Domain.Associated<Domain.Country>.LoadingType.None);

        Task DeleteRaceAsync(int raceId);
        Task GenerateSecondStartList(int raceId);

        Task<Domain.RaceData> GetRaceDataByRaceAndStartlistAndPositionAsync(
            Domain.Race race,
            bool firstStartList,
            int position,
            Domain.Associated<Domain.RaceState>.LoadingType raceStateLoadingType = Domain.Associated<Domain.RaceState>.LoadingType.Reference);
        Task<bool> UpdateRaceStateOfRaceDataAsync(Domain.RaceData raceData);
        Task<IEnumerable<Domain.RankedSkier>> GetRankedSkiersOfRaceAsync(int raceId);

        Task<IEnumerable<Domain.RaceState>> GetAllRaceStatesAsync();

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
        Task<Domain.Skier> GetSkierByStartPositionAsync(int startPositionId);
        Task<int> CreateSkierAsync(Domain.Skier skier);
        Task MarkSkierAsRemoved(int skierId);
        Task<bool> IsLastSkierOfStartList(Domain.RaceData raceData);

        Task<bool> IsNextStartPositionAsync(Domain.Race race, bool firstStartlist, int position);

        Task<Domain.Skier> GetSkierByRaceAndStartlistAndPositionAsync(Domain.Race race, bool firstStartList, int position);

        Task<Domain.TimeMeasurement> CreateTimeMeasurementAsync(int measurement, int sensorId, int raceDataId, bool isValid);
        Task<Dictionary<int, (double mean, double standardDeviation)>> CalculateNormalDistributionOfMeasumentsPerSensorAsync(
            int venueId, int raceTypeId);
        Task<Domain.TimeMeasurement> GetTimeMeasurementByRaceDataAndSensorIdAsync(int raceDataId, int sensorId);

        Task<IEnumerable<Domain.Venue>> GetAllVenuesAsync(
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None);
        Task<Domain.Venue> GetVenueByIdAsync(
            int id,
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None);

        Task<IEnumerable<Domain.StartPosition>> GetStartPositionListAsync(int raceId, bool firstStartList);

        Task<IEnumerable<Domain.Sex>> GetAllSexesAsync();
    }
}
