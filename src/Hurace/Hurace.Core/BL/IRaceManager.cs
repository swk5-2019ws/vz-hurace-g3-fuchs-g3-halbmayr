using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public interface IRaceManager
    {
        Task<IEnumerable<Domain.Country>> GetAllCountries();
        Task<IEnumerable<Domain.Race>> GetAllRacesAsync();
        Task<IEnumerable<Domain.RaceType>> GetAllRaceTypesAsync();
        Task<IEnumerable<Domain.Season>> GetAllSeasons();
        Task<Domain.Season> GetSeasonByDate(DateTime date);
        Task<IEnumerable<Domain.Venue>> GetAllVenuesAsync();
    }
}
