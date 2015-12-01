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

        public MainPage()
        {
            this.InitializeComponent();

            player = new Player(canvas);
            invaders = new Invaders(canvas);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Game;
            dispatcherTimer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30);
            dispatcherTimer.Start();
        }

        private void Game(object sender, object e)
        {   
            if(!invaders.playerAlive())
            {
                canvas.Children.Clear();
                player = new Player(canvas);
                invaders = new Invaders(canvas);
                return;
            }
            invaders.Draw(canvas, player.getPlayer());
            player.Draw(canvas, invaders.getInvaderGrid());
        }
    }
}