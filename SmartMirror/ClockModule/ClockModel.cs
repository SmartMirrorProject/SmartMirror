using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMirror.VoiceControlModule;

namespace SmartMirror.ClockModule
{
    public class ClockModel : IVoiceControllableModule
    {
        private ClockViewModel ViewModel;
        public bool MilitaryTime { get; set; }

        public IVoiceController VoiceController { get; set; }

        public ClockModel(ClockViewModel viewModel)
        {            
            MilitaryTime = false;
            InitClockViewModel(viewModel);
            VoiceController = new ClockVoiceController("Grammar\\clockGrammar.xml", this);
        }

        private void InitClockViewModel(ClockViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.Time = DateTime.Now.ToString("h:mm");
        }

        public void UpdateTime()
        {
            ViewModel.Time = DateTime.Now.ToString(!MilitaryTime ? "h:mm" : "HH:mm");
        }

        public void HandleVoiceCommand(string format)
        {
            if (ClockCommands.FORMAT_12HR.Equals(format))
            {
                MilitaryTime = false;
                UpdateTime();
            }
            else if (ClockCommands.FORMAT_24HR.Equals(format))
            {
                MilitaryTime = true;
                UpdateTime();
            }
        }
    }
}
