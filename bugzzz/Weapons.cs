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
    class Weapons
    {
        public string[] names;
        public int[] damages;
        public float[] delays;
        Texture2D[] sprite;

        public Weapons(Texture2D a, Texture2D b, Texture2D c)
        {
            sprite = new Texture2D[3];
            names = new String[3];
            damages = new int[3];
            delays = new float[3];
            sprite[0] = a;
            names[0] = "Rifle";
            damages[0] = 100;
            delays[0] = 0.20f;
            sprite[1] = b;
            names[1] = "Shotgun";
            damages[1] = 10;
            delays[1] = 1.0f;
            sprite[2] = c;
            names[2] = "Flame Thrower";
            damages[2] = 100;
            delays[2] = 0.1f;

            //TODO::Add Fourth Weapon
        }
    }
}
