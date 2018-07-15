﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlatformer
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public Rectangle Bounds { get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); } }

        public Sprite(Texture2D texture, Vector2 position, SpriteBatch spriteBatch)
        {
            Position = position;
            Texture = texture;
            SpriteBatch = spriteBatch;
        }
        /// <summary>
        /// Draws object
        /// </summary>
        public virtual void Draw()
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }
        
    }
}
