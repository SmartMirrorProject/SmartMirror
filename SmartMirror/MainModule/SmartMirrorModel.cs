using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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

        private Visibility musicVisibility;
        public Visibility MusicVisibility
        {
            get { return musicVisibility; }
            set
            {
                musicVisibility = value;
                OnPropertyChanged();
            }
        }

        public IVoiceController VoiceController { get; set; }

        public SettingsModel Settings { get; }

        private MusicViewModel musicDisplay;

        private int trackNumber;
        private List<StorageFile> songs;

        public SmartMirrorModel(SettingsModel settings, MusicViewModel music)
        {
            Display = Visibility.Visible;
            TravelTimeVisibility = Visibility.Visible;
            SensorVisibility = Visibility.Collapsed;
            MusicVisibility = Visibility.Collapsed;
            Settings = settings;
            musicPlaying = false;
            musicDisplay = music;
            InitMusic();
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

        private const int LIGHT_ON = 1;
        private const int MOTION_DETECTED = 1;

        private int cyclesSinceMotion;
        private int cyclesBeforeDisplayOff = 240; //This should be a minute, based off number of milliseconds specified in main page for sensor timer.

        public void HandleMotionSensorData(int motion)
        {
            if (Display.Equals(Visibility.Collapsed))
            {
                //If the display is off but the mirror senses motion or light, turn display on
                if (!IsDisplayOn() && motion == MOTION_DETECTED)
                {
                    Display = Visibility.Visible;                
                }
            }

            //If there is no motion, increase the cycle since motion count.
            //Else set it back to 0.
            if (motion != MOTION_DETECTED)
            {
                cyclesSinceMotion += 1;
            }
            else
            {
                cyclesSinceMotion = 0;
            }

            //If the display is on and there has been no motion for a minute,
            //Turn the display off.
            if (IsDisplayOn() && cyclesSinceMotion >= cyclesBeforeDisplayOff)
            {
                Display = Visibility.Collapsed;
            }
        }

        private const int NO_GESTURE = 0;
        private const int UP_GESTURE = 1;
        private const int DOWN_GESTURE = 2;
        private const int TOP_GESTURE = 3;
        private const int BOTTOM_GESTURE = 4;

        private bool musicPlaying;

        private bool shouldWait = false;

        public void HandleGesture(int gesture)
        {
            if (gesture != NO_GESTURE)
            {
                if (gesture == UP_GESTURE)
                {
                    VolumeUp();
                }
                else if (gesture == DOWN_GESTURE)
                {
                    VolumeDown();
                }
                if (!shouldWait)
                {
                    if (gesture == TOP_GESTURE)
                    {
                        shouldWait = true;
                        if (musicPlaying)
                        {
                            PauseMusic();
                        }
                        else
                        {
                            PlayMusic();

                        }
                    }
                    if (gesture == BOTTOM_GESTURE)
                    {
                        shouldWait = true;
                        NextSong();
                    }
                }
                else
                {
                    shouldWait = false;
                }
            }
        }

        public bool IsDisplayOn()
        {
            return Display.Equals(Visibility.Visible);
        }

        private List<string> songTitles;
        private List<string> artistNames;
        private MediaElement playingSong;

        public async void InitMusic()
        {
            songTitles = new List<string>();
            songTitles.Add("Breathe");
            songTitles.Add("My Champion");
            songTitles.Add("Paranoid Android");
            songTitles.Add("Up&Up");
            artistNames = new List<string>();
            artistNames.Add("Alter Bridge");
            artistNames.Add("Alter Bridge");
            artistNames.Add("Fox Capture Plan");
            artistNames.Add("Coldplay");
            trackNumber = 0;
            songs = new List<StorageFile>();
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Music");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("breathe.mp3");
            songs.Add(file);
            file = await folder.GetFileAsync("champion.mp3");
            songs.Add(file);
            file = await folder.GetFileAsync("paranoid.mp3");
            songs.Add(file);
            file = await folder.GetFileAsync("up.mp3");
            songs.Add(file);
            var stream = await songs[0].OpenAsync(Windows.Storage.FileAccessMode.Read);
            playingSong = new MediaElement();
            playingSong.SetSource(stream, songs[0].ContentType);
            musicDisplay.Artist = artistNames[0];
            musicDisplay.Track = songTitles[0];
            PlayMusic();
        }

        public void PlayMusic()
        {
            musicPlaying = true;
            MusicVisibility = Visibility.Visible;
            playingSong?.Play();
        }

        public void PauseMusic()
        {
            musicPlaying = false;
            MusicVisibility = Visibility.Collapsed;
            playingSong?.Pause();
        }

        public async void NextSong()
        {
            trackNumber++;
            if (trackNumber >= songs.Count)
            {
                trackNumber = 0;
            }
            musicDisplay.Artist = artistNames[trackNumber];
            musicDisplay.Track = songTitles[trackNumber];
            playingSong?.Stop();
            var stream = await songs[trackNumber].OpenAsync(Windows.Storage.FileAccessMode.Read);
            playingSong?.SetSource(stream, songs[trackNumber].ContentType);
        }

        public void VolumeUp()
        {
            playingSong.Volume += 5;
        }

        public void VolumeDown()
        {
            playingSong.Volume -= 5;
        }

        public bool IsMusicPlaying()
        {
            return musicPlaying;
        }

        public bool SongEnded()
        {
            return playingSong.CurrentState != MediaElementState.Playing;
        }
    }
}
