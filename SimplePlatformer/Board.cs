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
        public Texture2D TileTexture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public static Board CurrentBoard { get; private set; }

        /// <summary>
        /// Define board dimensions and tile texture
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        /// <param name="tileTexture"></param>
        /// <param name="spriteBatch"></param>
        public Board(int columns, int rows, Texture2D tileTexture, SpriteBatch spriteBatch)
        {
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            SpriteBatch = spriteBatch;
            CreateNewBoard();
            CurrentBoard = this;
        }

        public void CreateNewBoard()
        {
            InitializeAllTilesAndBlockSomeOnly();
            SetAllBorderTilesBlocked();
            SetTopLeftTileUnblocked();
        }

        private void SetTopLeftTileUnblocked()
        {
            Tiles[1, 1].IsBlocked = false;
        }

        private void InitializeAllTilesAndBlockSomeOnly()
        {
            Tiles = new Tile[Columns, Rows];
            var random = new Random();

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Tiles[x, y] = new Tile(TileTexture, new Vector2(TileTexture.Width * x, TileTexture.Height * y), SpriteBatch, random.Next(5) == 0);
                }
            }
        }

        private void DrawBoardByCreator()
        {
            Tiles = new Tile[Columns, Rows];
            SetAllBorderTilesBlocked();
            SetTopLeftTileUnblocked();
            MapCreator.StartDrawing();
        }

        /// <summary>
        /// Draw border from tiles
        /// </summary>
        private void SetAllBorderTilesBlocked()
        {
            for(int x=0; x < Columns; x++)
            {
                for(int y=0; y < Rows; y++)
                {
                    if (x == 0 || x == Columns-1 || y == 0 || y == Rows - 1)
                    {
                        Tiles[x, y].IsBlocked = true;
                    }
                }
            }
        }

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

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach(var tile in Tiles)
            {
                if(tile.IsBlocked && tile.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }
            return true;
        }

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

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }
    }

}
