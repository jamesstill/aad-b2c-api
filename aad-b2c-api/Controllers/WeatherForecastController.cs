using aad_b2c_api.Models;
using aad_b2c_api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;

namespace aad_b2c_api.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [RequiredScope(WeatherForecastReader)]
    public class WeatherForecastController : ControllerBase
    {
        const string WeatherForecastReader = "weatherforecast.reader";

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [Authorize(Policy = "WeatherForecast")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), 200)]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await Task.CompletedTask;

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}