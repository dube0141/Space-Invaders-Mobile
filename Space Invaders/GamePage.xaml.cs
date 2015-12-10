﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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


        public GamePage()
        {
            this.InitializeComponent();

            playerLives = 1;
            playerGameScore = 0;

            player = new Player(canvas, playerGameScore);
            invaders = new Invaders(canvas);
            sounds = new Sounds(grid);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Game;
            dispatcherTimer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30);
            dispatcherTimer.Start();
        }

        private void Game(object sender, object e)
        {
            if (!invaders.playerAlive())
            {
                playerGameScore = player.getScore();

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

                playerLives--;
                canvas.Children.Clear();
                invaders.rebuildInvaders(canvas);
                player = new Player(canvas, playerGameScore);
            }

            if (!invaders.invadersAreAlive())
            {
                invaders = new Invaders(canvas);
            }

            invaders.Draw(canvas, player.getPlayer(), sounds);
            player.Draw(canvas, invaders.getInvaderGrid(), sounds);

            scoreBlock.Text = player.getScore().ToString();
        }



        private void submitScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            var endGameScore = new ApplicationDataCompositeValue();

            endGameScore["name"] = playerName.Text;
            endGameScore["score"] = player.getScore();

            ApplicationData.Current.LocalSettings.Values["New Score Data"] = endGameScore;

            Frame.Navigate(typeof(HighScores));
        }
    }
}