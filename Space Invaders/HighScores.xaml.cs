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
    public sealed partial class HighScores : Page
    {
        public HighScores()
        {
            this.InitializeComponent();

           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //load scores
            if (e.NavigationMode == NavigationMode.New)
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Player Score Data"))
                {
                    var playerInfo = ApplicationData.Current.LocalSettings.Values["Player Score Data"] as ApplicationDataCompositeValue;

                    TextBlock nameTextB = new TextBlock();
                    TextBlock scoreTextB = new TextBlock();
                    nameTextB.Text = playerInfo["name"].ToString();
                    nameTextB.HorizontalAlignment = HorizontalAlignment.Left;
                    scoreTextB.Text = playerInfo["score"].ToString();
                    scoreTextB.HorizontalAlignment = HorizontalAlignment.Right;
                    scoresPanel.Children.Add(nameTextB);
                    scoresPanel.Children.Add(scoreTextB);


                }
            }

            base.OnNavigatedTo(e);
        }

        private void scoresBackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }
    }
}
