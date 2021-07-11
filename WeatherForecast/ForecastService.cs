using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace WeatherForecast
{
    public class ForecastService
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly String endpointUrl = "https://goweather.herokuapp.com/weather/";

        public static async Task<IDictionary<string, string>> GetTemperatureForKievAndOdessa()
        {
            var kievTemperature = await GetCityTemperature("Kiev");
            var odessaTemperature = await GetCityTemperature("Odessa");
            
            return new Dictionary<string, string>
            {
                ["Odessa"] = odessaTemperature,
                ["Kiev"] = kievTemperature
            };
        }

        private static async Task<String> GetCityTemperature(String city)
        {
            var forecastResponse = await client.GetAsync(endpointUrl + city);
            if (!forecastResponse.IsSuccessStatusCode)
            {
                return null;
            }
            
            var forecastAsString = await forecastResponse.Content.ReadAsStringAsync();
            var temperature = JsonDocument.Parse(forecastAsString).RootElement.GetProperty("temperature").GetString();

            return temperature;
        }
    }
}