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
    class ScoreDisplay
    {
        #region Fields
        // How much the score entity is worth in points
        private int pointVal;
        // How long the score should remain on screen
        private int time;
        // Should the score be drawn on screen
        private bool alive;
        // Position of Score to be drawn
        private Vector2 position;
        // Player Score done by
        private int player;
        #endregion

        #region Accessors
        public int PointVal
        {
            get
            {
                return pointVal;
            }
            set
            {
                pointVal = value;
            }
        }
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }
        public bool Alive
        {
            get
            {
                return alive;
            }
            set
            {
                alive = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set 
            {
                position = value;
            }
        }
        public int Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }
        #endregion

        public ScoreDisplay(int pointVal, int time, Vector2 position, bool alive, int player)
        {
            this.pointVal = pointVal;
            this.time = time;
            this.alive = alive;
            this.position = position;
            this.player = player;
        }
    }
}