using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MonoTris
{
    public class TitleScreen : GameState
    {

        private const String _gameTitle = "MONOTRIS";
        private const String _byLine = "Brian Sarfati 2019";

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawTitle(spriteBatch);
            DrawByLine(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyPressed(Keys.Space))
            {
                GameInstance.SwitchState<MainGameplay>();
            }
            InputManager.PostUpdate();
        }

        private void DrawTitle(SpriteBatch spriteBatch)
        {
            var textSize = Fonts.Title.MeasureString(_gameTitle);
            var drawPos = (ScreenSize - textSize) / 2;

            spriteBatch.DrawString(Fonts.Title, _gameTitle, drawPos, Color.White);
        }

        private void DrawByLine(SpriteBatch spriteBatch)
        {
            var textSize = Fonts.Regular.MeasureString(_byLine);
            var drawCenter = new Vector2(
                ScreenSize.X / 2,
                ScreenSize.Y - 32
            );
            var drawPos = drawCenter - (textSize / 2);

            spriteBatch.DrawString(Fonts.Regular, _byLine, drawPos, Color.White);
        }
    }
}
