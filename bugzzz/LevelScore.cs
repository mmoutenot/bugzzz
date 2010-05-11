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
        SpriteFont titlefont;
        SpriteBatch s;
        public int tempscore;
        Texture2D healthBar;
        bool scoreIsDone;
        bool avgLifeIsDone;
        bool spreeIsDone;
        bool livesLeftIsDone;
        bool gameOver;
        int alphaAvgLife;
        int alphaMaxSpree;
        int alphaLivesLeft;
        int alphaPressA;
        bool ascendingAlpha;

        public LevelScore(int level, Player player1, Player player2, bool alive, int scrolltime, SpriteFont levelfont, SpriteFont titlefont, GraphicsDevice gDevice, Texture2D healthBar, bool gameOver)
        {
            this.level = level;
            this.player1 = player1;
            this.player2 = player2;
            this.alive = alive;
            this.tempscore = 0;
            this.levelfont = levelfont;
            this.titlefont = titlefont;
            this.s = new SpriteBatch(gDevice);
            this.healthBar = healthBar;
            this.gameOver = gameOver;
            this.alphaAvgLife = 30;
            this.alphaMaxSpree = 30;
            this.alphaLivesLeft = 30;
            this.alphaPressA = 30;
            this.ascendingAlpha = true;
        }

        public void Draw(Viewport viewport)
        {
            s.Begin(SpriteBlendMode.AlphaBlend);
            int s1, s2;
            if (tempscore <= player1.score)
                s1 = tempscore;
            else
                s1 = player1.score;
            if (tempscore <= player2.score)
                s2 = tempscore;
            else
                s2 = player2.score;

            string levelStr = "Level " + level;
            if (gameOver)
            {
                levelStr = "Game Over";
            }
            Vector2 textSize = titlefont.MeasureString(levelStr);
            Vector2 textCenter = new Vector2(viewport.Width / 2, 50);

            s.DrawString(titlefont, levelStr, textCenter - (textSize / 2), new Color(Color.Yellow, (byte)(200)));
            s.Draw(healthBar, new Rectangle(viewport.Width / 8, viewport.Height / 4, viewport.Width * 3 / 4, viewport.Height / 2), new Color(Color.DarkBlue, (byte)(30)));

            // At the end of the level, halt the stats for p1,p2
            player1.stat.Started = false;
            player2.stat.Started = false;

            string avgLifeP1 = string.Format("{0:0.0}", player1.stat.averageLifeTime());
            string avgLifeP2 = string.Format("{0:0.0}", player2.stat.averageLifeTime());
            s.DrawString(levelfont, "Player 1", new Vector2((viewport.Width / 8) + 350, (viewport.Height / 4) + 40), new Color(Color.Yellow, (byte)(200)));
            s.DrawString(levelfont, "Player 2", new Vector2(((7 * viewport.Width) / 8) - (levelfont.MeasureString("Player 2").X) - 100, (viewport.Height / 4) + 40), new Color(Color.Yellow, (byte)(200)));
            /*
            s.DrawString(levelfont, "Score:       " + s1 + "                   " + s2, new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 100), new Color(Color.Red, (byte)(200)));
            s.DrawString(levelfont, "Avg Life:    " + avgLifeP1 + "s                  " + avgLifeP2+"s", new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 150), new Color(Color.Red, (byte)(200)));
            s.DrawString(levelfont, "Max Spree:   " + player1.stat.MaxSpreeLength + "                   " + player2.stat.MaxSpreeLength, new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 200), new Color(Color.Red, (byte)(200)));
            s.DrawString(levelfont, "Lives Left:  " + player1.healthBar.LivesLeft + "                     " + player2.healthBar.LivesLeft, new Vector2((viewport.Width / 4) + 10, (viewport.Height / 4) + 250), new Color(Color.Red, (byte)(200)));
            
             */
            if (!gameOver)
            {
                s.DrawString(levelfont, "Score:\nAvg Life:\nMax Spree:\nLives Left:", new Vector2((viewport.Width / 8) + 75, (viewport.Height / 4) + 100), new Color(Color.Red, (byte)(200)));
            }
            else
            {
                s.DrawString(levelfont, "Score:\nAvg Life:\nMax Spree:\nFav Weapon:", new Vector2((viewport.Width / 8) + 75, (viewport.Height / 4) + 100), new Color(Color.Red, (byte)(200)));
            }

            s.DrawString(levelfont, s1 + "", new Vector2((viewport.Width / 8) + 350, (viewport.Height / 4) + 100), new Color(Color.Red, (byte)(200)));
            s.DrawString(levelfont, s2 + "", new Vector2(((7 * viewport.Width) / 8) - (levelfont.MeasureString("Player 2").X) - 100, (viewport.Height / 4) + 100), new Color(Color.Red, (byte)(200)));
            if (scoreIsDone)
            {
                s.DrawString(levelfont, avgLifeP1, new Vector2((viewport.Width / 8) + 350, (viewport.Height / 4) + 150), new Color(Color.Red, (byte)(alphaAvgLife)));
                s.DrawString(levelfont, avgLifeP2, new Vector2(((7 * viewport.Width) / 8) - (levelfont.MeasureString("Player 2").X) - 100, (viewport.Height / 4) + 150), new Color(Color.Red, (byte)(alphaAvgLife)));
                if (alphaAvgLife > 100)
                {
                    avgLifeIsDone = true;
                }
                if (alphaAvgLife < 200)
                {
                    alphaAvgLife++;
                }
            }
            if (avgLifeIsDone)
            {
                s.DrawString(levelfont,player1.stat.MaxSpreeLength+"", new Vector2((viewport.Width / 8) + 355, (viewport.Height / 4) + 200), new Color(Color.Red, (byte)(alphaMaxSpree)));
                s.DrawString(levelfont,player2.stat.MaxSpreeLength+"", new Vector2(((7 * viewport.Width) / 8) - (levelfont.MeasureString("Player 2").X) - 100, (viewport.Height / 4) + 200), new Color(Color.Red, (byte)(alphaMaxSpree)));
                if (alphaMaxSpree > 100)
                {
                    spreeIsDone = true;
                }
                if (alphaMaxSpree < 200)
                {
                    alphaMaxSpree++;
                }
            }
            if (spreeIsDone)
            {
                if (!gameOver)
                {
                    s.DrawString(levelfont, player1.healthBar.LivesLeft + "", new Vector2((viewport.Width / 8) + 360, (viewport.Height / 4) + 250), new Color(Color.Red, (byte)(alphaLivesLeft)));
                    s.DrawString(levelfont, player2.healthBar.LivesLeft + "", new Vector2(((7 * viewport.Width) / 8) - (levelfont.MeasureString("Player 2").X) - 100, (viewport.Height / 4) + 250), new Color(Color.Red, (byte)(alphaLivesLeft)));
                }
                else
                {
                    s.DrawString(levelfont, player1.stat.favoritePickup(), new Vector2((viewport.Width / 8) + 360, (viewport.Height / 4) + 250), new Color(Color.Red, (byte)(alphaLivesLeft)));
                    s.DrawString(levelfont, player2.stat.favoritePickup(), new Vector2(((7 * viewport.Width) / 8) - (levelfont.MeasureString("Player 2").X) - 100, (viewport.Height / 4) + 250), new Color(Color.Red, (byte)(alphaLivesLeft)));
                }
                if (alphaLivesLeft > 100)
                    {
                        livesLeftIsDone = true;
                    }
                if (alphaLivesLeft < 200)
                {
                    alphaLivesLeft++;
                }

            }
            if (livesLeftIsDone)
            {
                s.DrawString(levelfont, "Press (A) to continue", new Vector2((viewport.Width / 4)+170, (3* viewport.Height / 4) - 50), new Color(Color.Blue, (byte)(alphaPressA)));
                //Console.WriteLine("Alpha:" + alphaPressA);
                if (ascendingAlpha)
                {
                    if (alphaPressA > 150)
                    {
                        ascendingAlpha = false;
                    }
                    else
                    {
                        alphaPressA+=4;
                    }
                }
                else
                {
                    if (alphaPressA <= 30)
                    {
                        ascendingAlpha = true;
                    }
                    else
                    {
                        alphaPressA-=4;
                    }
                }

            }
            if (tempscore < player1.score || tempscore < player2.score)
            {
                if (tempscore < 200)
                    tempscore += 5;
                else if (tempscore < 500)
                    tempscore += 10;
                else
                    tempscore += 20;
            }
            else
            {
                scoreIsDone = true;
            }

            s.End();
        }
    }

}