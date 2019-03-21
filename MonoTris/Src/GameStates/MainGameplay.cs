using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTris
{
    public class MainGameplay : GameState
    {

        private Stage _stage;
        private Stage _preview;
        private StageDrawer _stageDrawer;
        private StageDrawer _previewDrawer;
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
        }

        public override void Initialize()
        {

            _gravityTimer = new Timer(1, true);
            _gravityTimer.OnTimeOut += GravityTick;

            // initialize the stage and its renderers

            var stageConfig = new StageConfiguration { Width = 10, Height = 16 };
            _stage = new Stage(stageConfig);
            _preview = new Stage(stageConfig);

            var stageDrawerConfig = new StageDrawerConfiguration
            {
                Position = Vector2.Zero,
                Scale = 2.5f,
            };

            _stageDrawer = new StageDrawer(stageDrawerConfig);
            _previewDrawer = new StageDrawer(stageDrawerConfig)
            {
                ColorMask = new Color(0.1f, 0.1f, 0.1f, 0.1f)
            };

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _gravityTimer.Update(gameTime);
        }

        public void GravityTick()
        {
            // restart the gravity timer no matter what
            _gravityTimer.Start();
        }
    }
}
