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
using Hurace.Core.Debugging.Exceptions;

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

        [HttpGet("{skierId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Returns skier for the given skierId")]
        public async Task<ActionResult<Domain.Skier>> GetSkierById(int skierId)
        {
#if DEBUG
            logger.LogCall(new { skierId });
#endif

            var skier = await this.informationManager.GetSkierByIdAsync(skierId).ConfigureAwait(false);

            return skier == null
                ? NotFound($"Invalid skierId: {skierId}")
                : (ActionResult<Domain.Skier>)Ok(skier);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("Creates a new skier")]
        public async Task<ActionResult<IEnumerable<Domain.Race>>> CreateSkier(Domain.Skier skier)
        {
            if (skier is null || skier.Sex is null || skier.Country is null)
                return BadRequest();
#if DEBUG
            logger.LogCall(skier);
#endif

            try
            {
                var skierId = await this.informationManager.CreateSkierAsync(skier).ConfigureAwait(false);

                var createdSkier = await this.informationManager.GetSkierByIdAsync(skierId).ConfigureAwait(false);

                return CreatedAtAction(nameof(this.GetSkierById), new { skierId }, createdSkier);
            }
            catch (HuraceException e)
            {
                return BadRequest(new
                {
                    e.Message,
                    e.StackTrace
                });
            }
        }
    }
}