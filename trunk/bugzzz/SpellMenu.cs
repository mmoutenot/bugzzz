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
    class SpellMenu
    {
        #region Fields (active, state, curState, position)

        private bool active;
        private int width;
        private int height;
        
        //contains different images indicating what selection has been made
        private Texture2D[] state;
        public int[] bulletsLeft;

        //index of current state
        private int curState;

        //current position of top left corner of spell box
        private Vector2 position;


        //positions of box when it is locked on or off screen
        private Vector2 lockedOff;
        private Vector2 lockedOn;

        #endregion

        #region Accessors (Width, Height, State, Active, CurState, Position)

        public int Width
        {
            get
            {
                return width;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
        }
        public Texture2D State
        {
            get
            {
                return state[curState];
            }
        }
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
        public int CurState
        {
            get
            {
                return curState;
            }
            set
            {
                curState = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        #endregion



        #region Main Methods (Constructor, returnState, stateInc, stateDec)
        public SpellMenu(Texture2D[] states, Viewport vp, int id)
        {
            state = new Texture2D[4];
            bulletsLeft = new int[4];

            for (int i = 0; i < 4; i++)
            {
                bulletsLeft[i] = 0;
                this.state[i] = states[i];
            }
            bulletsLeft[0] = -1;


            this.active = false;
            curState = 0;
            if (id == 1)
                position = new Vector2(100f, (float)vp.Height);
            else
                position = new Vector2((float)(vp.Width - 300f), (float)vp.Height);

            lockedOff = position;
            lockedOn = new Vector2(position.X, position.Y - 80f);
            width = 200;
            height = 75;




        }

        //increments state (wraps around)
        public void stateInc()
        {
            //skips weapons that aren't active
            while(true){
                if (curState != 3)
                    curState++;
                else
                    curState = 0;
                if (bulletsLeft[curState] != 0)
                    break;
            }
        }
        //decrements state (wrap around)
        public void stateDec()
        {
            //skips weapons that aren't active
            while (true)
            {

                if (curState != 0)
                    curState--;
                else
                    curState = 3;
                if (bulletsLeft[curState] != 0)
                    break;
            }
        }

        public void moveOn()
        {
            if (position != this.lockedOn)
                this.position = new Vector2(position.X, position.Y - 8f);
        }
        public void moveOff()
        {
            if (position != this.lockedOff)
                this.position = new Vector2(position.X, position.Y + 8f);
        }
        public void stateReset()
        {
            curState = 0;
        }

        #endregion


    }
}
