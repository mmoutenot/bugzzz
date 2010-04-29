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
    class LevelScore
    {
        public int level;
        public Player player1;
        public Player player2;
        public bool alive;
        SpriteFont levelfont;
        SpriteBatch s;
        public int tempscore;
        Texture2D healthBar;

        public LevelScore(int level, Player player1, Player player2, bool alive, int scrolltime, SpriteFont levelfont, GraphicsDevice gDevice, Texture2D healthBar)
        {
            this.level = level;
            this.player1 = player1;
            this.player2 = player2;
            this.alive = alive;
            this.tempscore = 0;
            this.levelfont = levelfont;
            this.s = new SpriteBatch(gDevice);
            this.healthBar = healthBar;
        }

        public void Draw(Viewport viewport)
        {
            s.Begin();
            int s1, s2;
            if (tempscore <= player1.score)
                s1 = tempscore;
            else 
                s1 = player1.score;
            if (tempscore <= player2.score)
                s2 = tempscore;
            else 
                s2 = player2.score;


            s.Draw(healthBar, new Rectangle(viewport.Width / 4, viewport.Height / 4, viewport.Width / 2, viewport.Height / 2), new Color(Color.DarkBlue, (byte)(50)));
                
            s.DrawString(levelfont, "Player 1 Score: " + s1, new Vector2((viewport.Width/4)+10,(viewport.Height/4)+10),new Color(Color.Yellow,(byte)(200)));
            s.DrawString(levelfont, "Player 2 Score: " + s2, new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 50), new Color(Color.Yellow, (byte)(200)));
                
            if(tempscore < player1.score || tempscore < player2.score){
                tempscore += 5;
            }

            s.End();
        }
    }
}