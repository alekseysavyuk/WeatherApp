using System;
using System.Text;

namespace WeatherApp
{
    public class WeatherInfo
    {
        public string City { get; set; }
        public string Description { get; set; }
        public int TempMax { get; set; }
        public int TempMin { get; set; }
        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("===========================");
            builder.Append("\nCity: " + City + 
                           "\nDesc: " + Description + 
                           "\nMinimal Temperature: " + TempMin + 
                           "\nMaximal Temperature: " + TempMax + 
                           "\nDate: " + DateTime.ToString("D"));
            builder.Append("\n===========================\n");

            return builder.ToString();
        }
    }
}