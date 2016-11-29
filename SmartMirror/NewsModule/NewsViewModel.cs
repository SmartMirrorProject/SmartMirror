using SmartMirror.MainModule;
using Windows.UI.Xaml;

namespace SmartMirror.NewsModule
{
    public class NewsViewModel : BindableBase
    {

        public NewsViewModel()
        {
            string s = "Not Init";
            headline1 = s;
            headline2 = s;
            headline3 = s;
            Visible = Visibility.Visible;
        }

        public NewsViewModel(string hl1, string hl2, string hl3)
        {

            headline1 = hl1;
            headline2 = hl2;
            headline3 = hl3;
            Visible = Visibility.Visible;

        }

        private string headline1;
        public string Headline1
        {
            get { return headline1; }
            set
            {
                headline1 = value;
                OnPropertyChanged();
            }
        }

        private string headline2;
        public string Headline2
        {
            get { return headline2; }
            set
            {
                headline2 = value;
                OnPropertyChanged();
            }
        }

        private string headline3;
        public string Headline3
        {
            get { return headline3; }
            set
            {
                headline3 = value;
                OnPropertyChanged();
            }
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
    }
}
