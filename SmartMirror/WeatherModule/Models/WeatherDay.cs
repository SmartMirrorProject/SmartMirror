using System;
using System.Collections.Generic;
using SmartMirror.LocationModule;

namespace SmartMirror.WeatherModule.Models
{
    public class WeatherDay
    {
        public List<BasicWeather> WeatherDays { get; }
        public Location WeatherLocation { get; set; }
        public DateTime Date { get; set; }
        public DateTime ReadTime { get; }

        public WeatherDay(DateTime dateRead)
        {
            WeatherDays = new List<BasicWeather>();
            ReadTime = dateRead;
        }
    }

    //A Basic Weather object to represent hour increments of a day    
    public class BasicWeather
    {
        public double Temperature { get; set; }
        public string IconId { get; set; }
        public string WeatherType { get; set; }
        public DateTime Time { get; set; }

        public BasicWeather()
        {
            
        }

        public BasicWeather(double temp, string weather, string icon)
        {
            Temperature = temp;
            WeatherType = weather;
            IconId = icon;
        }
    }
}
