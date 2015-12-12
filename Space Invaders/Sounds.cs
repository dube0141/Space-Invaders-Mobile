using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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

        double volume;

        public Sounds(Grid grid)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("volumeLevel"))
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                Object volumeLevel = localSettings.Values["volumeLevel"];
                volume = (Convert.ToDouble(volumeLevel) / 100);
            }
            else
            {
                volume = 0.5;
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("musicOn"))
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                Object musicOn = localSettings.Values["musicOn"];
                if (!Convert.ToBoolean(musicOn))
                {
                    volume  = 0;
                }
            }

            BackgroundMusic = new MediaElement();
            BackgroundMusic.Source = new Uri("ms-appx:///Assets/Audio/01-opening-theme.mp3");
            BackgroundMusic.AutoPlay = true;
            BackgroundMusic.MediaEnded += new RoutedEventHandler(m_MediaEnded);
            grid.Children.Add(BackgroundMusic);
            BackgroundMusic.Volume = volume;
            BackgroundMusic.Play();

            playerShootSound = new MediaElement();
            playerShootSound.Source = new Uri("ms-appx:///Assets/Audio/shoot.wav");
            playerShootSound.AutoPlay = false;
            grid.Children.Add(playerShootSound);
            playerShootSound.Volume = volume;

            invaderKilledSound = new MediaElement();
            invaderKilledSound.Source = new Uri("ms-appx:///Assets/Audio/invaderkilled.wav");
            invaderKilledSound.AutoPlay = false;
            grid.Children.Add(invaderKilledSound);
            invaderKilledSound.Volume = volume;

            playerKilledSound = new MediaElement();
            playerKilledSound.Source = new Uri("ms-appx:///Assets/Audio/explosion.wav");
            playerKilledSound.AutoPlay = false;
            grid.Children.Add(playerKilledSound);
            playerKilledSound.Volume = volume;

            gameOverSound = new MediaElement();
            gameOverSound.Source = new Uri("ms-appx:///Assets/Audio/gameover.mp3");
            gameOverSound.AutoPlay = false;
            grid.Children.Add(gameOverSound);
            gameOverSound.Volume = volume;
        }

        void m_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.FromSeconds(0);
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
