using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMirror.WeatherModule;
using SmartMirror.WeatherModule.ViewModels;

namespace SmartMirror.MainModule
{
    class ViewModelLocator
    {
        //Constructor is private because this is class is a singleton.
        private ViewModelLocator() { }
        public static ViewModelLocator Instance { get; } = new ViewModelLocator();

        WeatherCurrentViewModel CurrentWeather;
    }
}
