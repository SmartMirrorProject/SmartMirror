using SmartMirror.MainModule;

namespace SmartMirror.SensorModule
{
    public class SensorViewModel : BindableBase
    {
        public SensorViewModel()
        {
            
        }

        private string temp;
        public string Temperature {
            get { return temp; }
            set
            {
                temp = value;
                OnPropertyChanged();
            }
        }

        private string humidity;
        public string Humidity
        {
            get { return humidity; }
            set
            {
                humidity = value;
                OnPropertyChanged();
            }
        }

        private string lightSensor;
        public string LightSensor
        {
            get { return lightSensor; }
            set
            {
                lightSensor = value;
                OnPropertyChanged();
            }
        }

        private string longRange;
        public string LongRangeSensor
        {
            get { return longRange; }
            set
            {
                longRange = value;
                OnPropertyChanged();
            }
        }

        private string shortRange;
        public string GestureControl
        {
            get { return shortRange; }
            set
            {
                shortRange = value;
                OnPropertyChanged();
            }
        }

    }
}
