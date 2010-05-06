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
        #region Fields
        
        //contains different images indicating what selection has been made
        private Texture2D[] state;

        //index of current state
        private int curState;

        //position of top left corner of spell box
        private Vector2 position;

        #endregion


        #region Main Methods Constructor, returnState, stateInc, stateDec
        public SpellMenu(Texture2D[] states)
        {
            state = new Texture2D[4];

            for (int i = 0; i < 4; i++)
            {
                this.state[i] = states[i];
            }

            curState = 0;

        }

        public int returnState()
        {
            return curState+1;
        }

        //increments state (wraps around)
        public void stateInc()
        {
            if (curState != 3)
                curState++;
            else
                curState = 0;
        }
        //decrements state (wrap around)
        public void stateDec()
        {
            if (curState != 0)
                curState--;
            else
                curState = 3;
        }


        #endregion


    }
}
