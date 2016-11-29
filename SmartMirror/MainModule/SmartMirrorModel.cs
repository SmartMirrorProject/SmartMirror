using Windows.UI.Xaml;
using SmartMirror.ClockModule;
using SmartMirror.Settings;
using SmartMirror.TravelTimeModule;
using SmartMirror.VoiceControlModule;

namespace SmartMirror.MainModule
{
    public class SmartMirrorModel : BindableBase, IVoiceControllableModule
    {
        private Visibility display;
        public Visibility Display {
            get { return display; }
            set
            {
                display = value;
                OnPropertyChanged();
            }
        }

        private Visibility travelTimeVisibility;
        public Visibility TravelTimeVisibility
        {
            get { return travelTimeVisibility; }
            set
            {
                travelTimeVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility sensorVisibility;
        public Visibility SensorVisibility
        {
            get { return sensorVisibility; }
            set
            {
                sensorVisibility = value;
                OnPropertyChanged();
            }
        }

        public IVoiceController VoiceController { get; set; }

        public SettingsModel Settings { get; }

        public SmartMirrorModel(SettingsModel settings)
        {
            Display = Visibility.Visible;
            Settings = settings;
            VoiceController = new SmartMirrorVoiceController("Grammar\\smartMirrorGrammar.xml", this);
        }

        public void HandleVoiceCommand(string cmd, string module)
        {
            if (SmartMirrorCommands.CMD_ON.Equals(cmd))
            {
                if (Display.Equals(Visibility.Collapsed))
                {
                    Display = Visibility.Visible;
                }
            }
            else if (SmartMirrorCommands.CMD_OFF.Equals(cmd))
            {
                if (Display.Equals(Visibility.Visible))
                {
                    Display = Visibility.Collapsed;
                }
            }
            else if (SmartMirrorCommands.CMD_SHOW.Equals(cmd))
            {
                if (SmartMirrorCommands.SENSORS.Equals(module))
                {
                    SensorVisibility = Visibility.Visible;
                }
                else if (SmartMirrorCommands.SETTINGS.Equals(module))
                {
                    Settings.TurnOn();
                }
                else if (SmartMirrorCommands.TRAVEL.Equals(module))
                {
                    TravelTimeVisibility = Visibility.Visible;
                }
            }
            else if (SmartMirrorCommands.CMD_HIDE.Equals(cmd))
            {
                if (SmartMirrorCommands.SENSORS.Equals(module))
                {
                    SensorVisibility = Visibility.Collapsed;
                }
                else if (SmartMirrorCommands.SETTINGS.Equals(module))
                {
                    Settings.TurnOff();
                }
                else if (SmartMirrorCommands.TRAVEL.Equals(module))
                {
                    TravelTimeVisibility = Visibility.Collapsed;
                }
            }
        }
    }
}
