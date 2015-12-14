using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Space_Invaders
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {        
            this.InitializeComponent();

            //check for stored volume data and set slider value
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("volumeLevel"))
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                object volumeLevel = localSettings.Values["volumeLevel"];
                slider.Value = Convert.ToDouble(volumeLevel);
            }
            else
            {   //default to full volume
                slider.Value = 100;
            }

            //check for music toggle data
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("musicOn"))
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                object musicOnBool = localSettings.Values["musicOn"];

                if (Convert.ToBoolean(musicOnBool))
                {
                    musicOn.Opacity = 1;
                    musicOff.Opacity = 0.2;
                }
                else
                {
                    musicOn.Opacity = 0.2;
                    musicOff.Opacity = 1;
                }
            }           
        }

        //handle naviation to, show back button
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += backButton_Tapped;
            base.OnNavigatedTo(e);
        }

        //handle nav from, hide back button
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            currentView.BackRequested -= backButton_Tapped;
            base.OnNavigatedFrom(e);
        }

        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private void settingsBackBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        //show how to page hide others
        private void howBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Collapsed;
            aboutGrid.Visibility = Visibility.Collapsed;
            howGrid.Visibility = Visibility.Visible;
            appBar.Visibility = Visibility.Collapsed;

            settingsBtn.RequestedTheme = ElementTheme.Light;
            aboutBtn.RequestedTheme = ElementTheme.Light;
            howBtn.RequestedTheme = ElementTheme.Dark;
        }

        //show about to page hide others
        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Collapsed;
            aboutGrid.Visibility = Visibility.Visible;
            howGrid.Visibility = Visibility.Collapsed;
            appBar.Visibility = Visibility.Visible;

            settingsBtn.RequestedTheme = ElementTheme.Light;
            aboutBtn.RequestedTheme = ElementTheme.Dark;
            howBtn.RequestedTheme = ElementTheme.Light;
        }

        //show settings to page hide others
        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Visible;
            aboutGrid.Visibility = Visibility.Collapsed;
            howGrid.Visibility = Visibility.Collapsed;
            appBar.Visibility = Visibility.Collapsed;

            settingsBtn.RequestedTheme = ElementTheme.Dark;
            aboutBtn.RequestedTheme = ElementTheme.Light;
            howBtn.RequestedTheme = ElementTheme.Light;
        }

        //new game
        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

        //clear all scores data event
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("All Score Data"))
            {
                ApplicationData.Current.LocalSettings.Values.Remove("All Score Data");
            }
        }

        private void slider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["volumeLevel"] = slider.Value;
        }

        private void musicOff_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            musicOn.Opacity = 0.2;
            musicOff.Opacity = 1;
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["musicOn"] = false;
        }

        private void musicOn_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            musicOn.Opacity = 1;
            musicOff.Opacity = 0.2;    
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["musicOn"] = true;
        }

        private async void gabeMail_Click(object sender, RoutedEventArgs e)
        {
            Uri emails = new Uri("mailto:dube0141@algonquinlive.com");
            await Windows.System.Launcher.LaunchUriAsync(emails);  

        }

        private async void mattMail_Click(object sender, RoutedEventArgs e)
        {
            Uri emails = new Uri("mailto:youn0482@algonquinlive.com");
            await Windows.System.Launcher.LaunchUriAsync(emails);

        }
    }
}
