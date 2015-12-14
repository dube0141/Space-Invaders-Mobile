using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Space_Invaders
{
    /// <summary>
    ///Mainpage, Handles navigation
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        //Handle high score button clicks
        private void highScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HighScores));
        }

        //Handle settings butten click
        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        //Handle play game button click
        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }
    } 
}
