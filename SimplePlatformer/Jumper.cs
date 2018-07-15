using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimplePlatformer
{
    class Jumper : Sprite
    {
        public Vector2 Movement { get; set; }
        private Vector2 _oldPosition;
        private int _prevUp;
        private float _speedLimit = 1f;

        public Jumper(Texture2D texture, Vector2 position, SpriteBatch spriteBatch) : base(texture, position, spriteBatch)
        {
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            AffectWithGravity();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
        }

        private void AffectWithGravity()
        {
            Movement += Vector2.UnitY * .5f;
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            _oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Board.CurrentBoard.WhereCanIGetTo(_oldPosition, Position, Bounds);
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        { 
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds *0.5f;
        }

        private void SimulateFriction()
        {
            if (IsOnFirmGround()) { Movement -= Movement * Vector2.One * 0.4f; }
            else { Movement -= Movement * Vector2.One * .055f; }
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            //get keyboard state
            KeyboardState keyboard = Keyboard.GetState();
            //change movement to reflect the keypress
            if (keyboard.IsKeyDown(Keys.Left)) { Movement += new Vector2(-1, 0); }
            if (keyboard.IsKeyDown(Keys.Right)) { Movement += new Vector2(1, 0); }
            if (keyboard.IsKeyDown(Keys.Up) && IsOnFirmGround() && _prevUp>5) { Movement = -Vector2.UnitY * 6f; _prevUp = 0; }
            if (keyboard.IsKeyUp(Keys.Up)) { if(_prevUp<int.MaxValue) _prevUp++; }
            MovementSpeedLimit();
        }

        private void MovementSpeedLimit()
        {
            var y = Movement.Y;
            if (Movement.X > _speedLimit) { Movement = new Vector2(_speedLimit, y); }
            else if(Movement.X < -_speedLimit) { Movement = new Vector2(-_speedLimit, y); }
        }

        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = Bounds;
            onePixelLower.Offset(0, 1);
            return !Board.CurrentBoard.HasRoomForRectangle(onePixelLower);
        }

        public void StopMovingIfBlocked()
        {
            var lastMovement = Position - _oldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }
    }
}
