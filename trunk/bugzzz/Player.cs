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
        public float p_rotation = 0.0f;
        public float p_rotation_b = 0.0f;
        public Texture2D p_spriteT = null;
        public Texture2D p_spriteB = null;
        public Vector2 p_velocity = Vector2.Zero;
        public Vector2 p_position = Vector2.Zero;
        public bool p_fire;
        private int p_id;
        public bool deploy;
        public int health;
        public int score;
        public Weapons weapon;
        public int activeWeapon;

        public Player(int id, Texture2D spriteT, Texture2D spriteB, Weapons w, Vector2 pos)
        {
            deploy = false;
            p_id = id; 
            p_fire = false;
            p_position = pos;
            p_velocity = Vector2.Zero;
            p_spriteT = spriteT;
            p_spriteB = spriteB;
            p_rotation = 0.0f;
            p_rotation_b = 0.0f;
            health = 100;
            weapon = w;
            activeWeapon = 0;
        }
    }
}
