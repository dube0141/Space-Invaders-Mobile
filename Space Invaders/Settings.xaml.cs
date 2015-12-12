using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Space_Invaders
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {        
            this.InitializeComponent();

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("volumeLevel"))
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                object volumeLevel = localSettings.Values["volumeLevel"];
                slider.Value = Convert.ToDouble(volumeLevel);
            }
            else
            {
                slider.Value = 100;
            }

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

        private void settingsBackBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void howBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Collapsed;
            aboutGrid.Visibility = Visibility.Collapsed;
            howGrid.Visibility = Visibility.Visible;

            settingsBtn.RequestedTheme = ElementTheme.Light;
            aboutBtn.RequestedTheme = ElementTheme.Light;
            howBtn.RequestedTheme = ElementTheme.Dark;
        }

        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Collapsed;
            aboutGrid.Visibility = Visibility.Visible;
            howGrid.Visibility = Visibility.Collapsed;

            settingsBtn.RequestedTheme = ElementTheme.Light;
            aboutBtn.RequestedTheme = ElementTheme.Dark;
            howBtn.RequestedTheme = ElementTheme.Light;
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsGrid.Visibility = Visibility.Visible;
            aboutGrid.Visibility = Visibility.Collapsed;
            howGrid.Visibility = Visibility.Collapsed;

            settingsBtn.RequestedTheme = ElementTheme.Dark;
            aboutBtn.RequestedTheme = ElementTheme.Light;
            howBtn.RequestedTheme = ElementTheme.Light;
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

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
    }
}
