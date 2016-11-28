using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using SmartMirror.MainModule;
using SmartMirror.WeatherModule.Models;

namespace SmartMirror.WeatherModule.ViewModels
{
    public class WeatherDayViewModel : BindableBase, IWeatherViewModel
    {
        public WeatherDayViewModel()
        {
            string s = "Not Init";
            Visible = Visibility.Collapsed;
            Location = s;
            Date = s;
            ReadTime = s;
            
            //Create a default list with a single weather hour in it to avoid exceptions.
            string currentIcon = "02d"; //This is just a default.
            BitmapImage weatherIcon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + currentIcon + ".png"));
            List<WeatherHourViewModel> defaultList = new List<WeatherHourViewModel> { new WeatherHourViewModel(s, s, weatherIcon) };
            WeatherHours = defaultList;
        }

        public WeatherDayViewModel(string loc, string date, string read, List<WeatherHourViewModel> hours)
        {
            Visible = Visibility.Collapsed;
            Location = loc;
            Date = date;
            ReadTime = read;
            WeatherHours = hours;
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

        private List<WeatherHourViewModel> weatherHours;
        public List<WeatherHourViewModel> WeatherHours
        {
            get { return weatherHours; }
            set
            {
                weatherHours = value;
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
    }

    public class WeatherHourViewModel
    {
        public WeatherHourViewModel(string time, string temp, BitmapImage icon)
        {
            Time = time;
            Temperature = temp;
            WeatherIcon = icon;
        }

        public string Temperature { get; set; }
        public string Time { get; set; }
        public BitmapImage WeatherIcon { get; set; }
    }
}
