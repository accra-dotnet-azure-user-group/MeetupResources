using Akka.Actor;
using Demo.Api.Actors.Messages;

namespace Demo.Api.Actors
{
    public class WeatherProcessorActor : ReceiveActor
    {
        private readonly ILogger<WeatherProcessorActor> _logger;

        public WeatherProcessorActor(ILogger<WeatherProcessorActor> logger)
        {
            _logger = logger;

            ReceiveAsync<WeatherForecastMessage>(ProcessWeatherData);
        }

        private async Task ProcessWeatherData(WeatherForecastMessage message)
        {
            try
            {
                var weatherForecast = message.WeatherForecast
                .OrderByDescending(x => x.TemperatureC)
                .First();

                _logger.LogInformation("The highest temperature of {Temp} degrees Celsius was recorded in {Country}",
                    weatherForecast.TemperatureC, weatherForecast.Country);

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing the weather data");
            }
        }
    }
}
