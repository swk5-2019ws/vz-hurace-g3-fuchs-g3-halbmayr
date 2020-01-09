using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace Hurace.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkierController : ControllerBase
    {
        private readonly ILogger<SkierController> logger;
        private readonly IInformationManager informationManager;

        public SkierController(ILogger<SkierController> logger, IInformationManager informationManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.Skier>>> GetAllRaces()
        {
            logger.LogInformation($"this is a log info");

            return Ok(await informationManager.GetAllSkiersAsync(
                Domain.Associated<Domain.Sex>.LoadingType.Reference,
                Domain.Associated<Domain.Country>.LoadingType.Reference,
                Domain.Associated<Domain.StartPosition>.LoadingType.None)
                .ConfigureAwait(false));
        }

        [HttpGet("{raceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns skiers for the given raceId")]
        public async Task<ActionResult<IEnumerable<Domain.Skier>>> GetRankedSkiersOfRace(int raceId)
        {
            logger.LogInformation($"this is a log info");

            var skierList = await informationManager.GetRankedSkiersOfRaceAsync(raceId)
                .ConfigureAwait(false);

            return skierList == null
                ? NotFound($"Invalid raceId: {raceId}")
                : (ActionResult<IEnumerable<Domain.Skier>>)Ok(skierList);
        }
    }
}