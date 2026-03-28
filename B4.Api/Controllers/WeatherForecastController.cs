using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers
{
    /// <summary>
    ///    Controlador de test: Weatherforecast
    /// </summary>
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="TemperatureC"></param>
        /// <param name="Summary"></param>
        public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
        {
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }

        /// <summary>
        ///     Get /weatherforecast
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("WeatherForecast Get requested");

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var forecast = Enumerable.Range(1, 5).Select(index =>
                   new WeatherForecast
                   (
                       DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                       Random.Shared.Next(-20, 55),
                       summaries[Random.Shared.Next(summaries.Length)]
                   ))
                   .ToArray();

            _logger.LogInformation("WeatherForecast Get returned {Count} entries", forecast.Length);
            return forecast;
        }
    }
}
