using System;
using System.Collections.Generic;
using SmartMirror.LocationModule;

namespace SmartMirror.WeatherModule.Models
{
    public class WeatherWeek
    {
        public List<WeatherWeekDay> WeatherDays { get; }
        public Location WeatherLocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime ReadTime { get; }

        public WeatherWeek()
        {
            WeatherDays = new List<WeatherWeekDay>(5);
            ReadTime = DateTime.Now;
        }

        public WeatherWeek(DateTime dateRead)
        {
            WeatherDays = new List<WeatherWeekDay>(5);
            ReadTime = dateRead;
        }

        //TODO setup constructor with parameters once we know what we need in the service class
        //public WeatherWeek()

        public WeatherWeekDay GetDay(int day)
        {
            if (day >= 5 || day < 0)
            {
                //TODO this should throw something like InvalidArgumentException
                throw new Exception("Day value out of range; " + day + " Value must be 0->4.");
            }
            WeatherWeekDay weatherDay;
            try
            {
                weatherDay = WeatherDays[day];
            }
            catch (Exception e)
            {
                //TODO log the exception occured here before we rethrow exception
                throw e;
            }
            return weatherDay;
        }

        public void InsertDay(int day, int high, int low, string weatherType, string iconID)
        {
            WeatherWeekDay dayOfWeek = new WeatherWeekDay()
            {
                HighTemperature = high,
                LowTemperature = low,
                WeatherType = weatherType,
                IconId = iconID
            };
            WeatherDays[day] = dayOfWeek;
        }

        public void InsertDay(int day, WeatherWeekDay dayOfWeek)
        {
            WeatherDays[day] = dayOfWeek;
        }

        public void AddDay(WeatherWeekDay day)
        {
            WeatherDays.Add(day);
        }
    }

    public class WeatherWeekDay
    {
        public double HighTemperature { get; set; }
        public double LowTemperature { get; set; }
        public string IconId { get; set; }
        public string WeatherType { get; set; }
    }
}