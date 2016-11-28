using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SmartMirror.WeatherModule.ViewModels
{
    interface IWeatherViewModel
    {
        Visibility Visible { get; set; }
        string Location { get; set; }
        string ReadTime { get; set; }
    }
}
