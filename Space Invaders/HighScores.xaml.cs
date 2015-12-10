using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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
 
    public sealed partial class HighScores : Page
    {

        Dictionary<string, int> scoresAndName = new Dictionary<string, int>();
        TextBlock nameTextBlock;
        TextBlock scoreTextBlock;
        Grid scoresGrid;
        int fontSize;


        public HighScores()
        {
            this.InitializeComponent();

            if (Window.Current.Bounds.Height < 600) fontSize = 15;
            else fontSize = 30;

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("All Score Data"))
            {
                var allScores = ApplicationData.Current.LocalSettings.Values["All Score Data"] as ApplicationDataCompositeValue;

                int count = (int)allScores["size"];

                for (int i = 0; i < count; i++)
                {
                    int score = (int)allScores["score_" + i];
                    String name = allScores["name_" + i].ToString();
                    scoresAndName.Add(name, score);
                }
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
     
            if (e.NavigationMode == NavigationMode.New)
            {

                var newScore = new ApplicationDataCompositeValue();

                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("New Score Data"))
                {

                    newScore = ApplicationData.Current.LocalSettings.Values["New Score Data"] as ApplicationDataCompositeValue;

                    if (scoresAndName.ContainsKey(newScore["name"].ToString())){

                        int checkScore = scoresAndName[newScore["name"].ToString()];
                        
                        if (checkScore < (int)newScore["score"])
                        {
                            scoresAndName[newScore["name"].ToString()] = (int)newScore["score"];
                            ApplicationData.Current.LocalSettings.Values.Remove("New Score Data");
                        }
                    }
                    else {

                        scoresAndName.Add(newScore["name"].ToString(), (int)newScore["score"]);
                        ApplicationData.Current.LocalSettings.Values.Remove("New Score Data");
                    }
                }

                updateScoresStorage();
            }

            base.OnNavigatedTo(e);
        }


        private void updateLeaderboard()
        {
            int count = 0;
            scoresGrid = new Grid();

            var sortedScoreAndNames = from pair in scoresAndName
                                      orderby pair.Value descending
                                      select pair;


            foreach (var pair in sortedScoreAndNames)
            {

                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(40);
                scoresGrid.RowDefinitions.Add(row);

                nameTextBlock = new TextBlock();
                scoreTextBlock = new TextBlock();

                nameTextBlock.FontFamily = new FontFamily("Fonts/space_invaders.ttf#Space Invaders");
                nameTextBlock.FontSize = fontSize;
                nameTextBlock.Foreground = new SolidColorBrush(Colors.Yellow);
                nameTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                nameTextBlock.Text = pair.Key.ToString();
                Grid.SetRow(nameTextBlock, count);
                scoresGrid.Children.Add(nameTextBlock);

                scoreTextBlock.FontFamily = new FontFamily("Fonts/space_invaders.ttf#Space Invaders");
                scoreTextBlock.FontSize = fontSize;
                scoreTextBlock.Margin = new Thickness (0,0,20,0);
                scoreTextBlock.Foreground = new SolidColorBrush(Colors.Yellow);
                scoreTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                scoreTextBlock.Text = pair.Value.ToString();
                Grid.SetRow(scoreTextBlock, count);
                scoresGrid.Children.Add(scoreTextBlock);

                count++;
            }

            scrollBoard.Content = scoresGrid;
        }

        private void updateScoresStorage()
        {


            var allScores = new ApplicationDataCompositeValue();
            int count = 0;

            allScores["size"] = scoresAndName.Count();

            foreach (var pair in scoresAndName)
            {
                allScores["name_" + count] = pair.Key;
                allScores["score_" + count] = pair.Value;
                count++;
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("All Score Data")) ApplicationData.Current.LocalSettings.Values.Remove("All Score Data");
            ApplicationData.Current.LocalSettings.Values["All Score Data"] = allScores;

            updateLeaderboard();
        }



        private void scoresBackBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }
    }
}
