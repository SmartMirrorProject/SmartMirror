using System;
using SmartMirror.LocationModule;

namespace SmartMirror.WeatherModule.Models
{
    public class WeatherCurrent
    {
        public double Temperature { get; set; }
        public double HighTemperature { get; set; }
        public double LowTemperature { get; set; }
        public Location WeatherLocation { get; set; }
        public string WeatherType { get; set; }
        public string IconId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ReadTime { get; set; }

        public WeatherCurrent(DateTime readTime)
        {
            ReadTime = readTime;
        }

        public WeatherCurrent(int temp, int high, int low, string weather, 
            string icon, DateTime date, DateTime readTime)
        {
            Temperature = temp;
            HighTemperature = high;
            LowTemperature = low;
            WeatherType = weather;
            IconId = icon;
            Date = date;
            ReadTime = readTime;
        }
    }
}
