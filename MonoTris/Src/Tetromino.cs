using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoTris
{
    public class Tetromino
    {
        public readonly TetrominoShape Shape;
        public Vector2 Position { get; private set; }
        public ShapeRotation Rotation { get; private set; }

        public Tetromino(TetrominoShape shape, Vector2 position)
        {
            Shape = shape;
            Position = position;
        }

        /// <summary> returns `true` if the move was successful and `false` otherwise </summary>
        public bool Move(Stage stage, Vector2 direction)
        {
            return TryUpdate(stage, direction, Rotation);
        }

        /// <summary> returns `true` if the rotation was successful and `false` otherwise </summary>
        public bool Rotate(Stage stage, RotationDirection direction)
        {
            var finalRotation = Rotation;
            switch (Rotation)
            {
                case ShapeRotation.Zero:
                    finalRotation = direction == RotationDirection.Clockwise 
                        ? ShapeRotation.Ninety
                        : ShapeRotation.TwoSeventy;
                    break;
                case ShapeRotation.Ninety:
                    finalRotation = direction == RotationDirection.Clockwise
                        ? ShapeRotation.OneEighty
                        : ShapeRotation.Zero;
                    break;
                case ShapeRotation.OneEighty:
                    finalRotation = direction == RotationDirection.Clockwise
                        ? ShapeRotation.TwoSeventy
                        : ShapeRotation.Ninety;
                    break;
                case ShapeRotation.TwoSeventy:
                    finalRotation = direction == RotationDirection.Clockwise
                        ? ShapeRotation.Zero
                        : ShapeRotation.OneEighty;
                    break;

            }
            return TryUpdate(stage, new Vector2(), finalRotation);
        }

        private bool TryUpdate(Stage stage, Vector2 moveDirection, ShapeRotation newRotation)
        {
            var success = false;
            ApplyStage(stage, BlockColor.None);

            if (moveDirection.LengthSquared() > 0 && CanMove(stage, moveDirection))
            {
                Position += moveDirection;
                success = true;
            }

            ApplyStage(stage, Shape.Color);
            return success;
        }

        private void ApplyStage(Stage stage, BlockColor color)
        {
            var rotatedShape = Shape.GetShapeRotated(Rotation);
            var rotatedPivot = Shape.GetPivotRotated(Rotation);

            for (var y = 0; y < rotatedShape.GetLength(0); y++)
            {
                var gridY = Position.Y - rotatedPivot.Y + y;
                for (var x = 0; x < rotatedShape.GetLength(1); x++)
                {
                    var gridX = Position.X - rotatedPivot.X + x;
                    if (rotatedShape[y, x])
                    {
                        stage.SetCellColor((int)gridX, (int)gridY, color);
                    }
                }
            }
        }

        private bool CanMove(Stage stage, Vector2 direction)
        {
            var rotated = Shape.GetShapeRotated(Rotation);
            var rotatedPivot = Shape.GetPivotRotated(Rotation);

            var rotatedWidth = rotated.GetLength(1);
            var rotatedHeight = rotated.GetLength(0);

            var projectedPosition = Position + direction;

            // check if the resulting position will put the bounding box of the block out of range of the stage
            var leftBoundary = (int)(projectedPosition.X - rotatedPivot.X); 
            var rightBoundary = (int)(projectedPosition.X - rotatedPivot.X) + rotatedWidth - 1;
            var topBoundary = (int)(projectedPosition.Y - rotatedPivot.Y);
            var bottomBoundary = (int)(projectedPosition.Y - rotatedPivot.Y) + rotatedHeight - 1;

            if (leftBoundary < 0) return false;
            if (rightBoundary >= stage.Width) return false;
            if (topBoundary < 0) return false;
            if (bottomBoundary >= stage.Height) return false;

            return true;
        }
    }
}