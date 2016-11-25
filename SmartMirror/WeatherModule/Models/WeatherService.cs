using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using SmartMirror.LocationModule;

namespace SmartMirror.WeatherModule.Models
{
    //TODO in the actual implementation I don't want these to be static methods.
    //This class will be a singleton injected using Unity
    class WeatherService
    {
        //--------------------------------------------------------------------------------
        //--------------------------------Current Weather---------------------------------
        //--------------------------------------------------------------------------------
        //TODO set this method up to handle issues where the read fails.
        public static WeatherCurrent FetchCurrentWeather(Location location)
        {
            //Read the weather then assign all the fields of Today
            WeatherData today = GetCurrentWeatherData(location);
            WeatherCurrent todaysWeather = new WeatherCurrent(DateTime.Now);
            if (today != null)
            {
                todaysWeather.Temperature = today.main.temp;
                todaysWeather.WeatherType = today.weather[0].main;
                todaysWeather.IconId = today.weather[0].icon;
                todaysWeather.HighTemperature = today.main.temp_max;
                todaysWeather.LowTemperature = today.main.temp_min;
                todaysWeather.WeatherLocation = location;
                todaysWeather.Date = UnixTimeStampToDateTime(today.dt);
            }
            return todaysWeather;
        }
        //--------------------------------------------------------------------------------
        //------------------------------End Current Weather-------------------------------
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        //---------------------------------Today's Weather--------------------------------
        //--------------------------------------------------------------------------------
        //TODO set this method up to handle issues where the read fails.
        public static WeatherDay FetchTodaysWether(Location location)
        {
            DaysWeatherData forecast = GetWeatherForecastData(location);
            WeatherDay today = null;
            if (forecast != null)
            {
                today = ProcessTodaysWeatherData(forecast, location);
            }
            return today;
        }

        private static WeatherDay ProcessTodaysWeatherData(DaysWeatherData daysData, Location location)
        {
            WeatherDay today = new WeatherDay(DateTime.Now)
            {
                //Store tomorrow's date as 'long date' format: "Thursday, 10 April 2008"
                Date = DateTime.Today,
                WeatherLocation = location
            };
            //Iterate through all weather data retrieved and create BasicWeather objects
            for (int i = 1; i < daysData.cnt; i++)
            {
                DateTime date = UnixTimeStampToDateTime(daysData.list[i].dt);
                //If the current result is for today, process it and store it in today
                if (ValidTimeToday(date))
                {
                    BasicWeather weather = new BasicWeather
                    {
                        Temperature = daysData.list[i].main.temp,
                        IconId = daysData.list[i].weather[0].icon,
                        WeatherType = daysData.list[i].weather[0].main,
                        Time = date
                    };
                    today.WeatherDays.Add(weather);
                }
            }
            return today;
        }

        //Check the date passed in and ensure it is today, if so return true else return false
        private static bool ValidTimeToday(DateTime date)
        {
            DateTime todayDt = DateTime.Today;
            string today = todayDt.ToString("yyyy-MM-dd");
            if (today.Equals(date.ToString("yyyy-MM-dd")))
            {
                return true;
            }
            return false;
        }
        //--------------------------------------------------------------------------------
        //-------------------------------End Today's Weather------------------------------
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        //-------------------------------Tomorrow's Weather-------------------------------
        //--------------------------------------------------------------------------------
        //TODO set this method up to handle issues where the read fails.
        public static WeatherDay FetchTomorrowsWeather(Location location)
        {
            //Read tomorrows weather data and process it into a Weather Day format
            DaysWeatherData forecast = GetWeatherForecastData(location);
            WeatherDay tomorrow = null;
            if (forecast != null)
            {
                tomorrow = ProcessTomorrowsWeatherData(forecast, location);
            }
            return tomorrow;
        }

