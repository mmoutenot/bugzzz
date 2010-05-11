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
    class PauseMenu
    {

        #region Fields (background, activated, nonActivated, state)
        private Texture2D background;
        private Texture2D[] activeIcons;
        private Texture2D[] normalIcons;

        private int state;
        private Vector2 vp;
        private bool active;
        private bool select;

        private float glow;
        private float progress;
        private float progInc;

        #endregion

        #region Accessors

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

        public PauseMenu(Texture2D[] inputs, Viewport v)
        {
            progress = 0f;
            progInc = 1.5f;
            active = false;
            select = false;
            state = 0;
            vp.X = v.Width;
            vp.Y = v.Height;
            activeIcons = new Texture2D[3];
            normalIcons = new Texture2D[3];

            for (int i = 0; i < 6; i++)
            {
                if (i % 3 == i)
                    activeIcons[i] = inputs[i];
                else
                    normalIcons[i - 3] = inputs[i];
            }
            
            background = inputs[6];

        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(background, new Rectangle(0, 0, (int)vp.X, (int)vp.Y), new Color(Color.White, (byte)50));
            Vector2 pos = new Vector2();
            pos.Y = vp.Y*21/56;
            pos.X = 0;
            for (int i = 0; i < 3; i++)
            {
                pos.X = vp.X / 2 - activeIcons[i].Width / 2;
                if (state == i)
                    s.Draw(this.activeIcons[i], new Rectangle((int)pos.X, (int)pos.Y, (int)activeIcons[i].Width, (int)activeIcons[i].Height), Color.White);
                else
                    s.Draw(this.normalIcons[i], new Rectangle((int)pos.X, (int)pos.Y, (int)activeIcons[i].Width, (int)activeIcons[i].Height), Color.White);
                pos.Y = pos.Y + 110;
            }
        }

        public void stateInc()
        {
            if (state != 2)
                state++;
            else
                state = 0;
        }
        public void stateDec()
        {
            if (state != 0)
                state--;
            else
                state = 2;
        }
    }
}