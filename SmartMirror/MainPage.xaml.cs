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
using SmartMirror.LocationModule;
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
        private LocationService locationService;

        //Weather Model and ViewModels
        private WeatherModel weatherModel;
        public WeatherCurrentViewModel CurrentWeather;
        public WeatherDayViewModel TodaysWeather;
        public WeatherDayViewModel TomorrowsWeather;
        public WeatherWeekViewModel WeeksWeather;
        

        public MainPage()
        {
            this.InitializeComponent();
            locationService = new LocationService("Orlando", "FL", "United States", "32817");
            InitWeatherElements();
        }

        private void InitWeatherElements()
        {
            weatherModel = new WeatherModel();
            weatherModel.InitWeather(locationService.DefaultLocation);
            UpdateWeather();
        }

        private void UpdateWeather()
        {
            CurrentWeather = new WeatherCurrentViewModel(weatherModel.CurrentWeather);
            TodaysWeather = new WeatherDayViewModel(weatherModel.TodaysWeather);
            TomorrowsWeather = new WeatherDayViewModel(weatherModel.TomorrowsWeather);
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
                        UpdateWeather();
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
    }
}
