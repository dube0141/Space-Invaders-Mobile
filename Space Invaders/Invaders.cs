using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Space_Invaders
{
    class Invaders
    {
        private Image[,] invaderGrid;
        private Image[] invaderBullets;
        private BitmapImage playerExplosion1 = new BitmapImage(new Uri("ms-appx:///Assets/sprites/explosion-1.png"));
        private BitmapImage playerExplosion2 = new BitmapImage(new Uri("ms-appx:///Assets/sprites/explosion-2.png"));

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

        //default invaders contructor accepted the canvas and current level
        public Invaders(Canvas canvas, int lvl)
        {
            //initialize variables and grid
            createRowsAndColumns();
            invaderGrid = new Image[columns, rows];
            invaderBullets = new Image[3];
            
            isMovingLeft = invadersAreMoving = isMovingDown = toggleSprite = false;
            isShooting = true;
            isPlayerAlive = true;
            count = 0;
            speed = lvl;

            //loops to set the alien grid and images
            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    Image invader = new Image();
                    BitmapImage bitmapSource;

                    //If the row number is smaller than 1, use this sprite
                    if (r < 1)
                    {
                        invader.Height = 24 * sizeModifier();
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-1-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-1-1.png"));
                    }
                    //If the row number is smaller than 3, use this sprite
                    else if (r < 3)
                    {
                        invader.Height = 28 * sizeModifier();
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-2-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-2-1.png"));
                    }
                    //If the row number is bigger than 3, use this sprite
                    else
                    {
                        invader.Height = 32 * sizeModifier();
                        invader.Tag = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-3-2.png"));
                        bitmapSource = new BitmapImage(new Uri("ms-appx:///Assets/sprites/alien-3-1.png"));
                    }

                    //Set the x coords for selected the invader
                    Canvas.SetLeft(invader, 32 + (50 * c));
                    
                    //Set the y coords for the selected invader
                    if (rows * 32 >= Window.Current.Bounds.Height / 2) Canvas.SetTop(invader, -((rows * 32) - 64) + (50 * r));
                    else Canvas.SetTop(invader, 32 + (50 * r));

                    invader.Width = 32 * sizeModifier();
                    invader.Source = bitmapSource;

                    //add invader to the canvas and invaderGrid
                    canvas.Children.Add(invader);
                    invaderGrid[c, r] = invader;
                }
            }
        }

        //Draw and update all object for every game frame
        public void Draw(Canvas canvas, Image player, Sounds sounds)
        {
            //For every 15 loops (minus the speed) toggle the invader image and move it
            if (count % (15 - speed) == 1) toggleSprite = invadersAreMoving = true;
            //For every 20 loops (minus the speed) make a selected invader shoot if the randomNumber is equal to 2
            if (count % (20 - speed) == 1 && new Random().Next(0, 3) == 2) invaderShoot(canvas, player);
            //Move the invader downward
            if (isMovingDown)
            {
                for (int c = 0; c < columns; c++)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        isMovingDown = false;
                        Canvas.SetTop(invaderGrid[c, r], Canvas.GetTop(invaderGrid[c, r]) + invaderGrid[c, r].Width);
                        //If the invader is not dead, continue
                        if (invaderGrid[c, r].Tag != null)
                        {
                            if (Canvas.GetTop(invaderGrid[c, r]) >= Window.Current.Bounds.Height - (invaderGrid[c, r].Height * 3))
                            {
                                isPlayerAlive = false;
                            }
                        }
                    }
                }
            }

            //change the x coords for all the invaders
            if (invadersAreMoving)
            {
                //Invaders are moving left
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
                //Invaders are moving right
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

            //Handle bullet collision and motion
            if (isShooting)
            {
                for (int i = 0; i < invaderBullets.Length; i++)
                {
                    if (canvas.Children.Contains(invaderBullets[i]))
                    {
                        //If the invader bullet is in between the x coords of the player, continue
                        if (Canvas.GetLeft(player) <= Canvas.GetLeft(invaderBullets[i]) && Canvas.GetLeft(player) + player.Width >= Canvas.GetLeft(invaderBullets[i]))
                        {
                            //If the invader bullet is in between the y coords of the player, continue
                            if (Canvas.GetTop(player) + player.Height >= Canvas.GetTop(invaderBullets[i]) && Canvas.GetTop(player) <= Canvas.GetTop(invaderBullets[i]))
                            {
                                //Kill the player
                                removeKilled(player);
                                sounds.playPlayerKilledSound();
                                isShooting = false;                                
                            }
                        }
                        //If the invader bullet collides with the bottom screen, continue
                        else if (Canvas.GetTop(invaderBullets[i]) >= Window.Current.Bounds.Height - invaderBullets[i].Height)
                        {
                            //delete bullet
                            canvas.Children.Remove(invaderBullets[i]);
                        }

                        //Move bullet downward
                        Canvas.SetTop(invaderBullets[i], Canvas.GetTop(invaderBullets[i]) + 15);
                    }
                }
            }

            toggleSprite = false;
            count++;
        }

        //animate the player death using an async method.
        private async void removeKilled(Image player)
        {
            player.Source = playerExplosion1;
            await Task.Delay(100);
            player.Source = playerExplosion2;
            await Task.Delay(100);
            player.Source = playerExplosion1;
            await Task.Delay(100);
            player.Source = playerExplosion2;
            await Task.Delay(100);
            player.Source = null;

            await Task.Delay(500);
            setPlayerAlive(false);
        }

        //Create a new invader bullet
        private void invaderShoot(Canvas canvas, Image player)
        {
            for (int i = 0; i < invaderBullets.Length; i++)
            {
                Random random = new Random();
                int oldRandomNumber = randomColumn;
                //Select a random column in the invaderGrid
                randomColumn = random.Next(0, columns);

                //If this column was last used, select another one
                if (randomColumn == oldRandomNumber) randomColumn = random.Next(0, columns);

                //Loop through the invaderGrid and select an invader to shoot
                for (int r = rows - 1; r > 0; r--)
                {
                    //If the selected invader is not dead, continue
                    if (invaderGrid[randomColumn, r].Tag != null)
                    {
                        if (!canvas.Children.Contains(invaderBullets[i]))
                        {
                            invaderBullets[i] = new Image();
                            invaderBullets[i].Width = 4;
                            invaderBullets[i].Height = 12;
                            invaderBullets[i].Source = new BitmapImage(new Uri("ms-appx:///Assets/Sprites/alien-bullet.png"));

                            //Set x and y coords for the new bullet
                            Canvas.SetTop(invaderBullets[i], Canvas.GetTop(invaderGrid[randomColumn, r]));
                            Canvas.SetLeft(invaderBullets[i], Canvas.GetLeft(invaderGrid[randomColumn, r]) + (invaderGrid[randomColumn, r].Width));

                            //Add the bullet to the canvas
                            canvas.Children.Add(invaderBullets[i]);
                            return;
                        }
                    }
                }
            }
        }

        //Toggle invader sprite image
        private void toggle(Image invader)
        {
            //If the invader is not dead, continue
            if (invader.Tag != null)
            {
                BitmapImage oldImage = (BitmapImage)invader.Source;
                BitmapImage newImage = (BitmapImage)invader.Tag;

                invader.Tag = oldImage;
                invader.Source = newImage;
            }
        }

        //Generate rows and columns that will fit the current screen width and height 
        private void createRowsAndColumns()
        {
            columns = Convert.ToInt32(Window.Current.Bounds.Width / 80);
            rows = Convert.ToInt32(Window.Current.Bounds.Height / 100);

            while (columns * rows <= 40) rows++;
        }

        //Control sprite image size (to adapt to various screen sizes)
        private double sizeModifier()
        {
            if (Window.Current.Bounds.Width <= 300) return 0.5;
            else if (Window.Current.Bounds.Width <= 500) return 0.8;
            else return 1;
        }

        //Return the invaderGrid
        public Image[,] getInvaderGrid()
        {
            return invaderGrid;
        }

        //Return the playerAlive status
        public bool playerAlive()
        {
            return isPlayerAlive;
        }
        
        //Set the playerAlive status
        public void setPlayerAlive(bool status)
        {
            isPlayerAlive = status;
            isShooting = status;
        }

        //Rebuild the invaderGrid (for a new level)
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

        //Check to see if all the invaders are dead
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
                        //Increment the invader movement and animation speed
                        else if (counter <= 10) speed = 2;
                        else if (counter <= 20) speed = 3;
                        else if (counter <= 30) speed = 4;
                        else if (counter <= 39) speed = 7;
                    }
                }
            }

            return true;
        }
    }
}