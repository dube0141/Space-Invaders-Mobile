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

namespace Space_Invaders
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer dispatcherTimer;
        
        private Player player;
        private Invaders invaders;

        private ushort playerLives;

        public MainPage()
        {
            this.InitializeComponent();

            player = new Player(canvas);
            invaders = new Invaders(canvas);

            playerLives = 3;

        dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Game;
            dispatcherTimer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30);
            dispatcherTimer.Start();
        }

        private void Game(object sender, object e)
        {   
            if(!invaders.playerAlive())
            {
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
                        life1.Visibility = Visibility.Collapsed;
                        dispatcherTimer.Stop();
                        gameOverPanel.Visibility = Visibility.Visible;
                        invaders = new Invaders(canvas);
                        return;
                }

                playerLives--;
                canvas.Children.Clear();
                invaders.rebuildInvaders(canvas);
                player = new Player(canvas);
            }
            
            invaders.Draw(canvas, player.getPlayer());
            player.Draw(canvas, invaders.getInvaderGrid());

            scoreBlock.Text = player.getScore().ToString();
        }
    }
}