        private static WeatherDay ProcessTomorrowsWeatherData(DaysWeatherData daysData, Location location)
        {
            WeatherDay tomorrow = new WeatherDay(DateTime.Now)
            {
                //Store tomorrow's date as 'long date' format: "Thursday, 10 April 2008"
                Date = DateTime.Today.AddDays(1),
                WeatherLocation = location
            };
            //Iterate through all weather data retrieved and create BasicWeather objects
            for (int i = 1; i < daysData.cnt; i++)
            {
                DateTime date = UnixTimeStampToDateTime(daysData.list[i].dt);
                //If the current result is for tomorrow, process it and store it in Tomorrow
                if (ValidTimeTomorrow(date))
                {
                    BasicWeather weather = new BasicWeather
                    {
                        Temperature = daysData.list[i].main.temp,
                        IconId = daysData.list[i].weather[0].icon,
                        WeatherType = daysData.list[i].weather[0].main,
                        Time = date
                    };
                    tomorrow.WeatherDays.Add(weather);
                }
            }
            return tomorrow;
        }

        //Check the date passed in and ensure it is tomorrow, if so return true else return false
        private static bool ValidTimeTomorrow(DateTime date)
        {
            DateTime tomorrowDt = DateTime.Today.AddDays(1);
            string tomorrow = tomorrowDt.ToString("yyyy-MM-dd");
            if (date.ToString("yyyy-MM-dd").Equals(tomorrow))
            {
                return true;
            }
            return false;
        }
        //--------------------------------------------------------------------------------
        //-----------------------------End Tomorrow's Weather-----------------------------
        //--------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------
        //---------------------------------Week's Weather---------------------------------
        //--------------------------------------------------------------------------------
        //TODO set this method up to handle issues where the read fails.
        public static WeatherWeek FetchWeeksWeather(Location location)
        {
            WeeksWeatherData forecast = GetWeeksWeatherForecastData(location);
            WeatherWeek week = null;
            if (forecast != null)
            {
                week = ProcessWeeksWeatherData(forecast);
            }
            return week;
        }

        private static WeatherWeek ProcessWeeksWeatherData(WeeksWeatherData daysData)
        {
            WeatherWeek week = new WeatherWeek(DateTime.Now)
            {
                //Store tomorrow's date as 'long date' format: "Thursday, 10 April 2008"
                StartDate = DateTime.Today.ToString("D"),
                EndDate = DateTime.Today.AddDays(5).ToString("D")
            };
            //Iterate through all weather data retrieved and create BasicWeather objects
            for (int i = 1; i < daysData.cnt; i++)
            {
                DateTime date = UnixTimeStampToDateTime(daysData.list[i].dt);
                //If the current result is for today, process it and store it in today
                if (ValidTimeToday(date))
                {
                    WeatherWeekDay weather = new WeatherWeekDay()
                    {
                        HighTemperature = daysData.list[i].temp.max,
                        LowTemperature = daysData.list[i].temp.min,
                        IconId = daysData.list[i].weather[0].icon,
                        WeatherType = daysData.list[i].weather[0].main,
                    };
                    week.AddDay(weather);
                }
            }
            return week;
        }
        //--------------------------------------------------------------------------------
        //-------------------------------End Week's Weather-------------------------------
        //--------------------------------------------------------------------------------

