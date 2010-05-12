using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class NarcolepsyEffect
    {
        #region Fields (active, state, pressed)
        private bool active;
        private int width;
        private int height;
        private int state;
        private bool pressed;
        private int curLevel;
        private SpriteFont dispFont;
        private Random rand;
        private int playerID;
        private Keys[] konamiKeys;
        private Buttons[] konamiButtons;
        private int kCIndex;
        Texture2D[] narcArray;

        Texture2D narcPanel;
        Viewport viewport;

        //used for updateOne
        //indicates how many times a button has been pressed
        private int pressCount;
        private int totalCount;

        //current position of top left corner of spell box
        private Vector2 position;


        //positions of box when it is locked on or off screen
        private Vector2 lockedLeft;
        private Vector2 lockedRight;

        #endregion

        #region Accessors (Active, State, Pressed) [r = read only, w = write only, rw = read/write]
        //rw
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }
        //r
        public int State
        {
            get
            {
                return state;
            }
        }
        //rw
        public bool Pressed
        {
            get
            {
                return pressed;
            }
            set
            {
                pressed = value;
            }
        }
        
        #endregion


        #region Main Methods (Constructor, Update, Narc Update, updateLevel, updateOne, updateTwo, updateThree, updateFour, Draw)
        public NarcolepsyEffect(SpriteFont f, Texture2D narcPanel, Texture2D[] narcTextureArray, Viewport viewport, int id)
        {
            this.active = false;
            this.state = 0;
            this.pressed = false;
            this.curLevel = 1;
            this.totalCount = 10;
            this.pressCount = 0;
            dispFont = f;
            this.playerID = id;
            this.narcPanel = narcPanel;

            int hash = id*DateTime.Now.Second + 791;
            rand = new Random(hash);

            kCIndex = 0;
            konamiKeys = new Keys[10];
            konamiButtons = new Buttons[10];

            narcArray = narcTextureArray;

            //Keys and Buttons
            konamiKeys[0] = Keys.Up;
            konamiKeys[1] = Keys.Up;
            konamiKeys[2] = Keys.Down;
            konamiKeys[3] = Keys.Down;
            konamiKeys[4] = Keys.Left;
            konamiKeys[5] = Keys.Right;
            konamiKeys[6] = Keys.Left;
            konamiKeys[7] = Keys.Right;
            konamiKeys[8] = Keys.B;
            konamiKeys[9] = Keys.A;

            konamiButtons[0] = Buttons.DPadUp;
            konamiButtons[1] = Buttons.DPadUp;
            konamiButtons[2] = Buttons.DPadDown;
            konamiButtons[3] = Buttons.DPadDown;
            konamiButtons[4] = Buttons.DPadLeft;
            konamiButtons[5] = Buttons.DPadRight;
            konamiButtons[6] = Buttons.DPadLeft;
            konamiButtons[7] = Buttons.DPadRight;
            konamiButtons[8] = Buttons.B;
            konamiButtons[9] = Buttons.A;
            this.viewport = viewport;

            if (playerID == 1)
                position = new Vector2(-1*narcPanel.Width, viewport.Height/2-(narcPanel.Height/2));
            else
                position = new Vector2(viewport.Width, viewport.Height/2-(narcPanel.Height/2));
            if (playerID == 1)
            {
                lockedLeft = new Vector2(-1*narcPanel.Width, position.Y);
                lockedRight = new Vector2(0, position.Y);
            }
            else
            {
                lockedLeft = new Vector2(viewport.Width - narcPanel.Width, position.Y);
                lockedRight = new Vector2(viewport.Width, position.Y);
            }
                
        }


        public void Update(GamePadState g, KeyboardState k)
        {
            if (active)
            {
                NarcolepsyUpdate(g, k);
            }
            else if (rand.Next(1000) == 1)
            {

                active = true;
                int r = rand.Next(100);
                if (r < 50)
                    this.state = 1;
                else
                    this.state = 2;
                // this.state = rand.Next(2)+1;
                Console.WriteLine(state + "   " + r);
            }
            else
            {
                if (playerID == 1)
                {
                    moveLeft();
                }
                else
                {
                    moveRight();
                }
            }
            
        }

        private void NarcolepsyUpdate(GamePadState g, KeyboardState k)
        {
            switch (state)
            {
                case 1:
                    updateOne(g,k);
                    break;
                case 2:
                    if (playerID == 1)
                        moveRight();
                    else
                        moveLeft();
                    updateTwo(g,k);
                    break;
                case 3:
                    updateThree(g,k);
                    break;
                case 4:
                    updateFour(g,k);
                    break;
                default:
                    updateOne(g,k);
                    break;
            }

        }

        public void updateLevel(int level)
        {
            this.curLevel = level;
            this.totalCount = level * 10;
        }

        //press A repeatedly (space on keyboard)
        private void updateOne(GamePadState g, KeyboardState k)
        {
            if (g.IsConnected)
            {
                if (g.Buttons.A == ButtonState.Pressed && !this.pressed)
                {
                    pressCount++;
                    this.pressed = true;
                }
                else if (g.Buttons.A == ButtonState.Released)
                    this.pressed = false;
            }
            else
            {
                if (k.IsKeyDown(Keys.Space) && !this.pressed)
                {
                    pressCount++;
                    this.pressed = true;
                }
                else if (k.IsKeyUp(Keys.Space))
                    this.pressed = false;
            }
            if (this.pressCount >= this.totalCount)
            {
                this.active = false;
                this.pressCount = 0;
            }
        }

        //not yet implemented
        private void updateTwo(GamePadState g, KeyboardState k)
        {
            if (g.IsConnected)
            {
                if (g.IsButtonDown(konamiButtons[kCIndex]) && !this.pressed)
                {
                    this.kCIndex++;
                    this.pressed = true;
                }
                else if (g.IsButtonUp(konamiButtons[kCIndex]))
                {
                    this.pressed = false;
                }

                
            }
            else
            {
                if (k.IsKeyDown(konamiKeys[this.kCIndex]) && !this.pressed)
                {
                    this.kCIndex++;
                    this.pressed = true;
                    Console.WriteLine(kCIndex);
                }
                else if (k.IsKeyUp(konamiKeys[this.kCIndex]))
                {
                    this.pressed = false;
                }
            }

            if (kCIndex == 10)
            {
                this.pressed = false;
                this.active = false;
                this.kCIndex = 0;
            }

        }

        //not yet implemented
        private void updateThree(GamePadState g, KeyboardState k)
        {

            this.active = false;
        }

        //not yet implemented
        private void updateFour(GamePadState g, KeyboardState k)
        {
            this.active = false;
        }

        //draws text telling person what to do
        public void Draw(SpriteBatch s)
        {
                switch (state)
                {
                    case 1:
                        if(this.active)
                        s.DrawString(dispFont, "Oh no!! Player " + playerID + " press A (Space) a lot!!!", new Vector2(350f, 150f), Color.Green);
                        /*
                        if (playerID == 1)
                            s.Draw(narcPanel, new Vector2(0,viewport.Height/2-(narcPanel.Height/2)), null, Color.White);
                        
                         */
                        
                        break;
                    case 2:
                        //s.DrawString(dispFont, "Player " + playerID + " Enter the Konami Code!!!", new Vector2(350f, 150f), Color.Green);
                        if (playerID == 1)
                        {
                            s.Draw(narcPanel, new Rectangle((int)position.X, (int)position.Y, narcPanel.Width, narcPanel.Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipHorizontally, 0);
                            
                        }
                        else
                        {
                            s.Draw(narcPanel, new Rectangle((int)position.X, (int)position.Y, narcPanel.Width, narcPanel.Height), null, Color.White, (float)0, new Vector2(), SpriteEffects.None, 0);
                        }

                        switch (kCIndex)
                        {
                            case 0:
                                s.Draw(narcArray[4], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[4].Width, narcArray[4].Height), null, Color.Yellow, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[6], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 1:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[4], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.Yellow, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[6], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 2:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[4], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.Yellow, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[6], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 3:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[4], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.Yellow, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[6], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 4:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[4], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.Yellow, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[6], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 5:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[4], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.Yellow, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[6], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 6:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[4], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.Yellow, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[3], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[6], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 7:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[4], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.Yellow, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[6], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 8:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[7], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.Yellow, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[0], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                break;
                            case 9:
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 5, narcArray[5].Width, narcArray[5].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 55, narcArray[4].Width, narcArray[4].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 105, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X, (int)position.Y + 155, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.FlipVertically, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 205 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 255 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 305 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(-1 * Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[5], new Rectangle((int)position.X + narcArray[3].Width / 2, (int)position.Y + 355 + narcArray[3].Height / 2, narcArray[3].Width, narcArray[3].Height), null, Color.White, (float)(Math.PI / 2), new Vector2(narcArray[4].Width / 2, narcArray[4].Height / 2), SpriteEffects.None, 0);
                                s.Draw(narcArray[8], new Rectangle((int)position.X, (int)position.Y + 405, narcArray[3].Width, narcArray[3].Height), null, Color.White, 0, new Vector2(), SpriteEffects.None, 0);
                                s.Draw(narcArray[1], new Rectangle((int)position.X, (int)position.Y + 455, narcArray[3].Width, narcArray[3].Height), null, Color.Yellow, 0, new Vector2(), SpriteEffects.None, 0);
                                break;

                        }
                        
                        
                        
                        
                        break;
                    default:
                        break;
                }
        }

        public void moveRight()
        {
            if (position.X < this.lockedRight.X)
                this.position = new Vector2(position.X + 8f, position.Y);
            else
                this.position.X = this.lockedRight.X;
        }
        public void moveLeft()
        {
            if (position.X > this.lockedLeft.X)
                this.position = new Vector2(position.X-8f, position.Y);
            else
                this.position.X = this.lockedLeft.X;
        }

        #endregion


    }
}
