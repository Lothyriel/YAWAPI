using Newtonsoft.Json.Linq;
using WebAPI.src;
using WebAPI.WebServices;

namespace WebAPI.Domain
{
    public class Weather : WebService
    {
        private static readonly string ApiKey = AppSettings.WeatherApiKey;
        private const string CoordinatesApiAddress = "http://api.openweathermap.org/geo/1.0/direct?q={0}&appid={1}";
        private const string WeatherApiAddress = "https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&appid={2}";

        public static async Task<List<Dictionary<string, string>>> Forecast(string city)
        {
            (double lat, double lon) = await GetCityLattitude(city);

            if (lat == -1 && lon == -1)
                return new() { new() { ["City"] = $"City {city} was not found" } };

            return await GetCityWeather(lat, lon);
        }

        public static async Task<(double, double)> GetCityLattitude(string cityName)
        {
            var url = string.Format(CoordinatesApiAddress, cityName, ApiKey);

            var response = await HttpClient.GetAsync(url);

            var stringResponse = await response.Content.ReadAsStringAsync();

            var results = JArray.Parse(stringResponse);

            if (!results.Any())
            {
                return (-1d, -1d);
            }

            var cityData = results[0]!;
            var lat = cityData["lat"]!.ToObject<double>();
            var lon = cityData["lon"]!.ToObject<double>();
            return (lat, lon);
        }

        private static async Task<List<Dictionary<string, string>>> GetCityWeather(double lat, double lon)
        {
            var url = string.Format(WeatherApiAddress, lat, lon, ApiKey);

            var response = await HttpClient.GetAsync(url);

            var stringResponse = await response.Content.ReadAsStringAsync();

            var objResponse = JObject.Parse(stringResponse);

            var dailyData = objResponse["daily"]!.ToObject<JArray>();

            return dailyData!.Select(r => FilterTemperatureData(r, dailyData!.IndexOf(r))).ToList();
        }

        private static Dictionary<string, string> FilterTemperatureData(JToken dayData, int dayIndex)
        {
            var temp = dayData["temp"]!;
            var dataDate = DateTime.Now.AddDays(dayIndex);

            return new()
            {
                ["day"] = $"{ShowTwoDigits(dataDate.Day)}/{ShowTwoDigits(dataDate.Month)}",
                ["min"] = (temp["min"]!).ToCelsius(),
                ["max"] = (temp["max"]!).ToCelsius(),
                ["weather"] = dayData["weather"]![0]!["description"]!.ToString(),
            };
        }

        private static string ShowTwoDigits(int timeUnit)
        {
            return timeUnit > 9 ? $"{timeUnit}" : $"0{timeUnit}";
        }
    }
}