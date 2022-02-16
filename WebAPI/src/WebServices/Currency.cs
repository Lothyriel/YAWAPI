using Newtonsoft.Json.Linq;

namespace WebAPI.WebServices
{
    public class Currency : WebService
    {
        public const string CurrencyAPIAddress = "https://economia.awesomeapi.com.br/last/";

        public static async Task<Dictionary<string, string>> GetQuotation(string currencyIsoCode)
        {
            var response = await HttpClient.GetAsync($"{CurrencyAPIAddress}{currencyIsoCode}");

            if (!response.IsSuccessStatusCode)
            {
                return new Dictionary<string, string>
                {
                    [currencyIsoCode.ToUpper()] = "Currency not found!"
                };
            }

            var stringResponse = await response.Content.ReadAsStringAsync();
            var objResponse = JObject.Parse(stringResponse);
            var currencyData = objResponse[$"{currencyIsoCode.ToUpper()}BRL"]!;
            var currencyValue = currencyData["ask"]!.ToObject<double>();

            return new Dictionary<string, string>
            {
                [currencyIsoCode.ToUpper()] = $"{currencyValue} BRL"
            };
        }
    }
}