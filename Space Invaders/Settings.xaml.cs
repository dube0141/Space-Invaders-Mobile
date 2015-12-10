using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Space_Invaders
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();

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
    }
}
