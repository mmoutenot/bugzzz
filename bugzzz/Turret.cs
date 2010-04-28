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
    class Turret
    {
        public int bulletsLeft = 10;
        public float rotation = 0.0f;
        public Texture2D sprite = null;
        public bool fire;
        public Vector2 position = Vector2.Zero;
        public bool placed;
        public int closestEnemy;

        public Turret(Texture2D loadedTexture)
        {
            sprite = loadedTexture;
            placed = false;
            fire = false;
            closestEnemy = -1;
        }

    }
}
