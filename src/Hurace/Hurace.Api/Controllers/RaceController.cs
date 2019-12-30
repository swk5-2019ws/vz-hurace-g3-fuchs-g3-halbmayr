using Hurace.Core.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IInformationManager raceManager;

        public RaceController(ILogger<RaceController> logger, IInformationManager raceManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.Race>>> GetAllRaces()
        {
            logger.LogInformation($"this is a log info");

            return Ok(await raceManager.GetAllRacesAsync(
                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.None));
        }
    }
}