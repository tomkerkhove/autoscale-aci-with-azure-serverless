using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AutoscalingAzureContainerInstances.Website.Controllers
{
    /// <summary>
    /// API endpoint to check the health of the application.
    /// </summary>
    [ApiController]
    [Route("api/v1/health")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthController"/> class.
        /// </summary>
        /// <param name="healthCheckService">The service to provide the health of the API application.</param>
        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        ///     Get Health
        /// </summary>
        /// <remarks>Provides an indication about the health of the API.</remarks>
        /// <response code="200">API is healthy</response>
        /// <response code="503">API is unhealthy or in degraded state</response>
        [HttpGet(Name = "Health_Get")]
        [ProducesResponseType(typeof(HealthReport), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HealthReport), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Get()
        {
            HealthReport healthReport = await _healthCheckService.CheckHealthAsync();

            if (healthReport?.Status == HealthStatus.Healthy)
            {
                return Ok(healthReport);
            }

            return StatusCode(StatusCodes.Status503ServiceUnavailable, healthReport);
        }
    }
}
