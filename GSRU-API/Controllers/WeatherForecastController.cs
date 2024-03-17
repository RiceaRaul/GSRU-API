using GSRU_DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GSRU_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IUnitOfWork _uow) : ControllerBase
    {
        private readonly IUnitOfWork _uow = _uow;

        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _uow.Commit();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
