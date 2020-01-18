using Hurace.Core.BL;
using Hurace.Core.Debugging.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        private readonly ILogger<RaceController> logger;
        private readonly IInformationManager informationManager;

        public RaceController(ILogger<RaceController> logger, IInformationManager informationManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns all races")]
        public async Task<ActionResult<IEnumerable<Domain.Race>>> GetAllRaces()
        {
#if DEBUG
            logger.LogCall();
#endif

            return Ok(await informationManager.GetAllRacesAsync(
                    raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                    venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                    seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.None)
                .ConfigureAwait(false));
        }

        [HttpGet("{raceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns race for the given raceId")]
        public async Task<ActionResult<Domain.Race>> GetRaceById(int raceId)
        {
#if DEBUG
            logger.LogCall(new { raceId });
#endif

            var race = await informationManager.GetRaceByIdAsync(
                    raceId,
                    venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                    seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference,
                    raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference)
                .ConfigureAwait(true);

            return race == null
                ? NotFound($"Invalid raceId: {raceId}")
                : (ActionResult<Domain.Race>)Ok(race);
        }

        [HttpGet("{raceId}/ranks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns ranked skiers of specific race")]
        public async Task<ActionResult<IEnumerable<Domain.RankedSkier>>> GetRankedSkiersOfRace(int raceId)
        {
#if DEBUG
            logger.LogCall(new { raceId });
#endif

            try
            {
                var skierList = await informationManager.GetRankedSkiersOfRaceAsync(raceId).ConfigureAwait(true);

                return Ok(skierList);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"RaceId '{raceId}' not found");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns ranked skiers of specific race")]
        public async Task<ActionResult<IEnumerable<Domain.Race>>> GetRacesByFilter(Models.RaceFilter raceFilter)
        {
#if DEBUG
            logger.LogCall(new { raceFilter?.RaceTypeIds, raceFilter?.SeasonIds });
#endif

            return Ok(await informationManager.GetAllRacesOfRaceTypesAndSeasonsAsync(raceFilter?.RaceTypeIds, raceFilter?.SeasonIds)
                .ConfigureAwait(false));
        }
    }
}