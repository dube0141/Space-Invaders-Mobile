﻿using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Space_Invaders
{
    class Invaders
    {
        private Image[,] invaderGrid;
        private Image[] invaderBullets;

        private bool invadersAreMoving;
        private bool isMovingLeft;
        private bool isMovingDown;
        private bool toggleSprite;
        private bool isShooting;

        private int count;
        private int speed;
        private int randomColumn;
        private int columns = Convert.ToInt32(Window.Current.Bounds.Width / 120);
        private int rows = Convert.ToInt32(Window.Current.Bounds.Height / 100);

        MediaElement BackgroundMusic;

        public Invaders(Canvas canvas)
        {
            BackgroundMusic = new MediaElement();
            BackgroundMusic.Source = new Uri("ms-appx:///Assets/Audio/01-opening-theme.mp3");
            BackgroundMusic.AutoPlay = true;
            canvas.Children.Add(BackgroundMusic);
            BackgroundMusic.Volume = 0.2;
            BackgroundMusic.Play();

            invaderGrid = new Image[columns, rows];
            invaderBullets = new Image[3];

            isMovingLeft = invadersAreMoving = isMovingDown = toggleSprite = false;
            isShooting = true;
            speed = count = 0;

            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    Image invader = new Image();
                    BitmapImage bitmapSource;

                    if (r < 1)
                    {
                        invader.Height = 24;
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-1-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-1-1.png"));
                    }
                    else if (r < 3)
                    {
                        invader.Height = 28;
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-2-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-2-1.png"));
                    }
                    else
                    {
                        invader.Height = 32;
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-3-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-3-1.png"));
                    }

                    Canvas.SetLeft(invader, 50 + (50 * c));
                    Canvas.SetTop(invader, 50 + (50 * r));

                    invader.Width = 32;
                    invader.Source = bitmapSource;

                    canvas.Children.Add(invader);
                    invaderGrid[c, r] = invader;
                }
            }
        }

        public void Draw(Canvas canvas, Image player)
        {
            if (count % 15 == 1) toggleSprite = invadersAreMoving = true;
            if (count % 20 == 1 && new Random().Next(0, 3) == 2) invaderShoot(canvas, player);
            if (isMovingDown)
            {
                for (int c = 0; c < columns; c++)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        isMovingDown = false;
                        Canvas.SetTop(invaderGrid[c, r], Canvas.GetTop(invaderGrid[c, r]) + invaderGrid[c, r].Width);
                    }
                }
            }

            if (invadersAreMoving)
            {
                if (isMovingLeft)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            if (toggleSprite) toggle(invaderGrid[c, r]);
                            if (Canvas.GetLeft(invaderGrid[c, r]) <= 0 + (invaderGrid[c, r].Width * 2))
                            {
                                isMovingDown = true;
                                isMovingLeft = false;
                            }

                            Canvas.SetLeft(invaderGrid[c, r], Canvas.GetLeft(invaderGrid[c, r]) - 20);
                        }
                    }
                }
                else
                {
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            if (toggleSprite) toggle(invaderGrid[c, r]);
                            if (Canvas.GetLeft(invaderGrid[c, r]) >= Window.Current.Bounds.Width - (invaderGrid[c, r].Width * 3))
                            {
                                isMovingDown = true;
                                isMovingLeft = true;
                            }

                            Canvas.SetLeft(invaderGrid[c, r], Canvas.GetLeft(invaderGrid[c, r]) + 20);
                        }
                    }
                }

                invadersAreMoving = false;
            }

            if (isShooting)
            {
                for (int i = 0; i < invaderBullets.Length; i++)
                {
                    if (canvas.Children.Contains(invaderBullets[i]))
                    {
                        if (Canvas.GetLeft(player) <= Canvas.GetLeft(invaderBullets[i]) && Canvas.GetLeft(player) + player.Width >= Canvas.GetLeft(invaderBullets[i]))
                        {
                            if (Canvas.GetTop(player) + player.Height >= Canvas.GetTop(invaderBullets[i]) && Canvas.GetTop(player) <= Canvas.GetTop(invaderBullets[i]))
                            {
                                isShooting = false;
                            }
                        }
                        else if (Canvas.GetTop(invaderBullets[i]) >= Window.Current.Bounds.Height - invaderBullets[i].Height)
                        {
                            canvas.Children.Remove(invaderBullets[i]);
                        }

                        Canvas.SetTop(invaderBullets[i], Canvas.GetTop(invaderBullets[i]) + 5);
                    }
                }
            }

            toggleSprite = false;
            count++;
        }

        private void invaderShoot(Canvas canvas, Image player)
        {
            for (int i = 0; i < invaderBullets.Length; i++)
            {
                Random random = new Random();
                int oldRandomNumber = randomColumn;
                randomColumn = random.Next(0, columns);

                if (randomColumn == oldRandomNumber) randomColumn = random.Next(0, columns);

                for (int r = rows - 1; r > 0; r--)
                {
                    if (invaderGrid[randomColumn, r].Tag != null)
                    {
                        if (!canvas.Children.Contains(invaderBullets[i]))
                        {
                            invaderBullets[i] = new Image();
                            invaderBullets[i].Width = 4;
                            invaderBullets[i].Height = 12;
                            invaderBullets[i].Source = new BitmapImage(new Uri("ms-appx:///Assets/Sprites/alien-bullet.png"));

                            Canvas.SetTop(invaderBullets[i], Canvas.GetTop(invaderGrid[randomColumn, r]));
                            Canvas.SetLeft(invaderBullets[i], Canvas.GetLeft(invaderGrid[randomColumn, r]) + (invaderGrid[randomColumn, r].Width));

                            canvas.Children.Add(invaderBullets[i]);
                            return;
                        }
                    }
                }
            }
        }

        private void toggle(Image invader)
        {
            if (invader.Tag != null)
            {
                BitmapImage oldImage = (BitmapImage)invader.Source;
                BitmapImage newImage = (BitmapImage)invader.Tag;

                invader.Tag = oldImage;
                invader.Source = newImage;
            }
        }

        public Image[,] getInvaderGrid()
        {
            return invaderGrid;
        }

        public bool playerAlive()
        {
            return isShooting;
        }
    }
}