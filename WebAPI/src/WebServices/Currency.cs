using Newtonsoft.Json.Linq;

namespace WebAPI.WebServices
{
    public class Currency : WebService
    {
        public const string CurrencyAPI = "https://economia.awesomeapi.com.br/last/";

        public static async Task<Dictionary<string, string>> GetQuotation(string currencyIsoCode)
        {
            var response = await HttpClient.GetAsync($"{CurrencyAPI}{currencyIsoCode}");

            var result = "Currency not found!";

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                var objResponse = JObject.Parse(stringResponse);
                var currencyData = objResponse[$"{currencyIsoCode.ToUpper()}BRL"]!;
                var currencyValue = currencyData["ask"]!.ToObject<double>();

                result = $"{currencyValue} BRL";
            }
            return new Dictionary<string, string>
            {
                [currencyIsoCode.ToUpper()] = result
            };
        }
    }
}