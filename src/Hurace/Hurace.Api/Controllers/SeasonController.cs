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
    public class SeasonController : ControllerBase
    {
        private readonly ILogger<SeasonController> logger;
        private readonly IInformationManager informationManager;

        public SeasonController(ILogger<SeasonController> logger, IInformationManager informationManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns all seasons")]
        public async Task<ActionResult<IEnumerable<Domain.Season>>> GetAllRaces()
        {
#if DEBUG
            logger.LogCall();
#endif

            return Ok(await informationManager.GetAllSeasonsAsync()
                .ConfigureAwait(false));
        }
    }
}