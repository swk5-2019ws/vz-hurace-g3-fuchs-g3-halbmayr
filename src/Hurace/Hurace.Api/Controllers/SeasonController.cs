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
        public async Task<ActionResult<IEnumerable<Domain.Season>>> GetAllRaces()
        {
            logger.LogInformation($"this is a log info");

            return Ok(await informationManager.GetAllSeasonsAsync()
                .ConfigureAwait(false));
        }
    }
}