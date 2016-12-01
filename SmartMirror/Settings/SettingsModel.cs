using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using SmartMirror.LocationModule;
using SmartMirror.TravelTimeModule;
using SmartMirror.VoiceControlModule;

namespace SmartMirror.Settings
{
    public class SettingsModel : IVoiceControllableModule
    {
        private readonly SettingsViewModel viewModel;
        private SettingsData settings;

        public IVoiceController VoiceController { get; set; }

        public TravelTimeModel TravelTime { get; set; }

        public bool? IsClockMilitary => settings.MilitaryTime;
        public string HomeAddress => settings.HomeAddress.FullAddress;
        public string WorkAddress => settings.WorkAddress.FullAddress;
        public string StartTime => settings.StartTime;

        public bool? UseNewSerialInit => settings.NewSerialOption;

        public SettingsModel(SettingsViewModel vm)
        {
            viewModel = vm;
            foreach (HostName localHostName in NetworkInformation.GetHostNames())
            {
                if (localHostName.IPInformation != null)
                {
                    if (localHostName.Type == HostNameType.Ipv4)
                    {
                        viewModel.IpAddress = localHostName.ToString();
                        break;
                    }
                }
            }
            VoiceController = new SettingsVoiceController("Grammar\\settingsGrammar.xml", this);
            UpdateSettings();
        }

        private async void UpdateSettings()
        {
            settings = await SettingsService.FetchSettingsData();
            viewModel.MilitaryTime = settings.MilitaryTime;
            viewModel.HomeAddress = settings.HomeAddress.StreetAddress;
            viewModel.HomeCity = settings.HomeAddress.City;
            viewModel.HomeState = settings.HomeAddress.State;
            viewModel.HomeZip = settings.HomeAddress.ZipCode;
            viewModel.WorkAddress = settings.WorkAddress.StreetAddress;
            viewModel.WorkCity = settings.WorkAddress.City;
            viewModel.WorkState = settings.WorkAddress.State;
            viewModel.WorkZip = settings.WorkAddress.ZipCode;
            viewModel.WorkStartTime = settings.StartTime;
        }

        public void HandleVoiceCommand(string cmd)
        {
            if (SettingsCommands.CMD_SAVE.Equals(cmd))
            {
                UpdateData();
                SettingsService.Save(settings);
                TravelTime.UpdateTravelTime(settings.HomeAddress.FullAddress, settings.WorkAddress.FullAddress);
                settings.MilitaryTime = viewModel.MilitaryTime;
                viewModel.Visible = Visibility.Collapsed;
            }
            else if (SettingsCommands.CMD_CLOSE.Equals(cmd))
            {
                UpdateSettings();
                viewModel.Visible = Visibility.Collapsed;
            }
        }

        private void UpdateData()
        {
            settings.MilitaryTime = settings.MilitaryTime;
            settings.HomeAddress = new Address(viewModel.HomeCity, viewModel.HomeState, "US", viewModel.HomeZip)
            {
                StreetAddress = viewModel.HomeAddress
            };
            settings.WorkAddress = new Address(viewModel.WorkCity, viewModel.WorkState, "US", viewModel.WorkZip)
            {
                StreetAddress = viewModel.WorkAddress
            };
            settings.StartTime = viewModel.WorkStartTime;
        }

        public void TurnOn()
        {
            UpdateSettings();
            viewModel.Visible = Visibility.Visible;
        }

        public void TurnOff()
        {
            viewModel.Visible = Visibility.Collapsed;            
        }
    }
}