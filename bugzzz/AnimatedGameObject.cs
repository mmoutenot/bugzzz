using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
    class AnimatedGameObject : GameObject
    {
        ArrayList sprites;
        int duration, curFrame, index;
        public AnimatedGameObject(ArrayList textures, int ID)
            : base((Texture2D) textures[0], ID)
        {
            this.sprites = textures;
            this.duration = 15;
            index = 0;
            curFrame = 0;
        }

        public void updateAnim()
        {
            if (curFrame >= duration - 1)
            {
                curFrame = 0;
                if (index == sprites.Count -1) {
                    index = 0;
                } else {
                    index += 1;
                }
            }
            else
            {
                curFrame++;
            }
            sprite = (Texture2D) sprites[index];
        }
    }
}