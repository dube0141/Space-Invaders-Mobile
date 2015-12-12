using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Space_Invaders
{
    class Player
    {
        private Image playerSprite;
        private Image playerBullet;


        private BitmapImage playerBitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Sprites/player.png"));
        private BitmapImage invaderKilledImage = new BitmapImage(new Uri("ms-appx:///Assets/Sprites/explosion.png"));
        private BitmapImage alien1A = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-1-1.png"));
        private BitmapImage alien1B = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-1-2.png"));
        private BitmapImage alien2A = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-2-1.png"));
        private BitmapImage alien2B = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-2-2.png"));
        private BitmapImage alien3A = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-3-1.png"));
        private BitmapImage alien3B = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-3-2.png"));

        private bool isMovingLeft;
        private bool isMovingRight;
        private bool isShooting;

        private int playerScore;


        public Player(Canvas canvas, int playerGameScore)
        {
            Window.Current.CoreWindow.KeyDown += onKeyDown;
            Window.Current.CoreWindow.KeyUp += onKeyUp;

            playerScore = playerGameScore;
            isMovingLeft = isMovingRight = isShooting = false;


            playerSprite = new Image();
            playerSprite.Width = 52 * sizeModifier();
            playerSprite.Height = 32 * sizeModifier();

            Canvas.SetLeft(playerSprite, Window.Current.Bounds.Width / 2);
            Canvas.SetTop(playerSprite, Window.Current.Bounds.Height - (playerSprite.Height * 2));

            playerSprite.Source = playerBitmapImage;
            canvas.Children.Add(playerSprite);
        }

        public void Draw(Canvas canvas, Image[,] invaderGrid, Sounds sounds)
        {
            Canvas.SetTop(playerSprite, Window.Current.Bounds.Height - (playerSprite.Height * 2));

            if (isMovingLeft && Canvas.GetLeft(playerSprite) >= 0) Canvas.SetLeft(playerSprite, Canvas.GetLeft(playerSprite) - 6);
            if (isMovingRight && Canvas.GetLeft(playerSprite) <= Window.Current.Bounds.Width - playerSprite.Width) Canvas.SetLeft(playerSprite, Canvas.GetLeft(playerSprite) + 6);
            if (isShooting)
            {
                //If no bullet is found, create a new one.
                if (!canvas.Children.Contains(playerBullet))
                {
                    playerBullet = new Image();

                    playerBullet.Width = 3 * sizeModifier();
                    playerBullet.Height = 8 * sizeModifier();

                    playerBullet.Source = new BitmapImage(new Uri("ms-appx:///Assets/Sprites/player-bullet.png"));

                    Canvas.SetTop(playerBullet, Canvas.GetTop(playerSprite));
                    Canvas.SetLeft(playerBullet, Canvas.GetLeft(playerSprite) + (playerSprite.Width / 2 - 1));

                    sounds.playPlayerShootSound();
                    canvas.Children.Add(playerBullet);
                }
                //Move bullet upward and check for alienGrid collision.
                for (int r = 0; r < invaderGrid.GetLength(0); r++)
                {
                    for (int c = 0; c < invaderGrid.GetLength(1); c++)
                    {
                        if (Canvas.GetLeft(invaderGrid[r, c]) <= Canvas.GetLeft(playerBullet) && Canvas.GetLeft(invaderGrid[r, c]) + invaderGrid[r, c].Width >= Canvas.GetLeft(playerBullet))
                        {
                            if (Canvas.GetTop(invaderGrid[r, c]) + invaderGrid[r, c].Height >= Canvas.GetTop(playerBullet) && Canvas.GetTop(invaderGrid[r, c]) <= Canvas.GetTop(playerBullet))
                            {
                                if (invaderGrid[r, c].Tag != null)
                                {

                                    playerScore = playerScore + (10 * (invaderGrid.GetLength(1) - c));

                                    if (invaderGrid[r, c].Source == alien1A || invaderGrid[r, c].Source == alien1B) playerScore = playerScore + 100;
                                    else if (invaderGrid[r, c].Source == alien2A || invaderGrid[r, c].Source == alien2B) playerScore = playerScore + 50;
                                    else playerScore = playerScore + 25;


                                    isShooting = false;
                                    invaderGrid[r, c].Tag = null;
                                    sounds.playinvaderKilledSound();
                                    canvas.Children.Remove(playerBullet);
                                    removeKilled(canvas, invaderGrid[r, c]);

                                }
                            }
                        }
                        else if (Canvas.GetTop(playerBullet) <= 0)
                        {
                            isShooting = false;
                            canvas.Children.Remove(playerBullet);
                        }
                    }
                }

                Canvas.SetTop(playerBullet, Canvas.GetTop(playerBullet) - 15);
            }
        }

        private void onKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Left) isMovingLeft = true;
            if (args.VirtualKey == Windows.System.VirtualKey.Right) isMovingRight = true;

            if (args.VirtualKey == Windows.System.VirtualKey.Up) isShooting = true;
            if (args.VirtualKey == Windows.System.VirtualKey.Space) isShooting = true;
        }

        private void onKeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Left) isMovingLeft = false;
            if (args.VirtualKey == Windows.System.VirtualKey.Right) isMovingRight = false;
        }

        private double sizeModifier()
        {
            if (Window.Current.Bounds.Width <= 300) return 0.5;
            else if (Window.Current.Bounds.Width <= 500) return 0.8;
            else return 1;
        }

        private async void removeKilled(Canvas canvas, Image invader)
        {
            invader.Source = invaderKilledImage;
            await Task.Delay(300);
            canvas.Children.Remove(invader);
        }

        public Image getPlayer()
        {
            return playerSprite;
        }

        public int getScore()
        {
            return playerScore;
        }


    }
}
