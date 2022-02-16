using WebAPI;
using WebAPI.Domain;
using WebAPI.WebServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/Time/Day", (double days) => TimeSpan.FromDays(days).Result());
app.MapPost("/Time/Hour", (double hours) => TimeSpan.FromHours(hours).Result());
app.MapPost("/Time/Minute", (double minutes) => TimeSpan.FromMinutes(minutes).Result());

app.MapPost("/Area/Acre", (double acres) => Area.FromAcres(acres).Result());
app.MapPost("/Area/Hectare", (double hectares) => Area.FromHectares(hectares).Result());
app.MapPost("/Area/SquareMeter", (double meters) => Area.FromMeters(meters).Result());

app.MapPost("/Mass/Ton", (double tons) => Mass.FromTons(tons).Result());
app.MapPost("/Mass/Kilogram", (double kilograms) => Mass.FromKilograms(kilograms).Result());
app.MapPost("/Mass/Pound", (double pounds) => Mass.FromPounds(pounds).Result());

app.MapPost("/Speed/Mph", (double mph) => Speed.FromMph(mph).Result());
app.MapPost("/Speed/Kph", (double kph) => Speed.FromKph(kph).Result());

app.MapPost("/Currency", async (string currencyIsoCode) => await Currency.GetQuotation(currencyIsoCode));

app.MapPost("/Address/Name", async (string cep) => await Address.GetAddress(cep));
app.MapPost("/Address/CEP", async (string UF, string cidade, string rua) => await Address.GetCEP(UF, cidade, rua));

app.MapPost("/WeatherForecast", async (string city) => await Weather.Forecast(city));

app.Run();