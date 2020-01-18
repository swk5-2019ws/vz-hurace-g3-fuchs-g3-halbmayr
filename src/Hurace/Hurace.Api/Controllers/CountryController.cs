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
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> logger;
        private readonly IInformationManager informationManager;

        public CountryController(ILogger<CountryController> logger, IInformationManager informationManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("getAllCountries")]
        public async Task<ActionResult<IEnumerable<Domain.Country>>> GetAllCountries()
        {
#if DEBUG
            logger.LogCall();
#endif

            return Ok(await informationManager.GetAllCountriesAsync()
                .ConfigureAwait(false));
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [OpenApiOperation("getCountryById")]
        public async Task<ActionResult<Domain.Country>> GetCountryById(int countryId)
        {
#if DEBUG
            logger.LogCall(new { countryId });
#endif

            var country = await informationManager.GetCountryByIdAsync(countryId)
                .ConfigureAwait(false);

            return country == null
                ? NotFound($"Invalid countryId: {countryId}")
                : (ActionResult<Domain.Country>)Ok(country);
        }
    }
}