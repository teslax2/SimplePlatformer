using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformer
{
    public class Tile : Sprite
    {
        public bool IsBlocked { get; set; }
        public TileSlope TileSlope { get; set; }
        public Tile(Texture2D texture, Vector2 position, SpriteBatch spriteBatch, bool isBlocked) : base(texture, position, spriteBatch)
        {
            IsBlocked = isBlocked;
            TileSlope = TileSlope.Flat;
        }

        public Tile(Texture2D texture, Vector2 position, SpriteBatch spriteBatch, bool isBlocked, TileSlope tileSlope) : base(texture, position, spriteBatch)
        {
            IsBlocked = isBlocked;
            TileSlope = tileSlope;
        }

        public override void Draw()
        {
            if (IsBlocked) base.Draw();
        }
    }

    public enum TileSlope
    {
        Flat,
        Top,
        Left,
        Right,
    }
}
