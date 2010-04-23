using System;
using System.Collections.Generic;
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
        GameObject[] bullets;
        const int maxBullets = 15;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport viewport;
        Rectangle viewportRect;
        float p_rotation = 0.0f;
        Texture2D p_sprite = null;
        Vector2 p_velocity = Vector2.Zero;
        Vector2 p_position = Vector2.Zero;
        bool p_fire;



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
            p_position.X = 200;
            p_position.Y = 200;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            viewport = GraphicsDevice.Viewport;
            viewportRect = new Rectangle(0, 0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            bullets = new GameObject[maxBullets];
            for (int i = 0; i < maxBullets; i++)
            {
                bullets[i] = new GameObject(Content.Load<Texture2D>("sprites\\cannonball"));
            }
            
            p_sprite = Content.Load<Texture2D>("sprites\\smiley1");
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
            foreach (GameObject bullet in bullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.position = p_position - bullet.center;
                    bullet.velocity = new Vector2((float)Math.Cos(p_rotation + Math.PI / 2), (float)Math.Sin(p_rotation + Math.PI / 2)) * 15.0f;
                    return;
                }
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
                    Rectangle bulletRect = new Rectangle((int)bullet.position.X, (int)bullet.position.Y, bullet.sprite.Width, bullet.sprite.Height);
                    //Rectangle explodeRect = new Rectangle((int)x,(int)y,10,1000);
                    /*
                    if(cannonBallRect.Intersects(explodeRect)){
                        ball.alive = false;
                        score += 1;
                        break;
                    }
                    */
                }
            }
        }
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            UpdateInput();
            updateBullets();
            p_position += p_velocity;
            if (p_fire)
            {
                fireBullets();
            }
            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected)
            {
                p_velocity.X = currentState.ThumbSticks.Left.X*5;
                p_velocity.Y = -currentState.ThumbSticks.Left.Y * 5;
                //p_rotation = -(float)((Math.Tan(currentState.ThumbSticks.Right.Y / currentState.ThumbSticks.Right.X)*2*Math.PI)/180);
                const float DEADZONE = 0.2f;
                const float FIREDEADZONE = 0.3f;

                Vector2 direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                float magnitude = direction.Length();
                p_fire = false;
                if (magnitude > DEADZONE)
                {
                    p_rotation = (float)(-1*(3.14/2+Math.Atan2(currentState.ThumbSticks.Right.Y, currentState.ThumbSticks.Right.X)));
                    if (magnitude > FIREDEADZONE)
                    {
                        p_fire = true;
                    }
                } 

            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(p_sprite, new Rectangle((int)p_position.X, (int)p_position.Y, 100, 100), null, Color.White,p_rotation,new Vector2(160,160), SpriteEffects.None, 0);
            // TODO: Add your drawing code here
            foreach (GameObject bullet in bullets)
            {
                if (bullet.alive)
                {
                    spriteBatch.Draw(bullet.sprite, bullet.position, Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
