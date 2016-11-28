using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using SmartMirror.MainModule;
using SmartMirror.WeatherModule.Models;

namespace SmartMirror.WeatherModule.ViewModels
{
    public class WeatherCurrentViewModel : BindableBase, IWeatherViewModel
    {
        public WeatherCurrentViewModel()
        {
            string s = "Not Init";
            Visible = Visibility.Visible;
            Temperature = s;
            HighTemp = s;
            LowTemp = s;
            Location = s;
            Date = s;
            ReadTime = s;
            WeatherType = s;
            CurrentIcon = "02d"; //This is just a default.
            WeatherIcon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + CurrentIcon + ".png"));
        }

        public WeatherCurrentViewModel(string temp, string high, string low, string loc,
            string date, string read, string type, string icon)
        {
            Visible = Visibility.Visible;
            Temperature = temp;
            HighTemp = high;
            LowTemp = low;
            Location = loc;
            Date = date;
            ReadTime = read;
            WeatherType = type;
            CurrentIcon = icon;
            WeatherIcon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + icon + ".png"));
        }

        private Visibility visibility;
        public Visibility Visible
        {
            get { return visibility; }
            set
            {
                visibility = value;
                OnPropertyChanged();
            }
        }

        private string temperature;
        public string Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnPropertyChanged();
            }
        }

        private string highTemp;
        public string HighTemp
        {
            get { return highTemp; }
            set
            {
                highTemp = "Hi: " + value + "°F";
                OnPropertyChanged();
            }
        }

        private string lowTemp;
        public string LowTemp
        {
            get { return lowTemp; }
            set
            {
                lowTemp = "Low: " + value + "°F";
                OnPropertyChanged();
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged();
            }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        private string readTime;
        public string ReadTime
        {
            get { return readTime; }
            set
            {
                readTime = value;
                OnPropertyChanged();
            }
        }

        private string weatherType;
        public string WeatherType
        {
            get { return weatherType; }
            set
            {
                weatherType = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage weatherIcon;
        public BitmapImage WeatherIcon
        {
            get { return weatherIcon; }
            set
            {
                weatherIcon = value;
                OnPropertyChanged();
            }
        }

        public string CurrentIcon { get; set; }
    }
}
