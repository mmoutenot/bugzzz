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

            int hash = id * 17 + 111;
            rand = new Random(hash);
        }


        public void Update(GamePadState g, KeyboardState k)
        {
            if (active)
                NarcolepsyUpdate(g,k);
            else if (rand.Next(500) == 1)
            {
                active = true;
                //this.state = rand.Next(1,5);
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
            this.active = false;
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
                s.DrawString(dispFont, "Oh no!! Player " + playerID + " press A (Space) a lot!!!", new Vector2(350f, 150f), Color.Green);
        }

    }
}
