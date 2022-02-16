using Newtonsoft.Json.Linq;

namespace WebAPI.src
{
    public static class AppSettings
    {
        public static readonly string  WeatherApiKey = AppConfig[nameof(WeatherApiKey)]!.ToString();

        private const string JsonPath = @"appsettings.json";

        private static JObject AppConfig => JObject.Parse(File.ReadAllText(JsonPath));
    }
}