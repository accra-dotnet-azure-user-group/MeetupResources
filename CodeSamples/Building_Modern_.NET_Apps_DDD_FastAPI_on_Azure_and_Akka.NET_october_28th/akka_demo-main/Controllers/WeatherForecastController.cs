using Akka.Actor;
using Akka.Hosting;
using Demo.Api.Actors;
using Demo.Api.Actors.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IRequiredActor<WeatherProcessorActor> weatherActor) : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "India",
            "Ghana",
            "Togo",
            "Benin",
            "Nigeria",
            "Niger",
            "Chad",
            "Qatar",
            "Malta"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var results = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Country = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            var message = new WeatherForecastMessage(results);

            weatherActor.ActorRef.Tell(message);

            return results;
        }
    }
}
