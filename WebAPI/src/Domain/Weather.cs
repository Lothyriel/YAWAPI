using Newtonsoft.Json.Linq;
using WebAPI.src;
using WebAPI.WebServices;

namespace WebAPI.Domain
{
    public class Weather : WebService
    {
        private static string ApiKey = AppSettings.WeatherApiKey;
        private const string CoordinatesApi = "http://api.openweathermap.org/geo/1.0/direct?q={0}&appid={1}";
        private const string WeatherApi = "https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&appid={2}";

        public static async Task<List<Dictionary<string, string>>> Forecast(string city)
        {
            (double lat, double lon) = await GetCityLattitude(city);

            if (lat == -1 || lon == -1)
                return new() { new() { ["City"] = $"City {city} was not found" } };

            return await GetCityWeather(lat, lon);
        }

        private static async Task<List<Dictionary<string, string>>> GetCityWeather(double lat, double lon)
        {
            var url = string.Format(WeatherApi, lat, lon, ApiKey);

            var response = await HttpClient.GetAsync(url);

            var stringResponse = await response.Content.ReadAsStringAsync();

            var objResponse = JObject.Parse(stringResponse);

            var dailyData = objResponse["daily"]!.ToObject<JArray>();

            return dailyData!.Select(r => FilterTemperatureData(r, dailyData!.IndexOf(r))).ToList();
        }

        private static Dictionary<string, string> FilterTemperatureData(JToken dayData, int dayIndex)
        {
            var temp = dayData["temp"]!;
            var today = DateTime.Now;

            return new()
            {
                ["day"] = $"{today.Day + dayIndex}/{today.Month}",
                ["min"] = temp["min"]!.ToCelsius(),
                ["max"] = temp["max"]!.ToCelsius(),
                ["weather"] = dayData["weather"]![0]!["description"]!.ToString(),
            };
        }

        public static async Task<(double, double)> GetCityLattitude(string cityName)
        {
            var url = string.Format(CoordinatesApi, cityName, ApiKey);

            var response = await HttpClient.GetAsync(url);

            var stringResponse = await response.Content.ReadAsStringAsync();

            var results = JArray.Parse(stringResponse);

            var result = (-1d, -1d);

            if (results.Any())
            {
                var cityData = results[0]!;
                var lat = cityData["lat"]!.ToObject<double>();
                var lon = cityData["lon"]!.ToObject<double>();
                result = (lat, lon);
            }
            return result;
        }
    }
}