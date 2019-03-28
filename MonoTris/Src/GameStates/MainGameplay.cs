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
        private StageDrawer _stageDrawer;
        private StageDrawer _previewDrawer;
        private StageDrawer _borderDrawer;
        private Timer _gravityTimer;
        private Tetromino _activeTetromino;


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

            // spriteBatch.DrawString(Fonts.Title, "GAMEPLAY TIME!", new Vector2(0, 0), Color.White);
        }

        public override void Initialize()
        {
            _gravityTimer = new Timer(1, true);
            _gravityTimer.OnTimeOut += GravityTick;

            // initialize the stage and its renderers

            var stageConfig = new StageConfiguration { Width = 10, Height = 16 };
            _stage = new Stage(stageConfig);
            _preview = new Stage(stageConfig);
            _border = SetupBorder(stageConfig);

            var stageDrawerConfig = new StageDrawerConfiguration
            {
                Position = new Vector2(100, 100),
                Scale = 2.5f,
            };

            _stageDrawer = new StageDrawer(stageDrawerConfig);
            _previewDrawer = new StageDrawer(stageDrawerConfig)
            {
                ColorMask = new Color(0.1f, 0.1f, 0.1f, 0.1f)
            };
            _borderDrawer = SetupBorderDrawer(stageDrawerConfig);

            // TEMP: initialize the first active tetromino
            _activeTetromino = new Tetromino(TetrominoShapeDatabase.Shapes[6], new Vector2(5, 8));
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _gravityTimer.Update(gameTime);

            if (InputManager.IsKeyPressed(Keys.Up)) _activeTetromino.Move(_stage, -Vector2.UnitY);
            if (InputManager.IsKeyPressed(Keys.Down)) _activeTetromino.Move(_stage, Vector2.UnitY);
            if (InputManager.IsKeyPressed(Keys.Left)) _activeTetromino.Move(_stage, -Vector2.UnitX);
            if (InputManager.IsKeyPressed(Keys.Right)) _activeTetromino.Move(_stage, Vector2.UnitX);

            InputManager.PostUpdate();
        }

        public void GravityTick()
        {
            // restart the gravity timer no matter what
            _gravityTimer.Start();
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

        private void CreateNewTetromino()
        {

        }
    }
}
