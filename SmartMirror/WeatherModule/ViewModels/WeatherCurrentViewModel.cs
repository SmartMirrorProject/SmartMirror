using System;
using Windows.UI.Xaml.Media.Imaging;
using SmartMirror.MainModule;
using SmartMirror.WeatherModule.Models;

namespace SmartMirror.WeatherModule.ViewModels
{
    public class WeatherCurrentViewModel : BindableBase
    {
        private WeatherCurrent weather;

        public WeatherCurrentViewModel(WeatherCurrent weather)
        {
            SetWeather(weather);
            WeatherIcon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + weather.IconId + ".png"));
        }

        public String Temperature => "" + Math.Round(weather.Temperature);

        public String HighTemp => "High: " + Math.Round(weather.HighTemperature) + "°";

        public String LowTemp => "Low: " + Math.Round(weather.LowTemperature) + "°";

        public String Location => weather.WeatherLocation.City + ", " + weather.WeatherLocation.State;

        public String Date => weather.Date.ToString("D");

        public String ReadTime => weather.ReadTime.ToString("t");

        public String WeatherType => weather.WeatherType;

        public BitmapImage WeatherIcon { get; }

        public void SetWeather(WeatherCurrent weather)
        {
            if (null == weather) throw new ArgumentException("This class cannot be passed a null WeatherCurrent object.");
            this.weather = weather;
        }
    }
}