        //TODO make this method better. Should throw exceptions and shit if weather read fails
        private static WeatherData GetCurrentWeatherData(Location location)
        {
            try
            {
                //TODO implement code to get current location based on IP or data stored in Azure or user entered. Hardcoded to Orlando for now
                //Request the JSON file from OpenWeatherMaps
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                    ($"http://api.openweathermap.org/data/2.5/weather?id=" + "4167147" + "&units=imperial&appID=6eeb0ae7137df453623bb2cb436db19a"));
                HttpClient client = new HttpClient();
                var response = client.SendAsync(request).Result;
                //Parse the object from the JSON and return it
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var bytes = Encoding.Unicode.GetBytes(result);
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(WeatherData));
                        var data = (WeatherData)serializer.ReadObject(stream);
                        return data;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //TODO implement code to get current location based on IP or data stored in Azure or user entered. Hardcoded to Orlando for now
        private static DaysWeatherData GetWeatherForecastData(Location location)
        {
            try
            {
                //TODO implement code to get current location based on IP or data stored in Azure or user entered. Hardcoded to Orlando for now
                //Request the JSON file from OpenWeatherMaps
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                    ($"http://api.openweathermap.org/data/2.5/forecast?id=" + "4167147" + "&units=imperial&appID=6eeb0ae7137df453623bb2cb436db19a"));
                HttpClient client = new HttpClient();
                //Parse the object from the JSON and return it
                var response = client.SendAsync(request).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var bytes = Encoding.Unicode.GetBytes(result);
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(DaysWeatherData));
                        var data = (DaysWeatherData)serializer.ReadObject(stream);
                        return data;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static WeeksWeatherData GetWeeksWeatherForecastData(Location location)
        {
            try
            {
                //TODO implement code to get current location based on IP or data stored in Azure or user entered. Hardcoded to Orlando for now
                //Request the JSON file from OpenWeatherMaps
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                    ($"http://api.openweathermap.org/data/2.5/forecast/daily?id=" + "4167147" + "&units=imperial&appID=6eeb0ae7137df453623bb2cb436db19a"));
                HttpClient client = new HttpClient();
                //Parse the object from the JSON and return it
                var response = client.SendAsync(request).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var bytes = Encoding.Unicode.GetBytes(result);
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(WeeksWeatherData));
                        var data = (WeeksWeatherData)serializer.ReadObject(stream);
                        return data;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Convert a UnixTime value to a C# DateTime object
        private static DateTime UnixTimeStampToDateTime(double unixTime)
        {
            //Set the time to 01/01/1970 00:00:00:00
            System.DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTime).ToLocalTime();
        }
    }

    //------------------------------------------------------------------------------------------------------------
    //--------------------------------------Classes to parse JSON objects-----------------------------------------
    //--------------------------------------returned from OpenWeatherMaps-----------------------------------------
    //------------------------------------------------------------------------------------------------------------
    //A number of classes used for parsing JSON info from OpenWeatherMaps APIs.
    //Primary class for the current weather
    public class WeatherData
    {
        public TempInfo main { get; set; }
        public WeatherInfo[] weather { get; set; }
        public long dt { get; set; }
        public SysInfo sys { get; set; }
        public string name { get; set; }
    }

    //Primary class for a days Weather forecast. Used for today and tomorrow
    public class DaysWeatherData
    {
        public CityInfo city { get; set; }
        public int cnt { get; set; }
        public DaysWeatherList[] list { get; set; }
    }

    //Primary class for the weeks weather forecast
    public class WeeksWeatherData
    {
        public CityInfo city { get; set; }
        public int cnt { get; set; }
        public WeeksForecastList[] list { get; set; }
    }

    //------------------------------------------------------------------------------------------------------------
    //-------------------------------------Subclasses to match JSON layout----------------------------------------
    //------------------------------------------------------------------------------------------------------------
    public class DaysWeatherList
    {
        public long dt { get; set; }
        public TempInfo main { get; set; }
        public WeatherInfo[] weather { get; set; }
        public string dt_text { get; set; }
    }

    public class WeeksForecastList
    {
        public long dt { get; set; }
        public WeeksTempInfo temp { get; set; }
        public WeatherInfo[] weather { get; set; }
    }

    public class DaysForecast
    {
        public long dt { get; set; }
        public TempInfo temp { get; set; }
        public WeatherInfo weather { get; set; }
    }

    public class WeatherInfo
    {
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class TempInfo
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }

    public class WeeksTempInfo
    {
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double morn { get; set; }
    }

    public class SysInfo
    {
        public string country { get; set; }
    }

    public class CityInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }
}