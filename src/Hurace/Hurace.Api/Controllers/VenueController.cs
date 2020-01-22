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
    public class VenueController : ControllerBase
    {
        private readonly ILogger<VenueController> logger;
        private readonly IInformationManager informationManager;

        public VenueController(ILogger<VenueController> logger, IInformationManager informationManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation(nameof(GetAllVenues))]
        public async Task<ActionResult<IEnumerable<Domain.Venue>>> GetAllVenues()
        {
#if DEBUG
            logger.LogCall();
#endif

            return Ok(await informationManager.GetAllVenuesAsync(
                    Domain.Associated<Domain.Country>.LoadingType.Reference,
                    Domain.Associated<Domain.Season>.LoadingType.Reference)
                .ConfigureAwait(false));
        }
    }
}