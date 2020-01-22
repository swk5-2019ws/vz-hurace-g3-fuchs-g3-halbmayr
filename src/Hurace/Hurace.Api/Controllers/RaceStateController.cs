using Hurace.Core.BL;
using Hurace.Core.Debugging.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation(nameof(GetAllRaceStates))]
        public async Task<ActionResult<IEnumerable<Domain.RaceState>>> GetAllRaceStates()
        {
#if DEBUG
            logger.LogCall();
#endif

            return Ok(await informationManager.GetAllRaceStatesAsync()
                .ConfigureAwait(false));
        }
    }
}