using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Bugzzz
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //arrays containing bullet entities and enemies
        GameObject[] bullets;
        GameObject[] turretBullets;
        GameObject[] enemies;
        ArrayList score;
        Turret turret; //Player 1 turret
        Turret turret2; //Player 2 turret
        const int maxEnemies = 5;
        const int maxBullets = 15;

        // Score display time on screen as it fades out
        const int SCORE_TIME = 80;
        
        Player player1;
        Player player2;
        SpriteFont font;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport viewport;
        Rectangle viewportRect;

        Random rand;
        //for fire delay
        float elapsedTime = 0;
        float t_elapsedTime = 0;
        float fireDelay = 0.25f;
        Texture2D heathBar;

        //rotation increment
        float angle_rot = .18f;
        float turret_rot = .3f;

        KeyboardState previousKeyboardState = Keyboard.GetState();
        MouseState previousMouseState = Mouse.GetState();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //player's stating position

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            turret = new Turret(Content.Load<Texture2D>("sprites\\cannon"));
            heathBar = Content.Load<Texture2D>("sprites\\healthBar");

            rand= new Random();
            viewport = GraphicsDevice.Viewport;
            viewportRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //Initializes all of the bullets, enemies, etc.
            bullets = new GameObject[maxBullets];
            for (int i = 0; i < maxBullets; i++)
            {
                bullets[i] = new GameObject(Content.Load<Texture2D>("sprites\\cannonball"));
            }
            turretBullets = new GameObject[maxBullets];
            for (int i = 0; i < maxBullets; i++)
            {
                turretBullets[i] = new GameObject(Content.Load<Texture2D>("sprites\\cannonball"));
            }
            enemies = new GameObject[maxEnemies];
            for (int j = 0; j < maxEnemies; j++)
            {
                enemies[j] = new GameObject(Content.Load<Texture2D>("sprites\\enemy"));
            }

            // The maximum amount of scores to display on screen is the maximum number of dead enemies
            score = new ArrayList();

            player1 = new Player(1, Content.Load<Texture2D>("sprites\\cannon"), Content.Load<Texture2D>("sprites\\smiley1"));
            player2 = new Player(2, Content.Load<Texture2D>("sprites\\cannon"), Content.Load<Texture2D>("sprites\\smiley1"));

            // Loading in the font we will use for showing the killed enemies score value
            font = Content.Load<SpriteFont>("ScoreFont");
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        public void fireBullets()
        {
            //firing command
            foreach (GameObject bullet in bullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.position = player1.p_position - bullet.center;
                    bullet.velocity = new Vector2((float)Math.Cos(player1.p_rotation + Math.PI / 2), (float)Math.Sin(player1.p_rotation + Math.PI / 2)) * 15.0f;
                    return;
                }
            }
        }

        public void fireTurretBullets()
        {
            //firing command
            foreach (GameObject bullet in turretBullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.position = turret.position - bullet.center;
                    bullet.velocity = new Vector2((float)Math.Cos(turret.rotation + Math.PI / 2), (float)Math.Sin(turret.rotation + Math.PI / 2)) * 15.0f;
                    turret.bulletsLeft -= 1;
                    return;
                }
            }
        }

   
        public void updateEnemies()
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    //checks for collision with the player
                    Rectangle playerRect = new Rectangle((int)player1.p_position.X - player1.p_spriteB.Width / 2, (int)player1.p_position.Y - player1.p_spriteB.Height/2, player1.p_spriteB.Width, player1.p_spriteB.Height);
                    Rectangle enemyRect = new Rectangle((int)enemy.position.X,(int)enemy.position.Y,enemy.sprite.Width,enemy.sprite.Height);
                    if (playerRect.Intersects(enemyRect))
                    {
                        //p_alive = false;
                        enemy.alive = false;
                        player1.health -= 25;
                        
                        break;
                    }
                    playerRect = new Rectangle((int)player2.p_position.X - player2.p_spriteB.Width / 2, (int)player2.p_position.Y - player2.p_spriteB.Height / 2, player2.p_spriteB.Width, player2.p_spriteB.Height);
                    
                    if (playerRect.Intersects(enemyRect))
                    {
                        //p_alive = false;
                        enemy.alive = false;
                        player2.health -= 25;
                        
                        break;
                    }
                    if (!viewportRect.Contains(new Point((int)enemy.position.X, (int)enemy.position.Y)))
                    {
                        enemy.alive = false;
                    }
                }
                else
                {
                   // Console.WriteLine("made an enemy");
                    enemy.alive = true;
                    int side = rand.Next(4);
                    if (side == 0)
                    {
                        enemy.position = new Vector2(viewportRect.Left, MathHelper.Lerp(0.0f, (float)viewportRect.Height, (float)rand.NextDouble()));
                    }
                    else if (side == 1)
                    {
                        enemy.position = new Vector2(viewportRect.Top, MathHelper.Lerp(0.0f, (float)viewportRect.Width, (float)rand.NextDouble()));
                    }
                    else if (side == 2)
                    {
                        enemy.position = new Vector2(viewportRect.Right, MathHelper.Lerp(0.0f, (float)viewportRect.Height, (float)rand.NextDouble()));
                    }
                    else
                    {
                        enemy.position = new Vector2(viewportRect.Bottom, MathHelper.Lerp(0.0f, (float)viewportRect.Width, (float)rand.NextDouble()));
                    }
                }
                //makes enemies move towards the player
                Vector2 target;

                if (MathFns.Distance(enemy.position, player1.p_position) > MathFns.Distance(enemy.position, player2.p_position))
                    target = player2.p_position;
                else
                    target = player1.p_position;

                enemy.velocity = target - enemy.position;
                enemy.velocity.Normalize();
                enemy.position += enemy.velocity * 2;
            }
        }

        public void updateBullets()
        {
            foreach (GameObject bullet in bullets)
            {
                if (bullet.alive)
                {
                    bullet.position += bullet.velocity;
                    if (!viewportRect.Contains(new Point((int)bullet.position.X, (int)bullet.position.Y)))
                    {
                        bullet.alive = false;
                        continue;
                    }
                    Rectangle bulletRect = new Rectangle(
                        (int)bullet.position.X,
                        (int)bullet.position.Y,
                        bullet.sprite.Width,
                        bullet.sprite.Height);
                    //enemy-bullet collision detection.
                    foreach (GameObject enemy in enemies)
                    {
                        Rectangle enemyRect = new Rectangle(
                            (int)enemy.position.X,
                            (int)enemy.position.Y,
                            enemy.sprite.Width,
                            enemy.sprite.Height);

                        if (bulletRect.Intersects(enemyRect))
                        {
                            bullet.alive = false;
                            enemy.alive = false;
                            score.Add(new Score(20,SCORE_TIME,enemy.position,true));
                            player1.score += 20;
                            break;
                        }
                    }
                }
            }
            foreach (GameObject bullet in turretBullets)
            {
                if (bullet.alive)
                {
                    bullet.position += bullet.velocity;
                    if (!viewportRect.Contains(new Point((int)bullet.position.X, (int)bullet.position.Y)))
                    {
                        bullet.alive = false;
                        continue;
                    }
                    Rectangle bulletRect = new Rectangle(
                        (int)bullet.position.X,
                        (int)bullet.position.Y,
                        bullet.sprite.Width,
                        bullet.sprite.Height);
                    //enemy-bullet collision detection.
                    foreach (GameObject enemy in enemies)
                    {
                        Rectangle enemyRect = new Rectangle(
                            (int)enemy.position.X,
                            (int)enemy.position.Y,
                            enemy.sprite.Width,
                            enemy.sprite.Height);

                        if (bulletRect.Intersects(enemyRect))
                        {
                            bullet.alive = false;
                            enemy.alive = false;
                            score.Add(new Score(10, SCORE_TIME, enemy.position, true));
                            player1.score += 10;
                            break;
                        }
                    }
                }
            }
        }
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                player1.deploy = true;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += elapsed;
            t_elapsedTime += elapsed;
            // TODO: Add your update logic here
            UpdateTurret();
            UpdateInput();
            updateBullets();
            updateEnemies();
            player1.p_position += player1.p_velocity;
            if (player1.p_fire && elapsedTime>=fireDelay)
            {
                elapsedTime = 0.0f;
                fireBullets();
            }
            if (turret.fire && t_elapsedTime >= fireDelay+.5 && turret.placed)
            {
                t_elapsedTime = 0.0f;
                fireTurretBullets();
            }
            base.Update(gameTime);
        }


        protected void UpdateTurret()
        {
            if (turret.bulletsLeft > 0)
            {

                if (player1.deploy)
                {
                    turret.position = player1.p_position;
                    turret.rotation = 0f;
                    turret.placed = true;
                    player1.deploy = false;
                }
                if (turret.placed)
                {
                    double temp = MathFns.Distance(turret.position, enemies[0].position);
                    turret.closestEnemy = 0;
                    Vector2 offset = new Vector2(); //allows for aiming at center of target

                    for (int i = 1; i < enemies.Length; i++)
                    {
                        if (MathFns.Distance(enemies[i].position,turret.position) < temp)
                        {
                           turret.closestEnemy = i;
                           offset.X = enemies[i].sprite.Width/2;
                           offset.Y = enemies[i].sprite.Height/2;
                        }
                    }

                    Vector2 aim = new Vector2(enemies[turret.closestEnemy].position.X + offset.X, enemies[turret.closestEnemy].position.Y + offset.Y);
                    Vector2 direction = turret.position - aim;
                    
                    direction.Normalize();
                    float desiredAngle = (float)Math.Acos((double)direction.X);

                    if (direction.Y < 0)
                    {
                        desiredAngle = (float)(2.0f * Math.PI) - (float)desiredAngle;
                    }

                    //Smooth Rotation
                    if (desiredAngle != turret.rotation)
                    {
                      // Console.WriteLine("Turret: " + turret.rotation + "Desired: " + desiredAngle);
                       turret.rotation = MathFns.Clerp(turret.rotation, desiredAngle + (float)Math.PI / 2, this.turret_rot);
                    }
                    
                    //previous rotation function
                    //turret.rotation = desiredAngle + (float)Math.PI / 2;
                    turret.fire = true;
                }
            }
            else
            {
                turret.placed = false;
                player1.deploy = false;
                turret.bulletsLeft = 25;

            }
        }
        protected void UpdateInput()
        {


            GamePadState currentState;

            #region Player 1 Control Scheme
            currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected)
            {
                #region XBox Controls Player1
                player1.p_velocity.X = currentState.ThumbSticks.Left.X * 5;
                player1.p_velocity.Y = -currentState.ThumbSticks.Left.Y * 5;
                //player1.p_rotation = -(float)((Math.Tan(currentState.ThumbSticks.Right.Y / currentState.ThumbSticks.Right.X)*2*Math.PI)/180);
                const float DEADZONE = 0.2f;
                const float FIREDEADZONE = 0.3f;

                Vector2 direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                float magnitude = direction.Length();
                player1.p_fire = false;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Length() > DEADZONE)
                {
                    player1.p_rotation_b = (float)(-1 * (3.14 / 2 + Math.Atan2(currentState.ThumbSticks.Left.Y, currentState.ThumbSticks.Left.X)));
                }

                if (magnitude > DEADZONE)
                {
                    //Smooth Rotation
                    float angle = (float)(-1 * (3.14 / 2 + Math.Atan2(currentState.ThumbSticks.Right.Y, currentState.ThumbSticks.Right.X)));

                    if (angle != player1.p_rotation)
                        player1.p_rotation = MathFns.Clerp(player1.p_rotation, angle, angle_rot);

                    if (magnitude > FIREDEADZONE)
                    {
                        player1.p_fire = true;
                    }
                }
                #endregion

            }
            //keyboard controls
            else
            {
                #region Keyboard Controls Player1
                //player1.p_fire = false;
                KeyboardState keyboardState = Keyboard.GetState();
                MouseState mouse = Mouse.GetState();
                float XDistance = player1.p_position.X - mouse.X;
                float YDistance = player1.p_position.Y - mouse.Y;
                float xdim = 800;
                float ydim = 800;

                float rotation = (float)(Math.Atan2(YDistance, XDistance) + Math.PI / 2);
                player1.p_rotation = rotation;

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    player1.p_fire = true;
                    xdim = mouse.X;
                    ydim = mouse.Y;
                }
                else
                {
                    player1.p_fire = false;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    player1.p_velocity.X = -5;
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    player1.p_velocity.X = 5;
                }
                else
                {
                    player1.p_velocity.X = 0;
                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    player1.p_velocity.Y = -5;
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    player1.p_velocity.Y = 5;
                }
                else
                {
                    player1.p_velocity.Y = 0;
                }
                if (keyboardState.IsKeyDown(Keys.Z))
                {
                    player1.deploy = true;
                }
                if (keyboardState.IsKeyDown(Keys.Escape))
                    this.Exit();



                
                //cannon.rotation = MathHelper.Clamp(cannon.rotation, MathHelper.PiOver2, 0);
                // TODO: Add your update logic here
                previousKeyboardState = keyboardState;
                previousMouseState = mouse;

                #endregion
            }
            #endregion
            
            currentState = GamePad.GetState(PlayerIndex.Two);

            #region Player 2 Controls

            if (currentState.IsConnected)
            {
                #region XBox Controller Controls Player 2
                player2.p_velocity.X = currentState.ThumbSticks.Left.X * 5;
                player2.p_velocity.Y = -currentState.ThumbSticks.Left.Y * 5;
                //player2.p_rotation = -(float)((Math.Tan(currentState.ThumbSticks.Right.Y / currentState.ThumbSticks.Right.X)*2*Math.PI)/180);
                const float DEADZONE = 0.2f;
                const float FIREDEADZONE = 0.3f;

                Vector2 direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                float magnitude = direction.Length();
                player2.p_fire = false;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Length() > DEADZONE)
                {
                    player2.p_rotation_b = (float)(-1 * (3.14 / 2 + Math.Atan2(currentState.ThumbSticks.Left.Y, currentState.ThumbSticks.Left.X)));
                }

                if (magnitude > DEADZONE)
                {
                    //Smooth Rotation
                    float angle = (float)(-1 * (3.14 / 2 + Math.Atan2(currentState.ThumbSticks.Right.Y, currentState.ThumbSticks.Right.X)));

                    if (angle != player2.p_rotation)
                        player2.p_rotation = MathFns.Clerp(player2.p_rotation, angle, angle_rot);

                    if (magnitude > FIREDEADZONE)
                    {
                        player2.p_fire = true;
                    }
                }
                #endregion

            }
            //keyboard controls
            else
            {
                #region Keyboard Controls Player 2

                //player2.p_fire = false;
                KeyboardState keyboardState = Keyboard.GetState();
                MouseState mouse = Mouse.GetState();
                float XDistance = player2.p_position.X - mouse.X;
                float YDistance = player2.p_position.Y - mouse.Y;
                float xdim = 800;
                float ydim = 800;

                float rotation = (float)(Math.Atan2(YDistance, XDistance) + Math.PI / 2);
                player2.p_rotation = rotation;

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    player2.p_fire = true;
                    xdim = mouse.X;
                    ydim = mouse.Y;
                }
                else
                {
                    player2.p_fire = false;
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    player2.p_velocity.X = -5;
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    player2.p_velocity.X = 5;
                }
                else
                {
                    player2.p_velocity.X = 0;
                }
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    player2.p_velocity.Y = -5;
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    player2.p_velocity.Y = 5;
                }
                else
                {
                    player2.p_velocity.Y = 0;
                }
                if (keyboardState.IsKeyDown(Keys.Q))
                {
                    player2.deploy = true;
                }
                if (keyboardState.IsKeyDown(Keys.Escape))
                    this.Exit();




                //cannon.rotation = MathHelper.Clamp(cannon.rotation, MathHelper.PiOver2, 0);
                // TODO: Add your update logic here
                previousKeyboardState = keyboardState;
                previousMouseState = mouse;
                #endregion Keyboard Controls

            }
            #endregion
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(heathBar, new Rectangle(50, 50, player1.health, 15), Color.Red);
            spriteBatch.DrawString(font, player1.score.ToString(), new Vector2(30, 30), new Color(Color.Red, (byte)130));
            spriteBatch.Draw(player1.p_spriteB, new Rectangle((int)player1.p_position.X, (int)player1.p_position.Y, player1.p_spriteB.Width, player1.p_spriteB.Height), null, Color.White, player1.p_rotation_b, new Vector2(player1.p_spriteB.Width / 2, player1.p_spriteB.Height / 2), SpriteEffects.None, 0);
            spriteBatch.Draw(player1.p_spriteT, new Rectangle((int)player1.p_position.X, (int)player1.p_position.Y, player1.p_spriteT.Width, player1.p_spriteT.Height), null, Color.White, (float)(player1.p_rotation+.5*Math.PI), new Vector2(player1.p_spriteT.Width / 2, player1.p_spriteT.Height / 2), SpriteEffects.None, 0);
            if (turret.placed)
            {
                spriteBatch.Draw(turret.sprite, new Rectangle((int)turret.position.X, (int)turret.position.Y, turret.sprite.Width, turret.sprite.Height), null, Color.White, (float)(turret.rotation + .5 * Math.PI), new Vector2(turret.sprite.Width / 2, turret.sprite.Height / 2), SpriteEffects.None, 0);

            }// TODO: Add your drawing code here
            #region Drawing Code:Bullets, TurretBullets, Enemies, Scores
            foreach (GameObject bullet in bullets)
            {
                if (bullet.alive)
                {
                    spriteBatch.Draw(bullet.sprite, bullet.position, Color.White);
                }
            }
            foreach (GameObject bullet in turretBullets)
            {
                if (bullet.alive)
                {
                    spriteBatch.Draw(bullet.sprite, bullet.position, Color.White);
                }
            }
            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    spriteBatch.Draw(enemy.sprite, enemy.position, Color.White);
                }
            }
            ArrayList deadScores = new ArrayList(); //Used for determing what scores need to be deleted. 
            //Output Scores
            foreach (Score s in score)
            {
                if (s.alive)
                {
                    if (s.time > 0)
                    {
                        spriteBatch.DrawString(font, s.pointVal.ToString(), s.position, new Color(Color.Red,(byte)(s.time*2.5)));
                        s.time--;
                    }
                    else
                    {
                        s.alive = false;
                        deadScores.Add(s);
                    }
                }
            }
            //Remove Scores
            foreach (Score s in deadScores)
            {
                score.Remove(s);
            }
            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
