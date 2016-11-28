using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using SmartMirror.LocationModule;
using SmartMirror.VoiceControlModule;
using SmartMirror.WeatherModule.ViewModels;

namespace SmartMirror.WeatherModule.Models
{
    public class WeatherModel : IVoiceControllableModule
    {
        private IWeatherViewModel currentlyVisible;
        private readonly WeatherCurrentViewModel currentWeather;
        private readonly WeatherDayViewModel todaysWeather;
        private readonly WeatherDayViewModel tomorrowsWeather;
        private readonly WeatherWeekViewModel weeksWeather;

        public IVoiceController VoiceController { get; set; }

        public WeatherModel(WeatherCurrentViewModel current, WeatherDayViewModel today,
            WeatherDayViewModel tomorrow, WeatherWeekViewModel week)
        {
            currentWeather = current;
            todaysWeather = today;
            tomorrowsWeather = tomorrow;
            weeksWeather = week;
            currentlyVisible = currentWeather;
            VoiceController = new WeatherVoiceController("Grammar\\weatherGrammar.xml", this);
        }

        public void InitWeather(Location location)
        {
            //Perform an initial Update on all the weather types using the location
            UpdateCurrentWeather(location);
            UpdateTodaysWeather(location);
            UpdateTomorrowsWeather(location);
            UpdateWeeksWeather(location);
        }

        //There is currently only one type of weather voice command
        //It is to show the weather of a certain time frame. Here we will simply
        //Update which time frame is currently visible
        public void HandleVoiceCommand(string timeFrame)
        {
            //Turn off the currently displayed element.
            currentlyVisible.Visible = Visibility.Collapsed;
            //Update currentlyVisible weather item based on the time frame from the voice command.
            switch (timeFrame)
            {
                case WeatherCommands.TIME_CURRENT:
                    currentlyVisible = currentWeather;
                    break;
                case WeatherCommands.TIME_TODAY:
                    currentlyVisible = todaysWeather;
                    break;
                case WeatherCommands.TIME_TOMORROW:
                    currentlyVisible = tomorrowsWeather;
                    break;
                case WeatherCommands.TIME_WEEK:
                    currentlyVisible = weeksWeather;
                    break;
            }
            //Turn the item that is supposed to be visible on
            currentlyVisible.Visible = Visibility.Visible;
        }

        public void UpdateCurrentWeather(Location location)
        {
            //Fetch the new weather information from WeatherService
            WeatherCurrent weather = WeatherService.FetchCurrentWeather(location);
            
            //Update the view model, currentWeather
            currentWeather.Temperature = weather.Temperature.ToString("F0");
            currentWeather.HighTemp = weather.HighTemperature.ToString("F0");
            currentWeather.LowTemp = weather.LowTemperature.ToString("F0");
            currentWeather.Location = location.City + ", " + location.State;
            currentWeather.Date = weather.Date.ToString("M");
            currentWeather.ReadTime = weather.ReadTime.ToString("g");
            currentWeather.WeatherType = weather.WeatherType;
            if (IconNeedsUpdate(currentWeather.CurrentIcon, weather.IconId))
            {
                currentWeather.WeatherIcon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + weather.IconId + ".png"));
            }
        }

        public void UpdateTodaysWeather(Location location)
        {
            WeatherDay weather = WeatherService.FetchTodaysWeather(location);
            ConvertWeatherDayToWeatherDayViewModel(weather, todaysWeather);
        }

        public void UpdateTomorrowsWeather(Location location)
        {
            WeatherDay weather = WeatherService.FetchTomorrowsWeather(location);
            ConvertWeatherDayToWeatherDayViewModel(weather, tomorrowsWeather);
        }

        private void ConvertWeatherDayToWeatherDayViewModel(WeatherDay model, WeatherDayViewModel viewModel)
        {
            List<WeatherHourViewModel> hoursList = new List<WeatherHourViewModel>();
            foreach (WeatherHour hour in model.WeatherDays)
            {
                string temp = hour.Temperature.ToString("F0");
                string time = hour.Time.ToString("h:mm tt");
                BitmapImage icon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + hour.IconId + ".png"));
                WeatherHourViewModel hourVM = new WeatherHourViewModel(time, temp, icon);
                hoursList.Add(hourVM);
            }
            viewModel.WeatherHours = hoursList;
            viewModel.Location = model.WeatherLocation.City + ", " + model.WeatherLocation.State;
            viewModel.Date = model.Date.ToString("M");
            viewModel.ReadTime = model.ReadTime.ToString("g");
        }

        public void UpdateWeeksWeather(Location location)
        {
            WeatherWeek weather = WeatherService.FetchWeeksWeather(location);
            List<WeatherWeekDayViewModel> daysList = new List<WeatherWeekDayViewModel>();
            foreach (WeatherWeekDay day in weather.WeatherDays)
            {
                string high = day.HighTemperature.ToString("F0");
                string low = day.LowTemperature.ToString("F0");
                string date = day.Date.ToString("MM/dd");
                BitmapImage icon = new BitmapImage(new Uri("ms-appx:/Assets/Weather/" + day.IconId + ".png"));
                WeatherWeekDayViewModel dayVM = new WeatherWeekDayViewModel(high, low, date, icon);
                daysList.Add(dayVM);                
            }
            weeksWeather.WeatherDays = daysList;
            weeksWeather.Location = location.City + "," + location.State;
            weeksWeather.DateRange = weather.StartDate.ToString("MM/dd") + " - " + weather.EndDate.ToString("MM/dd");
            weeksWeather.ReadTime = weather.ReadTime.ToString("g");
        }

        //Used to determine if the weather icon on the model is the same as the icon
        //on the view model. If they are different, this method returns true. Else, false.
        private bool IconNeedsUpdate(string viewModel, string model)
        {
            return !model.Equals(viewModel);
        }
    }
}