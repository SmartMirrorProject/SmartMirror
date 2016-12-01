using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMirror.MainModule;

namespace SmartMirror.TravelTimeModule
{
    public class TravelTimeViewModel : BindableBase
    {
        private string dist;
        public string Distance
        {
            get { return dist; }
            set
            {
                dist = value;
                OnPropertyChanged();
            }
            
        }

        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged();
            }

        }
    }
}
