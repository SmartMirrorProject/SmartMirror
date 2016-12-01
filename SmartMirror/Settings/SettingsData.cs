using SmartMirror.LocationModule;

namespace SmartMirror.Settings
{
    class SettingsData
    {
        public SettingsData()
        {

        }

        public bool? NewSerialOption { get; set; }

        public bool? MilitaryTime { get; set; }

        public Address HomeAddress { get; set; }

        public Address WorkAddress { get; set; }

        public string StartTime { get; set; }
    }
}
