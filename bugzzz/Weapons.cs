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
        public string name;
        public int damage;
        public int id;
        Texture2D[] sprite;

        public Weapons(Texture2D a, Texture2D b, Texture2D c)
        {
            sprite = new Texture2D[3];
            sprite[0] = a;
            sprite[1] = b;
            sprite[2] = c;
            id = 1;
        }
    }
}
