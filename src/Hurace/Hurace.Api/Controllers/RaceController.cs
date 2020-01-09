using Hurace.Core.BL;
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
        public async Task<ActionResult<IEnumerable<Domain.Race>>> GetAllRaces()
        {
            logger.LogInformation($"this is a log info");

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
        public async Task<ActionResult<Domain.Race>> GetCountryById(int raceId)
        {
            logger.LogInformation($"this is a log info");

            var race = await informationManager.GetRaceByIdAsync(raceId)
                .ConfigureAwait(false);

            return race == null
                ? NotFound($"Invalid raceId: {raceId}")
                : (ActionResult<Domain.Race>)Ok(race);
        }
    }
}