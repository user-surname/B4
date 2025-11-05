
using Microsoft.AspNetCore.Mvc;

///

namespace B4.Api.Controllers
{
    /// <summary>
    ///    Controlador de test: Weatherforecast
    /// </summary>
    [Route("[controller]")] // Define la ruta base como "WeatherForecast"
    public class WeatherForecastController : ControllerBase
    {
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
        [HttpGet] // Define el método HTTP para GET
        public IEnumerable<WeatherForecast> Get()
        {
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
            return forecast;
        }
    }
}
