using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
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
        //declare some variables
        Dictionary<string, int> scoresAndName = new Dictionary<string, int>(); 
        TextBlock nameTextBlock;
        TextBlock scoreTextBlock;
        Grid scoresGrid;
        int fontSize;


        public HighScores()
        {
            this.InitializeComponent();

            //set font size depending on screen width
            if (Window.Current.Bounds.Height < 600) fontSize = 15;
            else fontSize = 30;

            //check to see if there is data stored for scores
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("All Score Data"))
            {
                //save this stored data to a variable
                var allScores = ApplicationData.Current.LocalSettings.Values["All Score Data"] as ApplicationDataCompositeValue;

                //grab the overall size key value of the scores array
                int count = (int)allScores["size"];

                //loop through each score data and store them to local array variable
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
             //save navigation state and set back button event up
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += backButton_Tapped;
     
            //check to see if this is a new page load
            if (e.NavigationMode == NavigationMode.New)
            {
                //prepare local variable to hold new score data
                var newScore = new ApplicationDataCompositeValue();

                //check to see if new score data exists in storage
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("New Score Data"))
                {
                    //set the variablel to new score data
                    newScore = ApplicationData.Current.LocalSettings.Values["New Score Data"] as ApplicationDataCompositeValue;

                    //check to see if the same name has been entered twice
                    if (scoresAndName.ContainsKey(newScore["name"].ToString())){

                        //grab the current score from this already stored name
                        int checkScore = scoresAndName[newScore["name"].ToString()];
                        
                        //compare scores to see if new score is higher
                        if (checkScore < (int)newScore["score"])
                        {
                            //save new score
                            scoresAndName[newScore["name"].ToString()] = (int)newScore["score"];
                            ApplicationData.Current.LocalSettings.Values.Remove("New Score Data");
                        }
                    }
                    else {
                        //ignore new score
                        scoresAndName.Add(newScore["name"].ToString(), (int)newScore["score"]);
                        ApplicationData.Current.LocalSettings.Values.Remove("New Score Data");
                    }
                }
                //update score storage
                updateScoresStorage();
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //close back button and event
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            currentView.BackRequested -= backButton_Tapped;
            base.OnNavigatedFrom(e);
        }

        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {   //go back
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }

        }

        private void updateLeaderboard()
         {   
            //initialize a count var and grid
            int count = 0;
            scoresGrid = new Grid();

            //sort the stored scores data from highest to lowest score
            var sortedScoreAndNames = from pair in scoresAndName
                                      orderby pair.Value descending
                                      select pair;

            //loop through each score
            foreach (var pair in sortedScoreAndNames)
            {
                //create new grid row to hold score and set height
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(40);
                scoresGrid.RowDefinitions.Add(row);

                //create new textblocks and style accordingly
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

            //add the grid to the scroll board
            scrollBoard.Content = scoresGrid;
        }

        private void updateScoresStorage()
        {
            //create local var to set new scores array
            var allScores = new ApplicationDataCompositeValue();
            int count = 0;

            //set a size value to record array count
            allScores["size"] = scoresAndName.Count();

            //for each score add key and value to array
            foreach (var pair in scoresAndName)
            {
                allScores["name_" + count] = pair.Key;
                allScores["score_" + count] = pair.Value;
                count++;
            }

            //remove current stored scores data and reset it
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("All Score Data")) ApplicationData.Current.LocalSettings.Values.Remove("All Score Data");
            ApplicationData.Current.LocalSettings.Values["All Score Data"] = allScores;

            //update the leadboard with the master scores
            updateLeaderboard();
        }


        //handle navigation buttons
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
