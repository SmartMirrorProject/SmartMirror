using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMirror.MainModule;

namespace SmartMirror.ClockModule
{
    public class ClockViewModel : BindableBase
    {
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

        public ClockViewModel()
        {
            Time = "Time is not initialized.";
        }

        public ClockViewModel(String time)
        {
            Time = time;
        }
    }
}
