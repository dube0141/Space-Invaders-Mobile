using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Space_Invaders
{
    class Sounds
    {
        MediaElement BackgroundMusic;
        MediaElement playerShootSound;
        MediaElement invaderKilledSound;
        MediaElement playerKilledSound;
        MediaElement gameOverSound;


        public Sounds(Grid grid)
        {
            BackgroundMusic = new MediaElement();
            BackgroundMusic.Source = new Uri("ms-appx:///Assets/Audio/01-opening-theme.mp3");
            BackgroundMusic.AutoPlay = true;
            BackgroundMusic.MediaEnded += new RoutedEventHandler(m_MediaEnded);
            grid.Children.Add(BackgroundMusic);
            BackgroundMusic.Volume = 0.2;

            playerShootSound = new MediaElement();
            playerShootSound.Source = new Uri("ms-appx:///Assets/Audio/shoot.wav");
            playerShootSound.AutoPlay = false;
            grid.Children.Add(playerShootSound);
            playerShootSound.Volume = 0.2;

            invaderKilledSound = new MediaElement();
            invaderKilledSound.Source = new Uri("ms-appx:///Assets/Audio/invaderkilled.wav");
            invaderKilledSound.AutoPlay = false;
            grid.Children.Add(invaderKilledSound);
            invaderKilledSound.Volume = 0.2;

            playerKilledSound = new MediaElement();
            playerKilledSound.Source = new Uri("ms-appx:///Assets/Audio/explosion.wav");
            playerKilledSound.AutoPlay = false;
            grid.Children.Add(playerKilledSound);
            playerKilledSound.Volume = 0.2;

            gameOverSound = new MediaElement();
            gameOverSound.Source = new Uri("ms-appx:///Assets/Audio/gameover.mp3");
            gameOverSound.AutoPlay = false;
            grid.Children.Add(gameOverSound);
            gameOverSound.Volume = 0.2;
        }

        void m_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.FromSeconds(0);
            BackgroundMusic.Play();
        }

        public void playBackgroundMusic()
        {
            BackgroundMusic.Play();
        }
        public void playPlayerShootSound()
        {
            playerShootSound.Play();
        }

        public void playinvaderKilledSound()
        {
            invaderKilledSound.Play();
        }
        public void playPlayerKilledSound()
        {
            playerKilledSound.Play();
        }
        public void playGameOverSound()
        {
            gameOverSound.Play();
        }

    }
}
