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
    class Score
    {
        // How much the score entity is worth in points
        public int pointVal;
        
        // How long the score should remain on screen
        public int time;

        // Should the score be drawn on screen
        public bool alive;

        // Position of Score to be drawn
        public Vector2 position;

        // Player Score done by
        public int player;

        public Score(int pointVal, int time, Vector2 position, bool alive, int player)
        {
            this.pointVal = pointVal;
            this.time = time;
            this.alive = alive;
            this.position = position;
            this.player = player;
        }
    }
}