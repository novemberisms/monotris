using System;
using System.Diagnostics;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoTris
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonoTris : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Stage _stage;
        StageDrawer _stageDrawer;

        public MonoTris()
        {
            _graphics = new GraphicsDeviceManager(this);
            // set the window dimensions
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();
            // let the mouse be visible
            IsMouseVisible = true;
            // set the root directory for content
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            var stageConfig = new StageConfiguration { Width = 10, Height = 16 };
            _stage = new Stage(stageConfig);
            _stageDrawer = new StageDrawer { Position = Vector2.Zero, Scale = 2.5F };

            Debug.WriteLine("Stage dimensions: {0}, {1}", _stage.Width, _stage.Height);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Images.LoadContent(Content);

            using (var writer = new StreamWriter("hello.txt"))
            {
                Debug.WriteLine("writing to hello.txt");
                Debug.WriteLine("pwd: {0}" ,System.IO.Directory.GetCurrentDirectory());
                writer.WriteLine("What'cha gonna do boi?");
            }

            var content = File.ReadAllText("Content/TetrominoShapes.json");
            Debug.WriteLine(content);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Teal);

            // this needs to be called this way so that we use the 'Nearest' filter instead of the 'Linear' one.
            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise
            );

            _stageDrawer.DrawStage(_spriteBatch, _stage);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
