//TODO this shit is terrible, this all needs to be changed. just trying to verify that the models and the service work

using SmartMirror.WeatherModule.Models;

namespace SmartMirror.WeatherModule
{
    class WeatherViewModel
    {
        private WeatherModel weatherModel;

        public WeatherViewModel(WeatherModel model)
        {
            weatherModel = model;
        }


        //----------------------- Current Weather -----------------------
        public string GetCurrentWeatherTemperature()
        {
            return "Temperature: " + weatherModel.CurrentWeather.Temperature;
        }

        public string GetCurrentWeatherHighTemperature()
        {
            return "High: " + weatherModel.CurrentWeather.HighTemperature;
        }

        public string GetCurrentWeatherLowTemperature()
        {
            return "Low: " + weatherModel.CurrentWeather.LowTemperature;
        }

//        public string GetCurrentWeatherLocation()
//        {
//            return "Location: " + weatherModel.CurrentWeather.City + ", " +
//                                    weatherModel.CurrentWeather.State + ", " +
//                                    weatherModel.CurrentWeather.Country;
//        }

        public string GetCurrentWeatherIconId()
        {
            return "IconID: " + weatherModel.CurrentWeather.IconId;
        }

        public string GetCurrentWeatherWeatherType()
        {
            return "Weather: " + weatherModel.CurrentWeather.WeatherType;
        }

        public string GetCurrentWeatherDate()
        {
            return "Date: " + weatherModel.CurrentWeather.Date.ToString("f");
        }

        public string GetCurrentWeatherReadTime()
        {
            return "Weather Read Time: " + weatherModel.CurrentWeather.ReadTime.ToString("g");
        }

        //----------------------- Todays Weather ------------------------


        //---------------------- Tomorrows Weather ----------------------


        //------------------------ Weeks Weather ------------------------

    }
}