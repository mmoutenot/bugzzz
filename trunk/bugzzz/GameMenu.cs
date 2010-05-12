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
    class GameMenu
    {
        #region Fields (background, activated, nonActivated, state)
        private Texture2D background;       //background
        private Texture2D[] activeIcons;        //selected icon images
        private Texture2D[] normalIcons;        //unselected icon images
        private float[] widths;             // image widths
        private int state;              //state that is selected
        private Vector2 vp;             //viewport width and height
        private bool active;            //Bool if menu is active or not
        private bool select;            //Indicates if button is being pressed

        private float glow;
        private float progress;
        private float progInc;
        #endregion

        #region Accessors (Active)

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
        public bool Select
        {
            get
            {
                return select;
            }
            set
            {
                select = value;
            }
        }
        public int State
        {
            get
            {
                return state;
            }
        }

        #endregion


        /// <summary>
        /// Constructor for Game Menu
        /// </summary>
        /// <param name="inputs">Array of textures for menu</param>
        /// <param name="v">Game Viewport</param>
        public GameMenu(Texture2D[] inputs, Viewport v)
        {
            progress = 0f;
            progInc = 1.5f;
            active = true;
            select = false;
            state = 0;
            vp.X = v.Width;
            vp.Y = v.Height;
            activeIcons = new Texture2D[4];
            normalIcons = new Texture2D[4];
            widths = new float[4];

            for (int i = 0; i < 8; i++)
            {
                if (i % 4 == i)
                    activeIcons[i] = inputs[i];
                else
                    normalIcons[i - 4] = inputs[i];
            }

            background = inputs[8];

            widths[0] = normalIcons[0].Width;
            widths[1] = normalIcons[1].Width;
            widths[2] = normalIcons[2].Width;
            widths[3] = normalIcons[3].Width;

        }

        /// <summary>
        /// Draw function for Main Menu.
        /// Allows modularization of functions
        /// </summary>
        /// <param name="s">Game Spritebatch</param>
        public void Draw(SpriteBatch s)
        {
            
            s.Draw(background, new Rectangle(0, 0, (int)vp.X, (int)vp.Y), new Color(Color.White, (byte)(int)progress));

            if (progress >= 50)
            {
                Vector2 pos = new Vector2();
                pos.Y = (float) (vp.Y * 21 / 56 - .05 * vp.Y);
                pos.X = 0;
                for (int i = 0; i < 4; i++)
                {
                    pos.X = (float) (vp.X / 2 - widths[i] / 2 - vp.X * 0.1);
                    if (state == i)
                    {
                        s.Draw(this.activeIcons[i], new Rectangle((int)pos.X, (int)pos.Y, (int)widths[i], 158), Color.White);
                    }
                    else
                        s.Draw(this.normalIcons[i], new Rectangle((int)pos.X, (int)pos.Y, (int)widths[i], 158), Color.White);

                    pos.Y = pos.Y + 110;
                }
            }
            if (progress <= 150)
                progress += progInc;

            
            
        }


        //increments state (wraps around)
        public void stateInc()
        {
            glow = 0;
            if (state != 3)
                state++;
            else
                state = 0;
        }
        //decrements state (wrap around)
        public void stateDec()
        {
            if (state != 0)
                state--;
            else
                state = 3;
        }

    }
}
