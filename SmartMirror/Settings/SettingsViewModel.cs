using Windows.UI.Xaml;
using SmartMirror.MainModule;

namespace SmartMirror.Settings
{
    public class SettingsViewModel : BindableBase
    {
        public SettingsViewModel()
        {
            Visible = Visibility.Collapsed;
            MilitaryTime = false;
            string s = "Not Init";
            HomeAddress = s;
            HomeCity = s;
            HomeState = s;
            HomeZip = s;
            WorkAddress = s;
            WorkCity = s;
            WorkState = s;
            WorkZip = s;
            WorkStartTime = s;
        }

        public SettingsViewModel(bool military, string homeA, string homeC, string homeS,
            string homeZ, string workA, string workC, string workS, string workZ, string startTime)
        {
            Visible = Visibility.Collapsed;
            MilitaryTime = military;
            HomeAddress = homeA;
            HomeCity = homeC;
            HomeState = homeS;
            HomeZip = homeZ;
            WorkAddress = workA;
            WorkCity = workC;
            WorkState = workS;
            WorkZip = workZ;
            WorkStartTime = startTime;
        }

        private Visibility visibility;
        public Visibility Visible
        {
            get { return visibility; }
            set
            {
                visibility = value;
                OnPropertyChanged();
            }
        }

        private bool? militaryTime;
        public bool? MilitaryTime
        {
            get { return (militaryTime ?? false); }
            set
            {
                militaryTime = value;
                OnPropertyChanged();
            }
        }

        private string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set
            {
                ipAddress = value;
                OnPropertyChanged();
            }
        }

        private string homeAddress;
        public string HomeAddress
        {
            get { return homeAddress; }
            set
            {
                homeAddress = value;
                OnPropertyChanged();
            }
        }

        private string homeCity;
        public string HomeCity
        {
            get { return homeCity; }
            set
            {
                homeCity = value;
                OnPropertyChanged();
            }
        }

        private string homeState;
        public string HomeState
        {
            get { return homeState; }
            set
            {
                homeState = value;
                OnPropertyChanged();
            }
        }

        private string homeZip;
        public string HomeZip
        {
            get { return homeZip; }
            set
            {
                homeZip = value;
                OnPropertyChanged();
            }
        }

        private string workAddress;
        public string WorkAddress
        {
            get { return workAddress; }
            set
            {
                workAddress = value;
                OnPropertyChanged();
            }
        }

        private string workCity;
        public string WorkCity
        {
            get { return workCity; }
            set
            {
                workCity = value;
                OnPropertyChanged();
            }
        }

        private string workState;
        public string WorkState
        {
            get { return workState; }
            set
            {
                workState = value;
                OnPropertyChanged();
            }
        }

        private string workZip;
        public string WorkZip
        {
            get
            {
                return workZip;
                
            }
            set
            {
                workZip = value;
                OnPropertyChanged();
            }
        }

        private string workStartTime;
        public string WorkStartTime
        {
            get { return workStartTime; }
            set
            {
                workStartTime = value;
                OnPropertyChanged();
            }
        }
    }
}
