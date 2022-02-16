using Newtonsoft.Json.Linq;

namespace WebAPI.WebServices
{
    public class Address : WebService
    {
        private const string AddressApiAddress = "https://viacep.com.br/ws/{0}/json/";
        private const string CEPApiAddress = "https://viacep.com.br/ws/{0}/{1}/{2}/json/";

        public static async Task<Dictionary<string, string>> GetCEP(string UF, string cidade, string rua)
        {
            var url = string.Format(CEPApiAddress, UF, cidade, rua);
            var response = await HttpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new Dictionary<string, string>
                {
                    ["Result"] = $"Cep Adress of {UF} {cidade} {rua} was not found"
                };
            }

            var stringResponse = await response.Content.ReadAsStringAsync();

            var objResponse = JArray.Parse(stringResponse);

            if (!objResponse.Any())
            {
                return new Dictionary<string, string>
                {
                    ["Result"] = $"Cep Adress of {UF} {cidade} {rua} was not found"
                };
            }

            var firstResult = objResponse[0];
            return new Dictionary<string, string>
            {
                ["localidade"] = firstResult["localidade"]!.ToString(),
                ["bairro"] = firstResult["bairro"]!.ToString(),
                ["logradouro"] = firstResult["logradouro"]!.ToString(),

                ["cep"] = firstResult["cep"]!.ToString(),
            };
        }

        public static async Task<Dictionary<string, string>> GetAddress(string cep)
        {
            var url = string.Format(AddressApiAddress, cep);
            var response = await HttpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new Dictionary<string, string>
                {
                    ["Result"] = $"Cep {cep} was not found"
                };
            }

            var stringResponse = await response.Content.ReadAsStringAsync();

            var objResponse = JObject.Parse(stringResponse);

            if (objResponse.TryGetValue("erro", out _))
            {
                return new Dictionary<string, string>
                {
                    ["Result"] = $"Cep {cep} was not found"
                };
            }

            return new Dictionary<string, string>
            {
                ["cep"] = cep,

                ["localidade"] = objResponse["localidade"]!.ToString(),
                ["bairro"] = objResponse["bairro"]!.ToString(),
                ["logradouro"] = objResponse["logradouro"]!.ToString(),
            };
        }
    }
}