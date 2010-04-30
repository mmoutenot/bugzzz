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
    class Player
    {
        public float rotation = 0.0f;
        public float rotation_b = 0.0f;
        public Texture2D spriteT = null;
        public Texture2D spriteB = null;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 position = Vector2.Zero;

        public int type;

        public bool fire;
        public bool move1;
        public bool move2;
        public bool move3;

        private int id;
        public bool deploy;
        
        public int health;
        public int energy;

        public int score;
        public Weapons weapon;
        public int activeWeapon;
        public Statistics stat;
        public int livesLeft;

        public Player(int i, Texture2D t, Texture2D b, Weapons w, Vector2 pos, int ty, Statistics s)
        {
            energy = 100;
            deploy = false;
            id = i;
 
            fire = false;
            move1 = false;
            move2 = false;
            move3 = false;

            position = pos;
            velocity = Vector2.Zero;
            spriteT = t;
            spriteB = b;
            rotation = 0.0f;
            rotation_b = 0.0f;
            health = 100;
            weapon = w;
            activeWeapon = 0;
            stat = s;
            livesLeft = 5;

            type = ty;
        }
    }
}
