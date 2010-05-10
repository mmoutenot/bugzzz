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
        private int state;
        private bool pressed;
        private int curLevel;
        private SpriteFont dispFont;
        private Random rand;
        private int playerID;
        private Keys[] konamiKeys;
        private Buttons[] konamiButtons;
        private int kCIndex;

        //used for updateOne
        //indicates how many times a button has been pressed
        private int pressCount;
        private int totalCount;
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
        public NarcolepsyEffect(SpriteFont f, int id)
        {
            this.active = false;
            this.state = 0;
            this.pressed = false;
            this.curLevel = 1;
            this.totalCount = 10;
            this.pressCount = 0;
            dispFont = f;
            this.playerID = id;

            int hash = id*DateTime.Now.Second + 791;
            rand = new Random(hash);

            kCIndex = 0;
            konamiKeys = new Keys[10];
            konamiButtons = new Buttons[10];

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


        }


        public void Update(GamePadState g, KeyboardState k)
        {
            if (active)
                NarcolepsyUpdate(g,k);
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
            
        }

        private void NarcolepsyUpdate(GamePadState g, KeyboardState k)
        {
            switch (state)
            {
                case 1:
                    updateOne(g,k);
                    break;
                case 2:
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
                else if (g.IsButtonDown(konamiButtons[kCIndex]))
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
            //TODO:: Needs to be fixed so that it works smoother
            if (this.active)
            {
                switch (state)
                {
                    case 1:
                        s.DrawString(dispFont, "Oh no!! Player " + playerID + " press A (Space) a lot!!!", new Vector2(350f, 150f), Color.Green);
                        break;
                    case 2:
                        s.DrawString(dispFont, "Player " + playerID + " Enter the Konami Code!!!", new Vector2(350f, 150f), Color.Green);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion


    }
}
