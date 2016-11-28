using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SmartMirror.ClockModule;
using SmartMirror.LocationModule;
using SmartMirror.VoiceControlModule;
using SmartMirror.WeatherModule.Models;
using SmartMirror.WeatherModule.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartMirror
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly LocationService locationService;


        //Clock Variables
        private readonly DispatcherTimer clockTimer;
        private const long ClockRefreshRate = 1000L; //Every Second    
        public ClockModel Clock;
        public ClockViewModel CurrentTime;

        //Weather Model and ViewModels
        private readonly DispatcherTimer weatherTimer;
        private const long WeatherRefreshRate = (15L * 60L * 1000L); //Every 15 Minutes
        //Weather timer should tick 12 times before week weather refreshes. This is 3 hours.
        private int todaysWeatherCounter;
        private const int TodaysWeatherRefresh = 12;
        //Weather timer should tick 24 times before week weather refreshes. This is 6 hours.
        private int tomorrowsWeatherCounter;
        private const int TomorrowsWeatherRefresh = 24;
        //Weather timer should tick 48 times before week weather refreshes. This is 12 hours.
        private int weeksWeatherCounter;
        private const int WeeksWeatherRefresh = 48;
        public WeatherModel Weather;
        public WeatherCurrentViewModel CurrentWeather;
        public WeatherDayViewModel TodaysWeather;
        public WeatherDayViewModel TomorrowsWeather;
        public WeatherWeekViewModel WeeksWeather;

        //Voice Controller Variables
        private readonly VoiceControlModel voiceController;
        private readonly List<IVoiceControllableModule> voiceControlledModules;
        private readonly DispatcherTimer voiceControlTimer;
        private const long voiceControlRefreshRate = 500L; //Every half second

        public MainPage()
        {
            this.InitializeComponent();

            //Init all timers
            clockTimer = new DispatcherTimer();
            weatherTimer = new DispatcherTimer();
            voiceControlTimer = new DispatcherTimer();

            voiceController = VoiceControlModel.Instance;
            voiceController.InitializeSpeechRecognizer();
            voiceControlTimer.Interval = TimeSpan.FromMilliseconds(voiceControlRefreshRate);
            voiceControlTimer.Tick += VoiceControlTick;            

            voiceControlledModules = new List<IVoiceControllableModule>();

            locationService = new LocationService("Orlando", "FL", "United States", "32817");
            InitClock();
            InitWeather();

            List<IVoiceController> controllers = new List<IVoiceController>();
            foreach (IVoiceControllableModule module in voiceControlledModules)
            {
                controllers.Add(module.VoiceController);
            }
            voiceController.LoadModulesAndStartProcessor(controllers);
            voiceControlTimer.Start();
        }

        public async void MainPage_Unloaded(object sender, object args)
        {
            voiceController.UnloadSpeechRecognizer();
        }

        private void InitClock()
        {
            CurrentTime = new ClockViewModel();
            Clock = new ClockModel(CurrentTime);
            clockTimer.Interval = TimeSpan.FromMilliseconds(ClockRefreshRate);
            clockTimer.Tick += ClockTick;
            clockTimer.Start();
            voiceControlledModules.Add(Clock);
        }

        private void InitWeather()
        {
            //Create all necessary view models for weather
            CurrentWeather = new WeatherCurrentViewModel();
            TodaysWeather = new WeatherDayViewModel();
            TomorrowsWeather = new WeatherDayViewModel();
            WeeksWeather = new WeatherWeekViewModel();
            //Init the weather model and give it a reference to all view models
            Weather = new WeatherModel(CurrentWeather, TodaysWeather,
                TomorrowsWeather, WeeksWeather);
            Weather.InitWeather(locationService.DefaultLocation);
            //Set the interval, tick handler, and then start the timer.
            weatherTimer.Interval = TimeSpan.FromMilliseconds(WeatherRefreshRate);
            weatherTimer.Tick += WeatherTick;
            weatherTimer.Start();
            todaysWeatherCounter = 0;
            tomorrowsWeatherCounter = 0;
            weeksWeatherCounter = 0;
            voiceControlledModules.Add(Weather);
        }

        private void UpdateTravelTime()
        {
            if (null != locationService.HomeAddress)
            {
                if (null != locationService.WorkAddress)
                {
                    //TravelTimeModel workTravelTime = TravelTimeService(locationService.HomeAddress.Address,
                    //    locationService.WorkAddress.Address);
                }
                if (null != locationService.SchoolAddress)
                {
                    //TravelTimeModel workTravelTime = TravelTimeService(locationService.HomeAddress.Address,
                    //    locationService.SchoolAddress.Address);
                }
            }
        }

        private async void ShowAddressContentDialog(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var result = await AddressContentDialog.ShowAsync();
            //If okay was pressed, attempt to save all the data in the Text Blocks
            if (ContentDialogResult.Primary == result)
            {
                if (HomeAddress.Text.Length > 0)
                {
                    if (!locationService.HomeAddress.StreetAddress1.Equals(HomeAddress.Text))
                    {
                        UpdateTravelTime();
                    }
                    locationService.HomeAddress = new Address(HomeCity.Text, HomeState.Text, "United States",
                        HomeZipCode.Text)
                    {
                        StreetAddress1 = HomeAddress.Text
                    };

                }

                if (WorkAddress.Text.Length > 0)
                {
                    if (!locationService.WorkAddress.StreetAddress1.Equals(WorkAddress.Text))
                    {
                        UpdateTravelTime();
                    }
                    locationService.WorkAddress = new Address(WorkCity.Text, WorkState.Text, "United States",
                        WorkZipCode.Text)
                    {
                        StreetAddress1 = WorkAddress.Text
                    };
                }

                if (SchoolAddress.Text.Length > 0)
                {
                    if (!locationService.SchoolAddress.StreetAddress1.Equals(SchoolAddress.Text))
                    {
                        UpdateTravelTime();
                    }
                    locationService.SchoolAddress = new Address(SchoolCity.Text, SchoolState.Text, "United States",
                        SchoolZipCode.Text)
                    {
                        StreetAddress1 = SchoolAddress.Text
                    };
                }
            }
        }

        //        private async void ShowAddressContentDialog( object sender, RoutedEventArgs e)
        //        {
        //            Button btn = sender as Button;
        //            ContentDialog dialog = new ContentDialog()
        //            {
        //                Title = "Enter Addresses",
        //                MaxWidth = this.ActualWidth
        //            };
        //
        //            TextBlock descriptionTextBlock = new TextBlock
        //            {
        //                Text = "Enter the following addresses for mirror elements including Weather and TravelTime",
        //                TextWrapping = TextWrapping.Wrap
        //            };
        //
        //            StackPanel panel = new StackPanel();
        //            panel.Children.Add(descriptionTextBlock);
        //
        //            TextBlock homeAddressTextBlock = new TextBlock
        //            {
        //                Text = "Home Address:",
        //                TextWrapping = TextWrapping.Wrap
        //            };
        //            TextBox homeAddressTextBox = new TextBox();
        //            StackPanel homeAddressPanel = new StackPanel {Orientation = Orientation.Horizontal};
        //            homeAddressPanel.Children.Add(homeAddressTextBlock);
        //            homeAddressPanel.Children.Add(homeAddressTextBox);
        //
        //            TextBlock workAddressTextBlock = new TextBlock
        //            {
        //                Text = "Work Address:",
        //                TextWrapping = TextWrapping.Wrap
        //            };
        //            TextBox workAddressTextBox = new TextBox();
        //            StackPanel workAddressPanel = new StackPanel { Orientation = Orientation.Horizontal };
        //            workAddressPanel.Children.Add(workAddressTextBlock);
        //            workAddressPanel.Children.Add(workAddressTextBox);
        //
        //            TextBlock schoolAddressTextBlock = new TextBlock
        //            {
        //                Text = "School Address:",
        //                TextWrapping = TextWrapping.Wrap
        //            };
        //            TextBox schoolAddressTextBox = new TextBox();
        //            StackPanel schoolAddressPanel = new StackPanel { Orientation = Orientation.Horizontal };
        //            schoolAddressPanel.Children.Add(schoolAddressTextBlock);
        //            schoolAddressPanel.Children.Add(schoolAddressTextBox);
        //
        //            panel.Children.Add(homeAddressPanel);
        //            panel.Children.Add(workAddressPanel);
        //            panel.Children.Add(schoolAddressPanel);
        //
        //            dialog.Content = panel;
        //
        //            // Add Buttons
        //            dialog.PrimaryButtonText = "OK";
        //            dialog.PrimaryButtonClick += delegate
        //            {
        //                string homeAddress = homeAddressTextBox.Text;
        //                string workAddress = workAddressTextBox.Text;
        //                string schoolAddress = schoolAddressTextBox.Text;
        //                //btn.Content = "Result: OK";
        //            };
        //
        //            dialog.SecondaryButtonText = "Cancel";
        //            dialog.SecondaryButtonClick += delegate {
        //                btn.Content = "Result: Cancel";
        //            };
        //
        //            // Show Dialog
        //            var result = await dialog.ShowAsync();
        //            if (result == ContentDialogResult.None)
        //            {
        //                btn.Content = "Result: NONE";
        //            }
        //        }

        //------------------------------------------------------------------------------------------------
        //-----------------------------------------Timer Ticks--------------------------------------------
        //------------------------------------------------------------------------------------------------
        private void ClockTick(object sender, object e)
        {
            Clock.UpdateTime();
        }

        private void WeatherTick(object sender, object e)
        {
            //Current weather is updated on every tick
            Weather.UpdateCurrentWeather(locationService.DefaultLocation);
            if (todaysWeatherCounter >= TodaysWeatherRefresh)
            {
                Weather.UpdateTodaysWeather(locationService.DefaultLocation);
                todaysWeatherCounter = 0;
            }
            if (tomorrowsWeatherCounter >= TomorrowsWeatherRefresh)
            {
                Weather.UpdateTomorrowsWeather(locationService.DefaultLocation);
                tomorrowsWeatherCounter = 0;
            }
            if (weeksWeatherCounter >= WeeksWeatherRefresh)
            {
                Weather.UpdateWeeksWeather(locationService.DefaultLocation);
                weeksWeatherCounter = 0;
            }
            todaysWeatherCounter += 1;
            tomorrowsWeatherCounter += 1;
            weeksWeatherCounter += 1;
        }

        private void VoiceControlTick(object sender, object e)
        {
            foreach (IVoiceControllableModule module in voiceControlledModules)
            {
                if (module.VoiceController.HasCommands)
                {
                    module.VoiceController.ProcessVoiceCommand();
                }
            }
        }

    }
}