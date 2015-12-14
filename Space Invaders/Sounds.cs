using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Space_Invaders
{
    class Sounds
    {
        //Variables
        MediaElement BackgroundMusic;
        MediaElement playerShootSound;
        MediaElement invaderKilledSound;
        MediaElement playerKilledSound;
        MediaElement gameOverSound;

        double volume;

        //Default constuctor
        public Sounds(Grid grid)
        {
            //If the volume is set in the localStorage, use this volume level
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

            //If the mute option is enabled in the localStorage, mute the volume
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("musicOn"))
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                Object musicOn = localSettings.Values["musicOn"];
                if (!Convert.ToBoolean(musicOn))
                {
                    volume  = 0;
                }
            }

            //Create the background music
            BackgroundMusic = new MediaElement();
            BackgroundMusic.Source = new Uri("ms-appx:///Assets/Audio/01-opening-theme.mp3");
            BackgroundMusic.AutoPlay = true;
            BackgroundMusic.MediaEnded += new RoutedEventHandler(m_MediaEnded);
            grid.Children.Add(BackgroundMusic);
            BackgroundMusic.Volume = volume;
            BackgroundMusic.Play();

            //Create the player shooting sound FX
            playerShootSound = new MediaElement();
            playerShootSound.Source = new Uri("ms-appx:///Assets/Audio/shoot.wav");
            playerShootSound.AutoPlay = false;
            grid.Children.Add(playerShootSound);
            playerShootSound.Volume = volume;

            //Create the invader killed sound FX
            invaderKilledSound = new MediaElement();
            invaderKilledSound.Source = new Uri("ms-appx:///Assets/Audio/invaderkilled.wav");
            invaderKilledSound.AutoPlay = false;
            grid.Children.Add(invaderKilledSound);
            invaderKilledSound.Volume = volume;

            //Create the player killed sound FX
            playerKilledSound = new MediaElement();
            playerKilledSound.Source = new Uri("ms-appx:///Assets/Audio/explosion.wav");
            playerKilledSound.AutoPlay = false;
            grid.Children.Add(playerKilledSound);
            playerKilledSound.Volume = volume;

            //Create the gameover sound FX
            gameOverSound = new MediaElement();
            gameOverSound.Source = new Uri("ms-appx:///Assets/Audio/gameover.mp3");
            gameOverSound.AutoPlay = false;
            grid.Children.Add(gameOverSound);
            gameOverSound.Volume = volume;
        }

        //Play sound on method call
        void m_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.FromSeconds(0);
            BackgroundMusic.Play();
        }

        //Play sound on method call
        public void playPlayerShootSound()
        {
            playerShootSound.Play();
        }

        //Play sound on method call
        public void playinvaderKilledSound()
        {
            invaderKilledSound.Play();
        }

        //Play sound on method call
        public void playPlayerKilledSound()
        {
            playerKilledSound.Play();
        }

        //Play sound on method call
        public void playGameOverSound()
        {
            gameOverSound.Play();
        }
    }
}
