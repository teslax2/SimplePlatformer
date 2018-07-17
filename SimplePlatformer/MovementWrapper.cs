using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlatformer
{
    public struct MovementWrapper
    {
        public Vector2 MovementToTry { get; private set; }
        public Vector2 FurthestAvailableLocationSoFar { get; set; }
        public int NumberOfStepsToBreakMovementInto { get; private set; }
        public bool IsDiagonalMove { get; private set; }
        public Vector2 OneStep { get; private set; }
        public Rectangle BoundingRectangle { get; set; }
        public Vector2 OriginalPosition { get; set; }

        public MovementWrapper(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            OriginalPosition = originalPosition;
            MovementToTry = destination - originalPosition;
            FurthestAvailableLocationSoFar = originalPosition;
            NumberOfStepsToBreakMovementInto = (int)(MovementToTry.Length() * 2) + 1;
            IsDiagonalMove = MovementToTry.X != 0 && MovementToTry.Y != 0;
            OneStep = MovementToTry / NumberOfStepsToBreakMovementInto;
            BoundingRectangle = boundingRectangle;
        }

    }
}
