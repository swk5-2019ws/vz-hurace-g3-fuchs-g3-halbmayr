using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public interface IRaceInformationManager
    {
        Task<IEnumerable<Domain.Country>> GetAllCountries();
        Task<Domain.Country> GetCountryById(int id);

        Task<IEnumerable<Domain.Race>> GetAllRacesAsync(bool loadAssociatedData = true);

        Task<IEnumerable<Domain.RaceType>> GetAllRaceTypesAsync();
        Task<Domain.RaceType> GetRaceTypeById(int id);

        Task<IEnumerable<Domain.Season>> GetAllSeasons();
        Task<IEnumerable<Domain.Season>> GetAllSeasonsByVenueId(int venueId);
        Task<Domain.Season> GetSeasonByDate(DateTime date);

        Task<IEnumerable<Domain.Venue>> GetAllVenuesAsync(bool loadAssociatedData = true);
        Task<Domain.Venue> GetVenueById(int id, bool loadAssociatedData = true);
    }
}
