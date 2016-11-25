using System;
using SmartMirror.WeatherModule.Models;

namespace SmartMirror.WeatherModule.ViewModels
{
    public class WeatherWeekViewModel
    {
        private WeatherWeek Weather;

        public WeatherWeekViewModel(WeatherWeek weather)
        {
            Weather = weather;
        }

    }
}
