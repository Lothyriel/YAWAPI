using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WebAPI
{
    public static class Extensions
    {
        public static Dictionary<object, object> Result(this TimeSpan span)
        {
            return new Dictionary<object, object>
            {
                ["Hours"] = span.TotalHours,
                ["Minutes"] = span.TotalMinutes,
                ["Days"] = span.TotalDays
            };
        }

        public static JObject ToJObject(this string stringResponse)
        {
            return JObject.Parse($"{{results:{stringResponse}}}");
        }

        public static string ToCelsius(this JToken kelvin)
        {
            var celsius = kelvin.ToObject<double>() - 273.15;
            return $"{celsius.ToString("00.0", CultureInfo.InvariantCulture)} °C";
        }
    }
}