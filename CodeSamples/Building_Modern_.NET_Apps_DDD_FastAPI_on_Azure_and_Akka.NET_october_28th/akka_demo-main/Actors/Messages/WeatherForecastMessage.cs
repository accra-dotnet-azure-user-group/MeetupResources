namespace Demo.Api.Actors.Messages
{
    public struct WeatherForecastMessage(WeatherForecast[] weatherForecast)
    {
        public WeatherForecast[] WeatherForecast { get; set; } = weatherForecast;
    }
}
