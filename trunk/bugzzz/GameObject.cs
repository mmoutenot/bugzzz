﻿using System;
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

        private int totHealth;
        private int curHealth;
        private int damage;
        private int iD;

        #region Accessors
        public int Damage
        {
            get
            {
                return damage;
            }
        }
        public int ID
        {
            get
            {
                return iD;
            }
        }
        #endregion

        public GameObject(Texture2D loadedTexture, int ID)
        {
            alive = false;
            velocity = Vector2.Zero;
            rotation = 0.0f;
            position = Vector2.Zero;
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);

            this.iD = ID;
            // set the gameobjects score
            this.Reset(ID);
        }

        public void UpdateVelocity()
        {
            switch (iD)
            {
                case 1:
                    this.velocity.Normalize();
                    break;
                case 2:
                    this.velocity.Normalize();
                    this.velocity = new Vector2(this.velocity.X * .95f, this.velocity.Y*.85f);
                    break;
                case 3:
                    this.velocity.Normalize();
                    this.velocity = new Vector2(this.velocity.X*.5f, this.velocity.Y* .5f);
                    break;
                default:
                    this.velocity.Normalize();
                    break;

            }
        }
        public void Update(int loss)
        {
            curHealth -= loss;
            if (curHealth <= 0f)
            {
                Console.WriteLine("OWWWW");
                this.alive = false;

            }
        }

        public void Reset(int ID)
        {
            switch (ID)
            {
                case 1: //enemy 1
                    totHealth = 100;
                    curHealth = totHealth;
                    damage = 5;
                    score = 10;
                    break;
                case 2: //enemy 2
                    totHealth = 450;
                    curHealth = totHealth;
                    damage = 25;
                    score = 25;
                    break;
                case 3: //enemy 3
                    totHealth = 5000;
                    curHealth = totHealth;
                    damage = 50;
                    score = 100;
                    break;
                case 4: //default weapon
                    damage = 100;
                    break;
                case 5: //shotgun
                    damage = 175;
                    break;
                case 6: //flamethrower
                    damage = 75;
                    break;
                case 7: //turret bullet
                    damage = 25;
                    break;
                default:
                    totHealth = 100;
                    curHealth = totHealth;
                    damage = 5;
                    score = 10;
                    break;
            }

        }
    };
}
