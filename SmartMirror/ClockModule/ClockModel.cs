using System;
using SmartMirror.MainModule;

namespace SmartMirror.ClockModule
{
    public class ClockModel
    {
        private readonly ClockViewModel viewModel;
        private readonly SmartMirrorModel mainModel;

        public ClockModel(ClockViewModel vm, SmartMirrorModel mirror)
        {
            mainModel = mirror;
            viewModel = vm;
            UpdateTime();
        }

        public void UpdateTime()
        {
            bool military = mainModel.Settings.IsClockMilitary.Value;
            viewModel.Time = DateTime.Now.ToString(military ? "HH:mm" : "h:mm");
        }
    }
}
