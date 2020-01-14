using Hurace.Core.BL;
using Hurace.Core.Logging.Extensions;
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
    public class RaceTypeController : ControllerBase
    {
        private readonly ILogger<RaceTypeController> logger;
        private readonly IInformationManager informationManager;

        public RaceTypeController(ILogger<RaceTypeController> logger, IInformationManager informationManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns all race types")]
        public async Task<ActionResult<IEnumerable<Domain.RaceType>>> GetAllRaceTypes()
        {
#if DEBUG
            logger.LogCall();
#endif

            return Ok(await informationManager.GetAllRaceTypesAsync()
                .ConfigureAwait(false));
        }
    }
}