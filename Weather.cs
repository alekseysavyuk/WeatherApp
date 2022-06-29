using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class Weather
    {
        public static string City { get; set; }

        public static string Api { get; private set; } = "C9LPZZ6EUF46T6YUL6RWFZ5KJ";
        private string Address { get; set; } = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{City}?unitGroup=us&key={Api}&contentType=json";

        private List<WeatherInfo> WeatherList { get; set; } = new List<WeatherInfo>(16);

        private async Task GetWeather()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, Address);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            dynamic weather = JsonConvert.DeserializeObject(body);

            foreach (var day in weather.days)
            {
                WeatherInfo weatherTemp = new WeatherInfo()
                {
                    City = City,
                    Description = day.description,
                    TempMax = (day.tempmax - 32) * 0.55,
                    TempMin = (day.tempmin - 32) * 0.55,
                    DateTime = day.datetime
                };

                WeatherList.Add(weatherTemp);
            }
        }

        public async Task<string> GetStringWeatherToBot(int days)
        {
            await GetWeather();
            
            StringBuilder builder = new StringBuilder();

            if (days > 14) days = 14;

            for (int i = 0; i < days; i++)
                builder.Append(WeatherList[i].ToString());

            return builder.ToString();
        }
    }
}