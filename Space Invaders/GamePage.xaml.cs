using System;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Space_Invaders
{
    public sealed partial class GamePage : Page
    {       
        private DispatcherTimer dispatcherTimer;

        private Player player;
        private Invaders invaders;
        private Sounds sounds;

        private ushort playerLives;
        private int playerGameScore;
        private int level;

        public GamePage()
        {
            this.InitializeComponent();

            //set preffered window size
            ApplicationView.PreferredLaunchViewSize = new Size(700, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            //initialize variable for gameplay
            playerLives = 3;
            playerGameScore = 0;
            level = 1;
            
            //initiate classes
            player = new Player(canvas, playerGameScore);
            invaders = new Invaders(canvas, level);
            sounds = new Sounds(grid);

            //start the game dispatch timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Game;
            dispatcherTimer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30);
            dispatcherTimer.Start();
        }

        //game method that fires in dispatch timer
        private void Game(object sender, object e)
        {
            //check to see if player has died
            if (!invaders.playerAlive())
            {
                //get the player score
                playerGameScore = player.getScore();

                //handle how many lives player has left
                switch (playerLives)
                {
                    case 3:
                        life3.Visibility = Visibility.Collapsed;
                        invaders.setPlayerAlive(true);
                        break;
                    case 2:

                        life2.Visibility = Visibility.Collapsed;
                        invaders.setPlayerAlive(true);
                        break;
                    case 1:
                        dispatcherTimer.Stop();
                        life1.Visibility = Visibility.Collapsed;
                        finalScoreBlock.Text = playerGameScore.ToString();
                        gameOverPanel.Visibility = Visibility.Visible;
                        sounds.playGameOverSound();
                        return;
                }

                //loss of a life and reset player and alien grid
                playerLives--;
                canvas.Children.Clear();
                invaders.rebuildInvaders(canvas);
                player = new Player(canvas, playerGameScore);
            }

            //new level upon killing of all aliens
            if (!invaders.invadersAreAlive())
            {
                level++;
                invaders = new Invaders(canvas, level);
            }

            //call the draw methods of each player and alien classes passing in requirements
            invaders.Draw(canvas, player.getPlayer(), sounds);
            player.Draw(canvas, invaders.getInvaderGrid(), sounds);

            //update score textblock
            scoreBlock.Text = player.getScore().ToString();
        }

        //submit score function 
        private void submitScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            var endGameScore = new ApplicationDataCompositeValue();

            //default name set with no name entry
            if (playerName.Text.Length > 0) endGameScore["name"] = playerName.Text;
            else endGameScore["name"] = "NEWBIE123";

            //set the new score to local storage
            endGameScore["score"] = player.getScore();

            ApplicationData.Current.LocalSettings.Values["New Score Data"] = endGameScore;

            Frame.Navigate(typeof(HighScores));
        }
    }
}