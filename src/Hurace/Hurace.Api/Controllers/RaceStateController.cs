using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hurace.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceStateController : ControllerBase
    {
        private readonly ILogger<RaceStateController> logger;
        private readonly IInformationManager informationManager;

        public RaceStateController(ILogger<RaceStateController> logger, IInformationManager informationManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.RaceState>>> GetAllRaceStates()
        {
            logger.LogInformation($"this is a log info");

            return Ok(await informationManager.GetAllRaceStates()
                .ConfigureAwait(false));
        }
    }
}