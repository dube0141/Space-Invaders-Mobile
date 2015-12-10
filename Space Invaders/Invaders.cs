using System;
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
        private bool isPlayerAlive;

        private int count;
        private int speed;
        private int randomColumn;
        private int columns;
        private int rows;

        public Invaders(Canvas canvas)
        {

            createRowsAndColumns();
            invaderGrid = new Image[columns, rows];
            invaderBullets = new Image[3];

            isMovingLeft = invadersAreMoving = isMovingDown = toggleSprite = false;
            isShooting = true;
            isPlayerAlive = true;
            speed = count = 0;

            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    Image invader = new Image();
                    BitmapImage bitmapSource;

                    if (r < 1)
                    {
                        invader.Height = 24 * sizeModifier();
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-1-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-1-1.png"));
                    }
                    else if (r < 3)
                    {
                        invader.Height = 28 * sizeModifier();
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-2-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-2-1.png"));
                    }
                    else
                    {
                        invader.Height = 32 * sizeModifier();
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-3-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-3-1.png"));
                    }



                    Canvas.SetLeft(invader, 32 + (50 * c));
                    if (rows * 32 >= Window.Current.Bounds.Height / 2) Canvas.SetTop(invader, -((rows * 32) - 64) + (50 * r));
                    else Canvas.SetTop(invader, 32 + (50 * r));

                    invader.Width = 32 * sizeModifier();
                    invader.Source = bitmapSource;

                    canvas.Children.Add(invader);
                    invaderGrid[c, r] = invader;
                }
            }
        }

        public void Draw(Canvas canvas, Image player, Sounds sounds)
        {
            if (count % (15 - speed) == 1) toggleSprite = invadersAreMoving = true;
            if (count % (20 - speed) == 1 && new Random().Next(0, 3) == 2) invaderShoot(canvas, player);
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
                            if (invaderGrid[c, r].Tag != null)
                            {
                                if (toggleSprite) toggle(invaderGrid[c, r]);
                                if (Canvas.GetLeft(invaderGrid[c, r]) <= 0 + (invaderGrid[c, r].Width * 2))
                                {
                                    isMovingDown = true;
                                    isMovingLeft = false;
                                }

                                Canvas.SetLeft(invaderGrid[c, r], Canvas.GetLeft(invaderGrid[c, r]) - (20 + speed));
                            }
                        }
                    }
                }
                else
                {
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            if (invaderGrid[c, r].Tag != null)
                            {
                                if (toggleSprite) toggle(invaderGrid[c, r]);
                                if (Canvas.GetLeft(invaderGrid[c, r]) >= Window.Current.Bounds.Width - (invaderGrid[c, r].Width * 3))
                                {
                                    isMovingDown = true;
                                    isMovingLeft = true;
                                }

                                Canvas.SetLeft(invaderGrid[c, r], Canvas.GetLeft(invaderGrid[c, r]) + (20 + speed));
                            }
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
                                sounds.playPlayerKilledSound();
                                isShooting = false;
                                setPlayerAlive(false);
                            }
                        }
                        else if (Canvas.GetTop(invaderBullets[i]) >= Window.Current.Bounds.Height - invaderBullets[i].Height)
                        {
                            canvas.Children.Remove(invaderBullets[i]);
                        }

                        Canvas.SetTop(invaderBullets[i], Canvas.GetTop(invaderBullets[i]) + 15);
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

        private void createRowsAndColumns()
        {
            columns = Convert.ToInt32(Window.Current.Bounds.Width / 80);
            rows = Convert.ToInt32(Window.Current.Bounds.Height / 100);

            if (columns * rows > 40)
            {
                columns = columns / 2;
                rows = rows / 2;
            }

            while (columns * rows <= 40) rows++;
        }

        private double sizeModifier()
        {
            if (Window.Current.Bounds.Width <= 300) return 0.5;
            else if (Window.Current.Bounds.Width <= 500) return 0.8;
            else return 1;
        }

        public Image[,] getInvaderGrid()
        {
            return invaderGrid;
        }

        public bool playerAlive()
        {
            return isPlayerAlive;
        }
        public void setPlayerAlive(bool status)
        {
            isPlayerAlive = status;
            isShooting = status;
        }
        public void rebuildInvaders(Canvas canvas)
        {
            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    if (invaderGrid[c, r].Tag != null) canvas.Children.Add(invaderGrid[c, r]);
                }
            }
        }

        public bool invadersAreAlive()
        {
            int counter = 0;

            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    if (invaderGrid[c, r].Tag == null)
                    {
                        counter++;
                        if (counter == invaderGrid.Length) return false;
                        else if (counter <= 10) speed = 1;
                        else if (counter <= 20) speed = 3;
                        else if (counter <= 30) speed = 5;
                        else if (counter <= 39) speed = 9;
                    }
                }
            }

            return true;
        }
    }
}

