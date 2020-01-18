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
        [OpenApiOperation("getAllSkiers")]
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
        [OpenApiOperation("getSkierById")]
        public async Task<ActionResult<Domain.Skier>> GetSkierById(int skierId)
        {
#if DEBUG
            logger.LogCall(new { skierId });
#endif

            try
            {
                var skier = await this.informationManager.GetSkierByIdAsync(skierId).ConfigureAwait(false);
                return Ok(skier);
            }
            catch (HuraceException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("createSkier")]
        public async Task<ActionResult<Domain.Skier>> CreateSkier(Domain.Skier skier)
        {
#if DEBUG
            logger.LogCall(skier);
#endif

            if (skier is null || skier.Sex is null || skier.Country is null)
                return BadRequest();

            try
            {
                var skierId = await this.informationManager.CreateSkierAsync(skier).ConfigureAwait(false);

                var createdSkier = await this.informationManager.GetSkierByIdAsync(skierId).ConfigureAwait(false);

                return CreatedAtAction(nameof(this.GetSkierById), new { skierId }, createdSkier);
            }
            catch (HuraceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{skierId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("updateSkier")]
        public async Task<ActionResult> UpdateSkier(int skierId, Domain.Skier skier)
        {
#if DEBUG
            logger.LogCall(new { skierId, skier });
#endif

            if (skier is null)
                return BadRequest("Passed skier is null");

            try
            {
                await this.informationManager.UpdateSkierById(skierId, skier).ConfigureAwait(false);
                return NoContent();
            }
            catch (HuraceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{skierId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("deleteSkier")]
        public async Task<ActionResult> DeleteSkier(int skierId)
        {
#if DEBUG
            logger.LogCall(new { skierId });
#endif

            try
            {
                await this.informationManager.MarkSkierAsRemoved(skierId).ConfigureAwait(false);
                return NoContent();
            }
            catch (HuraceException)
            {
                return NotFound();
            }
        }
    }
}