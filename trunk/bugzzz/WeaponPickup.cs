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
    class WeaponPickup
    {
        public Texture2D sprite;
        public Vector2 position;
        public int weaponIndex;

        public WeaponPickup(Texture2D loadedTexture,Vector2 pos, int index)
        {
            sprite = loadedTexture;
            position = pos;
            weaponIndex = index;
        }
    }
}
