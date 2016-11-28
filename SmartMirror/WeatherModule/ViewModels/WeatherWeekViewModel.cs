using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using SmartMirror.MainModule;
using SmartMirror.WeatherModule.Models;

namespace SmartMirror.WeatherModule.ViewModels
{
    public class WeatherWeekViewModel : BindableBase, IWeatherViewModel
    {
        public WeatherWeekViewModel()
        {
            string s = "Not Init";
            Visible = Visibility.Collapsed;
            Location = s;
            ReadTime = s;

            //Create a default list with a single weather hour in it to avoid exceptions.
            string currentIcon = "02d"; //This is just a default.
            BitmapImage weatherIcon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + currentIcon + ".png"));
            List<WeatherWeekDayViewModel> defaultList = new List<WeatherWeekDayViewModel>
                { new WeatherWeekDayViewModel(s, s, s, weatherIcon) };
            WeatherDays = defaultList;
        }

        public WeatherWeekViewModel(string loc, string read, List<WeatherWeekDayViewModel> days)
        {
            Visible = Visibility.Collapsed;
            Location = loc;
            ReadTime = read;
            WeatherDays = days;
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

        private List<WeatherWeekDayViewModel> weatherDays;
        public List<WeatherWeekDayViewModel> WeatherDays
        {
            get { return weatherDays; }
            set
            {
                weatherDays = value;
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

        private string dateRange;
        public string DateRange
        {
            get { return dateRange; }
            set
            {
                dateRange = value;
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

    public class WeatherWeekDayViewModel
    {
        public WeatherWeekDayViewModel(string high, string low, string date, BitmapImage icon)
        {
            HighTemp = high;
            LowTemp = low;
            Date = date;
            WeatherIcon = icon;
        }

        public string HighTemp { get; set; }
        public string LowTemp { get; set; }
        public string Date { get; set; }
        public BitmapImage WeatherIcon { get; set; }
    }
}
