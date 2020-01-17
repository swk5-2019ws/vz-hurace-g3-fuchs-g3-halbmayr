using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.BL;
using Hurace.Core.Logging.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace Hurace.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SexController : ControllerBase
    {
        private readonly ILogger<RaceController> logger;
        private readonly IInformationManager informationManager;

        public SexController(ILogger<RaceController> logger, IInformationManager informationManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns all sexes")]
        public async Task<ActionResult<IEnumerable<Domain.Sex>>> GetAllSexes()
        {
#if DEBUG
            logger.LogCall();
#endif

            return Ok(await informationManager.GetAllSexesAsync()
                .ConfigureAwait(false));
        }
    }
}