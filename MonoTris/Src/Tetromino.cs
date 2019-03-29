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
        public MoveCollisionResponse Move(Stage stage, Vector2 direction)
        {
            return TryUpdate(stage, direction, Rotation);
        }

        /// <summary> returns `true` if the rotation was successful and `false` otherwise </summary>
        public MoveCollisionResponse Rotate(Stage stage, RotationDirection direction)
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

        public Vector2 FindEstimatedLandingPosition(Stage stage)
        {
            ApplyStage(stage, BlockColor.None, Position);
            var currentOffset = new Vector2();
            while (true)
            {
                if (CanMove(stage, currentOffset) == MoveCollisionResponse.None)
                {
                    currentOffset += Vector2.UnitY;
                }
                else
                {
                    currentOffset -= Vector2.UnitY;
                    break;
                }
            }
            ApplyStage(stage, Shape.Color, Position);
            return Position + currentOffset;
        }

        private MoveCollisionResponse TryUpdate(Stage stage, Vector2 moveDirection, ShapeRotation newRotation)
        {
            ApplyStage(stage, BlockColor.None, Position);

            var oldRotation = Rotation;
            Rotation = newRotation;

            var collisionResponse = CanMove(stage, moveDirection);

            if (collisionResponse == MoveCollisionResponse.None)
            {
                Position += moveDirection;
            }
            else
            {
                Rotation = oldRotation;
            }

            ApplyStage(stage, Shape.Color, Position);

            return collisionResponse;
        }

        public void ApplyStage(Stage stage, BlockColor color, Vector2 position)
        {
            var rotatedShape = Shape.GetShapeRotated(Rotation);
            var rotatedPivot = Shape.GetPivotRotated(Rotation);

            for (var y = 0; y < rotatedShape.GetLength(0); y++)
            {
                var gridY = position.Y - rotatedPivot.Y + y;
                for (var x = 0; x < rotatedShape.GetLength(1); x++)
                {
                    var gridX = position.X - rotatedPivot.X + x;
                    if (rotatedShape[y, x])
                    {
                        stage.SetCellColor((int)gridX, (int)gridY, color);
                    }
                }
            }
        }

        private MoveCollisionResponse CanMove(Stage stage, Vector2 direction)
        {
            var rotatedShape = Shape.GetShapeRotated(Rotation);
            var rotatedPivot = Shape.GetPivotRotated(Rotation);

            var rotatedWidth = rotatedShape.GetLength(1);
            var rotatedHeight = rotatedShape.GetLength(0);

            var projectedPosition = Position + direction;

            // check if the resulting position will put the bounding box of the block out of range of the stage
            var leftBoundary = (int)(projectedPosition.X - rotatedPivot.X); 
            var rightBoundary = (int)(projectedPosition.X - rotatedPivot.X) + rotatedWidth - 1;
            var topBoundary = (int)(projectedPosition.Y - rotatedPivot.Y);
            var bottomBoundary = (int)(projectedPosition.Y - rotatedPivot.Y) + rotatedHeight - 1;

            if (leftBoundary < 0) return MoveCollisionResponse.LeftWall;
            if (rightBoundary >= stage.Width) return MoveCollisionResponse.RightWall;
            if (topBoundary < 0) return MoveCollisionResponse.Ceiling; // when would this happen?
            if (bottomBoundary >= stage.Height) return MoveCollisionResponse.Floor;

            // check if the resulting position would make the blocks of the Tetromino overlap with any existing blocks in the stage
            for (var y = topBoundary; y <= bottomBoundary; y++)
            {
                for (var x = leftBoundary; x <= rightBoundary; x++)
                {
                    var shapeHasBlock = rotatedShape[
                        y - topBoundary,
                        x - leftBoundary
                    ];
                    if (shapeHasBlock == true && stage.GetCell(x, y).Value != BlockColor.None)
                    {
                        return MoveCollisionResponse.ExistingBlock;
                    }
                }
            }

            return MoveCollisionResponse.None;
        }
    }

    public enum MoveCollisionResponse
    {
        None,
        LeftWall,
        RightWall,
        Ceiling,
        Floor,
        ExistingBlock,
    }
}