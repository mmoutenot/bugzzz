using System;
using System.Collections.Generic;
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
    class GameObject
    {
        public Texture2D sprite;
        public Vector2 velocity;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public bool alive;
        public int score;
        public GameObject(Texture2D loadedTexture)
        {
            alive = false;
            velocity = Vector2.Zero;
            rotation = 0.0f;
            position = Vector2.Zero;
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            
            // set the gameobjects score
            score = 20;
        }
    };
}
