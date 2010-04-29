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
        public int scrolltime;
        SpriteFont levelfont;
        SpriteBatch s;
        public int tempscore;
        public bool player2_drawScore;
        public bool player1_drawScore;

        public LevelScore(int level, Player player1, Player player2, bool alive, int scrolltime, SpriteFont levelfont, GraphicsDevice gDevice)
        {
            this.level = level;
            this.player1 = player1;
            this.player2 = player2;
            this.alive = alive;
            this.scrolltime = scrolltime;
            this.tempscore = 0;
            this.levelfont = levelfont;
            this.player1_drawScore = true;
            this.player2_drawScore = false;
            this.s = new SpriteBatch(gDevice);
        }

        public void Draw(Texture2D h, Viewport viewport)
        {
            s.Begin();
            int s1, s2;
            if(tempscore < player1.score || tempscore < player2.score){
                if (tempscore <= player1.score)
                    s1 = tempscore;
                else 
                    s1 = player1.score;
                if (tempscore <= player2.score)
                    s2 = tempscore;
                else 
                    s2 = player2.score;


                s.Draw(healthBar, new Rectangle(this.viewport.Width / 4, this.viewport.Height / 4, this.viewport.Width / 2, this.viewport.Height / 2), new Color(Color.DarkBlue, (byte)(32)));
                    
                s.DrawString(levelfont, "Player 1 Score: " + s1, new Vector2((viewport.Width/4)+10,(viewport.Height/4)+10),new Color(Color.Yellow,(byte)(200)));
                s.DrawString(levelfont, "Player 2 Score: " + s2, new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 50), new Color(Color.Yellow, (byte)(200)));
                tempscore += 8;
            }
            s.End();
        }
    }
}