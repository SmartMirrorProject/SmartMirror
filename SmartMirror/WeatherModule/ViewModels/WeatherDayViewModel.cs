using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;
using SmartMirror.MainModule;
using SmartMirror.WeatherModule.Models;

namespace SmartMirror.WeatherModule.ViewModels
{
    public class WeatherDayViewModel : BindableBase
    {
        private WeatherDay weather;

        public List<WeatherDayVM> WeatherDays; 

        public WeatherDayViewModel(WeatherDay weather)
        {
            SetWeather(weather);
            WeatherDays = ConvertToViewModels(weather.WeatherDays);
        }

        public String Location => weather.WeatherLocation.City + ", " + weather.WeatherLocation.State;

        public String Date => weather.Date.ToString("D");

        public String ReadTime => weather.ReadTime.ToString("t");
        
        public void SetWeather(WeatherDay weather)
        {
            if (null == weather) throw new ArgumentException("This class cannot be passed a null WeatherCurrent object.");
            this.weather = weather;
        }

        private List<WeatherDayVM> ConvertToViewModels(List<BasicWeather> days)
        {
            List<WeatherDayVM> viewModels = new List<WeatherDayVM>();
            foreach (BasicWeather day in days)
            {
                WeatherDayVM dayVM = new WeatherDayVM(day);
                viewModels.Add(dayVM);
            }
            return viewModels;
        } 
    }

    public class WeatherDayVM
    {
        public WeatherDayVM(BasicWeather weather)
        {
            Time = weather.Time.ToString("h:mm tt");
            Temperature = "" + Math.Round(weather.Temperature) + "°";
            WeatherIcon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + weather.IconId + ".png"));
        }

        public String Time { get; }
        public String Temperature { get; }
        public BitmapImage WeatherIcon { get; }
    }
}
