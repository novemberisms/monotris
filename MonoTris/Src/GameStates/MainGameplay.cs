using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoTris
{
    public class MainGameplay : GameState
    {
        private Stage _stage;
        private Stage _preview;
        private Stage _border;
        private Stage _nextPiece;
        private StageDrawer _stageDrawer;
        private StageDrawer _previewDrawer;
        private StageDrawer _borderDrawer;
        private StageDrawer _nextPieceDrawer;
        private Timer _gravityTimer;
        private Tetromino _activeTetromino;
        private Tetromino _nextTetromino;
        private RandomGenerator _randomGenerator;


        public override void Cleanup()
        {
            base.Cleanup();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw the stage and the transparent preview above it

            _stageDrawer.DrawStage(spriteBatch, _stage);
            _previewDrawer.DrawStage(spriteBatch, _preview);
            _borderDrawer.DrawStage(spriteBatch, _border);
            _nextPieceDrawer.DrawStage(spriteBatch, _nextPiece);

            // spriteBatch.DrawString(Fonts.Title, "GAMEPLAY TIME!", new Vector2(0, 0), Color.White);
        }

        public override void Initialize()
        {
            _gravityTimer = new Timer(0.5, true);
            _gravityTimer.OnTimeOut += GravityTick;

            // initialize the stage and its renderers
            var stageConfig = new StageConfiguration { Width = 10, Height = 20 };
            _stage = new Stage(stageConfig);
            _preview = new Stage(stageConfig);
            _border = SetupBorder(stageConfig);

            var stageDrawerConfig = new StageDrawerConfiguration
            {
                Position = new Vector2(50, 50),
                Scale = 2f,
            };

            _stageDrawer = new StageDrawer(stageDrawerConfig);
            _previewDrawer = new StageDrawer(stageDrawerConfig)
            {
                ColorMask = new Color(0.25f, 0.25f, 0.25f, 0.25f)
            };
            _borderDrawer = SetupBorderDrawer(stageDrawerConfig);

            // initialize the next piece preview drawer
            var nextPieceConfig = new StageConfiguration { Width = 4, Height = 4 };
            _nextPiece = new Stage(nextPieceConfig);
            var stageDrawEnd = _stageDrawer.Position + _stageDrawer.GetDrawDimensions(_stage);
            var nextPieceDrawerConfig = new StageDrawerConfiguration
            {
                Position = new Vector2(stageDrawEnd.X + 50, _stageDrawer.Position.Y),
                Scale = 2f,
            };
            _nextPieceDrawer = new StageDrawer(nextPieceDrawerConfig);

            // initialize the random shape generator
            _randomGenerator = new RandomGenerator();

            // initialize the first active tetromino and the next one
            CreateNewTetromino();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _gravityTimer.Update(gameTime);

            if (InputManager.IsKeyPressed(Keys.Up)) FastFall();
            else if (InputManager.IsKeyPressed(Keys.Down)) PerformMove(Vector2.UnitY);
            else if (InputManager.IsKeyPressed(Keys.Left)) PerformMove(-Vector2.UnitX);
            else if (InputManager.IsKeyPressed(Keys.Right)) PerformMove(Vector2.UnitX);
            else if (InputManager.IsKeyPressed(Keys.Z)) PerformRotation(RotationDirection.CounterClockwise);
            else if (InputManager.IsKeyPressed(Keys.X)) PerformRotation(RotationDirection.Clockwise);

            InputManager.PostUpdate();
        }

        public void GravityTick()
        {
            if (_activeTetromino == null) return;
            PerformMove(Vector2.UnitY);
        }

        /// <summary> 
        /// Moves the active tetromino in the specified `direction`, and if it fails, creates a new active tetromino
        /// and resets the `_gravityTimer`.
        /// </summary>
        private void PerformMove(Vector2 direction)
        {
            var moveResponse = _activeTetromino.Move(_stage, direction);
            var shouldEndTetrominoControl = false;

            switch (moveResponse)
            {
                case MoveCollisionResponse.None:
                case MoveCollisionResponse.LeftWall:
                case MoveCollisionResponse.RightWall:
                case MoveCollisionResponse.Ceiling:
                    break;
                case MoveCollisionResponse.ExistingBlock:
                case MoveCollisionResponse.Floor:
                    if (direction.Y > 0) shouldEndTetrominoControl = true;
                    break;
            }

            if (shouldEndTetrominoControl)
            {
                EndTetrominoControl();
            }

            UpdatePreview();
        }

        private void EndTetrominoControl()
        {
            ClearCompletedLines();
            CreateNewTetromino();
            _gravityTimer.Start();
        }

        private void PerformRotation(RotationDirection direction)
        {
            _activeTetromino.Rotate(_stage, direction);
            UpdatePreview();
        }

        private void FastFall()
        {
            var fallPosition = _activeTetromino.FindEstimatedLandingPosition(_stage);
            _activeTetromino.Move(_stage, fallPosition - _activeTetromino.Position);
            EndTetrominoControl();
        }

        private Stage SetupBorder(StageConfiguration playAreaConfig)
        {
            var config = new StageConfiguration()
            {
                Width = playAreaConfig.Width + 2,
                Height = playAreaConfig.Height + 2,
            };

            var border = new Stage(config);

            // left and right wall. we're starting from 1 because we're not going to
            // add a top wall and so the top of the boundary aligns with the top of the
            // play area
            for (var y = 1; y < border.Height; y++)
            {
                border.SetCellColor(0, y, BlockColor.Gray);
                border.SetCellColor(border.Width - 1, y, BlockColor.Gray);
            }
            // bottom
            for (var x = 0; x < border.Width; x++)
            {
                border.SetCellColor(x, border.Height - 1, BlockColor.Gray);
            }

            return border;
        }

        private StageDrawer SetupBorderDrawer(StageDrawerConfiguration playAreaConfig)
        {
            var config = new StageDrawerConfiguration()
            {
                Position = playAreaConfig.Position - new Vector2(StageDrawer.TileSize * playAreaConfig.Scale),
                Scale = playAreaConfig.Scale,
            };

            return new StageDrawer(config);
        }

        private MoveCollisionResponse CreateNewTetromino()
        {
            _activeTetromino = _nextTetromino ?? new Tetromino(_randomGenerator.GenerateShape(), new Vector2(1, 1));
            _nextTetromino = new Tetromino(_randomGenerator.GenerateShape(), new Vector2(1, 1));
            _nextPiece.ClearAll();
            _nextTetromino.ApplyStage(_nextPiece, _nextTetromino.Shape.Color, new Vector2(1));
            return _activeTetromino.Move(_stage, new Vector2());
        }

        private void ClearCompletedLines()
        {
            _stage.ClearCompletedLines();
        }

        private void UpdatePreview()
        {
            _preview.ClearAll();
            var fallPosition = _activeTetromino.FindEstimatedLandingPosition(_stage);
            _activeTetromino.ApplyStage(_preview, _activeTetromino.Shape.Color, fallPosition);
        }
    }
}
