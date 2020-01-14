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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns all skiers")]
        public async Task<ActionResult<IEnumerable<Domain.Skier>>> GetAllSkiers()
        {
#if DEBUG
            logger.LogCall();
#endif

            return Ok(await informationManager.GetAllSkiersAsync(
                Domain.Associated<Domain.Sex>.LoadingType.Reference,
                Domain.Associated<Domain.Country>.LoadingType.Reference,
                Domain.Associated<Domain.StartPosition>.LoadingType.None)
                .ConfigureAwait(false));
        }
    }
}