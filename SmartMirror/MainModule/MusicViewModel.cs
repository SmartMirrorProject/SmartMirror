using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.MainModule
{
    public class MusicViewModel : BindableBase
    {
        private string artist;
        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                OnPropertyChanged();
            }
        }

        private string track;
        public string Track
        {
            get { return track; }
            set
            {
                track = value;
                OnPropertyChanged();
            }
        }
    }
}
