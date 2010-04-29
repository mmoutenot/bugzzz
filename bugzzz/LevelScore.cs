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
        public int tempscore;
        public bool player2_drawScore;
        public bool player1_drawScore;

        public LevelScore(int level, Player player1, Player player2, bool alive, int scrolltime)
        {
            this.level = level;
            this.player1 = player1;
            this.player2 = player2;
            this.alive = alive;
            this.scrolltime = scrolltime;
            this.tempscore = 0;
            this.player1_drawScore = true;
            this.player2_drawScore = false;
        }

        public void Draw(SpriteBatch s,Texture2D h, Viewport viewport)
        {
            //s.Draw(h, new Rectangle(viewport.Width / 4, viewport.Height / 4, viewport.Width / 2, viewport.Height / 2), new Color(Color.DarkBlue, (byte)(255)));
                if(player1_drawScore && tempscore<player1.score){
                    s.DrawString(levelfont, "Player 1 Score: " + tempscore,new Vector2((viewport.Width/4)+10,(viewport.Height/4)+10),new Color(Color.Yellow,(byte)(200)));
                    tempscore += 8;
                }
                else
                {
                    s.DrawString(levelfont, "Player 1 Score: " + player1.score, new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 10), new Color(Color.Yellow, (byte)(200)));
                    player2_drawScore = true;
                    player1_drawScore = false;
                    tempscore = 0;
               }
                if (player2_drawScore && tempscore < player2.score)
                {
                    s.DrawString(levelfont, "Player 2 Score: " + tempscore, new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 50), new Color(Color.Yellow, (byte)(200)));
                    tempscore += 8;
                }
                else
                {
                    s.DrawString(levelfont, "Player 2 Score: " + player2.score, new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 50), new Color(Color.Yellow, (byte)(200)));
                }
        }
    }
}