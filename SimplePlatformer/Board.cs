using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlatformer
{
    class Board
    {
        private int _columns;
        private int _rows;

        public Tile[,] Tiles { get; set; }
        public int Columns { get { return _columns; } set { if (value >= 0) { _columns = value; } } }
        public int Rows { get { return _rows; } set { if (value >= 0) { _rows = value; } } }
        public Texture2D[] TileTexture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public static Board CurrentBoard { get; private set; }
        public bool CreatorOn { get; set; }

        /// <summary>
        /// Define board dimensions and tile texture
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        /// <param name="tileTexture"></param>
        /// <param name="spriteBatch"></param>
        public Board(int columns, int rows, Texture2D[] tileTexture, SpriteBatch spriteBatch)
        {
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            SpriteBatch = spriteBatch;
            CreateNewBoard();
            CurrentBoard = this;
        }
        #region Init
        public void CreateNewBoard()
        {
            InitializeAllTilesAndBlockSomeOnly();
            SetAllBorderTilesBlocked();
            SetTopLeftTileUnblocked();
            SetDifferentTextureForSurfaceTiles();
        }

        private void InitializeAllTilesAndBlockSomeOnly()
        {
            Tiles = new Tile[Columns, Rows];
            var random = new Random();

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Tiles[x, y] = new Tile(TileTexture[0], new Vector2(TileTexture[0].Width * x, TileTexture[0].Height * y), SpriteBatch, random.Next(5) == 0);
                }
            }
        }

        private void SetAllBorderTilesBlocked()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
                    {
                        Tiles[x, y].IsBlocked = true;
                    }
                }
            }
        }

        private void SetTopLeftTileUnblocked()
        {
            Tiles[1, 1].IsBlocked = false;
        }

        private void SetDifferentTextureForSurfaceTiles()
        {
            for(int y=1; y<Rows; y++)
            {
                for(int x=0; x<Columns; x++)
                {
                    if (!Tiles[x, y - 1].IsBlocked) { Tiles[x, y].Texture = TileTexture[1]; }
                    else if (Tiles[x, y - 1].IsBlocked) { Tiles[x, y].Texture = TileTexture[0]; }
                }
            }
        }
        #endregion

        #region Creator
        public void ClearAllInnerTiles()
        {
            for(int x = 1; x < Columns-1; x++)
            {
                for (int y = 1; y < Rows-1; y++)
                {
                    Tiles[x, y].IsBlocked = false;
                }
            }

        }
        
        public void DrawTileByMouse(Vector2 vector2,bool drawOrRemove)
        {
            var column = (int)vector2.X / TileTexture[0].Width;
            var row = (int)vector2.Y / TileTexture[0].Height;
            if(row >= Rows || column >= Columns) { return; }
            Tiles[column, row].IsBlocked = drawOrRemove;
            SetDifferentTextureForSurfaceTiles();
        }
        #endregion

        /// <summary>
        /// Draw all tiles on the board
        /// </summary>
        public void Draw()
        {
            foreach(var tile in Tiles)
            {
                tile.Draw();
            }
        }

        #region Collision


        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            var move = new MovementWrapper(originalPosition, destination, boundingRectangle);

            for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + move.OneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                if (HasRoomForRectangle(newBoundary)) { move.FurthestAvailableLocationSoFar = positionToTry; }
                else
                {
                    if (move.IsDiagonalMove)
                    {
                        int stepsLeft = move.NumberOfStepsToBreakMovementInto - (i - 1);
                        move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                    }
                    break;
                }
            }
            return move.FurthestAvailableLocationSoFar;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (var tile in Tiles)
            {
                if (tile.IsBlocked && tile.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }
            return true;
        }

        private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
        {
            if (wrapper.IsDiagonalMove)
            {
                int stepsLeft = wrapper.NumberOfStepsToBreakMovementInto - (i - 1);

                Vector2 remainingHorizontalMovement = wrapper.OneStep.X * Vector2.UnitX * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingHorizontalMovement, wrapper.BoundingRectangle);

                Vector2 remainingVerticalMovement = wrapper.OneStep.Y * Vector2.UnitY * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingVerticalMovement, wrapper.BoundingRectangle);
            }

            return wrapper.FurthestAvailableLocationSoFar;
        }

        #endregion
    }

}
